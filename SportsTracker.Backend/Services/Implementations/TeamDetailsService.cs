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
    public sealed class TeamDetailsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamDetailsService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamDetailsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamDetails?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamDetailsResponseDto, TeamDetails>(league, $"Team Details for {teamId}", CacheKeys.TeamDetails(league, teamId), EspnEndpoints.TeamDetails(league, teamId), dto => TeamDetailsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cacheOptions.TeamMinutes), logger, cancellationToken);
        }
    }
}