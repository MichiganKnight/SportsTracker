using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Standings;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Services.Implementations
{
    public class StandingsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<StandingsService> logger) : IStandingsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public async Task<LeagueStandings?> GetStandingsAsync(League league, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Standings(league);

            LeagueStandings? cached = await cache.GetAsync<LeagueStandings>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            logger.LogInformation("Fetching {League} Standings", league);

            string endpoint = EspnEndpoints.Standings(league);

            ApiResult<StandingsResponseDto> result = await espnApiClient.GetAsync<StandingsResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Fetch {League} Standings: {Message}", league, result.Error?.Message);
                
                return null;
            }
            
            LeagueStandings standings = StandingsMapper.Map(result.Value, league);

            await cache.SetAsync(cacheKey, standings, TimeSpan.FromMinutes(_cacheOptions.StandingsMinutes));
            
            return standings;
        }
    }
}