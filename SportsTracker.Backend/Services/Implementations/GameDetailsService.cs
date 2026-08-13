using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class GameDetailsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GameDetailsService> logger) : IGameDetailsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public async Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.GameDetails(league, gameId);
            
            GameDetails? cached = await cache.GetAsync<GameDetails>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            string endpoint = EspnEndpoints.GameDetails(league, gameId);
            
            logger.LogInformation("Fetching {League} Game Details for {GameId}", league, gameId);

            ApiResult<GameDetailsResponseDto> result = await espnApiClient.GetAsync<GameDetailsResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Fetch Game Details for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);

                return null;
            }
            
            GameDetails? gameDetails = GameDetailsMapper.Map(result.Value, league);

            if (gameDetails is null)
            {
                return null;
            }

            TimeSpan cachedLifetime = gameDetails.IsLive ? TimeSpan.FromSeconds(_cacheOptions.GameDetailsLiveSeconds) : TimeSpan.FromMinutes(_cacheOptions.GameDetailsFinalMinutes);
            
            await cache.SetAsync(cacheKey, gameDetails, cachedLifetime);
            
            return gameDetails;
        }
    }
}