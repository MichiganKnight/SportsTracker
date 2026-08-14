using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Base;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Services.Implementations
{
    public class TeamScheduleService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamScheduleService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamScheduleService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamSchedule?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamScheduleResponseDto, TeamSchedule>(league, $"Team Schedule for {teamId}", CacheKeys.TeamSchedule(league, teamId), EspnEndpoints.TeamSchedule(league, teamId), dto => TeamScheduleMapper.Map(dto, league, teamId), _ => TimeSpan.FromMinutes(_cacheOptions.TeamScheduleMinutes), logger, cancellationToken);
        }
    }
}