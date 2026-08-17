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
    public interface ITeamScheduleService
    {
        Task<TeamSchedule?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
    
    public class TeamScheduleService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamScheduleService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamScheduleService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamSchedule?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamScheduleResponseDto, TeamSchedule>(league, $"Team Schedule for {teamId}", CacheKeys.TeamSchedule(league, teamId), EspnEndpoints.TeamSchedule(league, teamId), dto => TeamScheduleMapper.Map(dto, league, teamId), _ => TimeSpan.FromMinutes(_cacheOptions.TeamScheduleMinutes), logger, cancellationToken);
        }
    }
}