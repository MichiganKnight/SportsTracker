using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs.Team;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.Services
{
    public interface ITeamService
    {
        Task<TeamSchedule?> GetScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default);
        Task<TeamRoster?> GetRosterAsync(League league, string teamId, CancellationToken cancellationToken = default);
        Task<TeamDetails?> GetDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default);
        Task<TeamStats?> GetStatsAsync(League league, string teamId, CancellationToken cancellationToken = default);
        Task<TeamInjuries?> GetInjuriesAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }

    public sealed class TeamService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamService
    {
        private readonly CacheOptions _cache = cacheOptions.Value;

        public Task<TeamSchedule?> GetScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamScheduleResponseDto, TeamSchedule>(league, $"Schedule for {teamId}", CacheKeys.TeamSchedule(league, teamId), EspnEndpoints.TeamSchedule(league, teamId),
                dto => TeamScheduleMapper.Map(dto, league, teamId), _ => TimeSpan.FromMinutes(_cache.TeamScheduleMinutes), logger, cancellationToken);
        }

        public Task<TeamRoster?> GetRosterAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamRosterResponseDto, TeamRoster>(league, $"Roster for {teamId}", CacheKeys.TeamRoster(league, teamId), EspnEndpoints.TeamRoster(league, teamId), dto => TeamRosterMapper.Map(dto, league, teamId),
                _ => TimeSpan.FromMinutes(_cache.TeamRosterMinutes), logger, cancellationToken);
        }

        public Task<TeamDetails?> GetDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamDetailsResponseDto, TeamDetails>(league, $"Details for {teamId}", CacheKeys.TeamDetails(league, teamId), EspnEndpoints.TeamDetails(league, teamId), dto => TeamDetailsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cache.TeamMinutes), logger, cancellationToken);
        }

        public Task<TeamStats?> GetStatsAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            int season = DateTime.UtcNow.Year;
            const int seasonType = 2;

            return GetOrFetchAsync<TeamStatsResponseDto, TeamStats>(league, $"Stats for {teamId}", CacheKeys.TeamStats(league, teamId, season, seasonType), EspnEndpoints.TeamStats(league, teamId, season, seasonType),
                dto => TeamStatsMapper.Map(dto, season, seasonType), _ => TimeSpan.FromMinutes(_cache.TeamMinutes), logger, cancellationToken);
        }

        public async Task<TeamInjuries?> GetInjuriesAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.TeamInjuries(league, teamId);

            TeamInjuries? cached = await cache.GetAsync<TeamInjuries>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }

            logger.LogInformation("Fetching {League} Injuries for {TeamId}", league, teamId);

            List<TeamInjury> injuries = await FetchInjuryRecordsAsync(league, teamId, cancellationToken);

            TeamInjuries result = new()
            {
                Injuries = injuries.OrderByDescending(injury => injury.Date).ToList()
            };

            await cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(_cache.TeamMinutes));

            return result;
        }

        private async Task<List<TeamInjury>> FetchInjuryRecordsAsync(League league, string teamId, CancellationToken cancellationToken)
        {
            List<TeamInjury> injuries = [];

            int page = 1;
            int pageCount;

            do
            {
                ApiResult<TeamInjuriesResponseDto> result = await espnApiClient.GetAsync<TeamInjuriesResponseDto>(EspnEndpoints.TeamInjuries(league, teamId, page), cancellationToken);

                if (!result.Success || result.Value is null)
                {
                    break;
                }

                TeamInjuriesResponseDto response = result.Value;

                pageCount = response.PageCount ?? 1;

                Task<TeamInjury?>[] injuryTasks = response.Items.Select(reference => FetchInjuryAsync(reference, cancellationToken)).ToArray();

                TeamInjury?[] pageInjuries = await Task.WhenAll(injuryTasks);

                foreach (TeamInjury? injury in pageInjuries)
                {
                    if (injury is not null)
                    {
                        injuries.Add(injury);
                    }
                }

                page++;
            } while (page <= pageCount);

            return injuries;
        }

        private async Task<TeamInjury?> FetchInjuryAsync(TeamInjuryReferenceDto reference, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(reference.Ref))
            {
                return null;
            }

            string endpoint = NormalizeEspnRef(reference.Ref);

            ApiResult<TeamInjuryDto> result = await espnApiClient.GetAsync<TeamInjuryDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                return null;
            }

            TeamInjury? injury = TeamInjuriesMapper.Map(result.Value);

            if (injury is null)
            {
                return null;
            }

            InjuryAthlete? athlete = await FetchInjuryAthleteAsync(injury.AthleteRef, cancellationToken);

            return new TeamInjury
            {
                Id = injury.Id,

                AthleteRef = injury.AthleteRef,
                Athlete = athlete,
                Status = injury.Status,
                StatusAbbreviation = injury.StatusAbbreviation,
                Date = injury.Date,
                ShortComment = injury.ShortComment,
                LongComment = injury.LongComment,
                Details = injury.Details
            };
        }

        private async Task<InjuryAthlete?> FetchInjuryAthleteAsync(string athleteRef, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(athleteRef))
            {
                return null;
            }

            string endpoint = NormalizeEspnRef(athleteRef);

            ApiResult<RosterAthleteDto> result = await espnApiClient.GetAsync<RosterAthleteDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                return null;
            }

            return TeamInjuriesMapper.MapAthlete(result.Value);
        }

        private static string NormalizeEspnRef(string reference)
        {
            if (reference.StartsWith("http://sports.core.api.espn.com/", StringComparison.OrdinalIgnoreCase))
            {
                return $"https://{reference["http://".Length..]}";
            }

            return reference;
        }
    }
}