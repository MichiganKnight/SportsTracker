using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
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
    }

    public sealed class TeamService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamService
    {
        private readonly CacheOptions _cache = cacheOptions.Value;
        
        public Task<TeamSchedule?> GetScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamScheduleResponseDto, TeamSchedule>(league, $"Schedule for {teamId}", CacheKeys.TeamSchedule(league, teamId), EspnEndpoints.TeamSchedule(league, teamId), dto => TeamScheduleMapper.Map(dto, league, teamId),
                _ => TimeSpan.FromMinutes(_cache.TeamScheduleMinutes), logger, cancellationToken);
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
    }
}