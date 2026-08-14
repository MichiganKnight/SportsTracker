using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Base;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class TeamRosterService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamRosterService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamRosterService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamRoster?> GetTeamRosterAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamRosterResponseDto, TeamRoster>(league, $"Team Roster for {teamId}", CacheKeys.TeamRoster(league, teamId), EspnEndpoints.TeamRoster(league, teamId), dto => TeamRosterMapper.Map(dto, league, teamId),
                _ => TimeSpan.FromMinutes(_cacheOptions.TeamRosterMinutes), logger, cancellationToken);
        }
    }
}