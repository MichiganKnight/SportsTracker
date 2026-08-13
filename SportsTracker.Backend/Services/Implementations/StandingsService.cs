using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Standings;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Base;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class StandingsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<StandingsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IStandingsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<LeagueStandings?> GetStandingsAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<StandingsResponseDto, LeagueStandings>(league, "Standings", CacheKeys.Standings(league), EspnEndpoints.Standings(league), dto => StandingsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cacheOptions.StandingsMinutes), logger, cancellationToken);
        }
    }
}