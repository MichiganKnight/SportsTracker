using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;

namespace SportsTracker.App.Services
{
    public interface IGameSummaryService
    {
        Task<GameSummaryResponseDto?> GetGameSummaryAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task InvalidateAsync(League league, string gameId);
    }
    
    public sealed class GameSummaryService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GameSummaryService> logger) : IGameSummaryService
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public async Task<GameSummaryResponseDto?> GetGameSummaryAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.GameSummary(league, gameId);
            
            GameSummaryResponseDto? cached = await cache.GetAsync<GameSummaryResponseDto>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            SemaphoreSlim gameLock = Locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
            
            await gameLock.WaitAsync(cancellationToken);

            try
            {
                cached = await cache.GetAsync<GameSummaryResponseDto>(cacheKey);

                if (cached is not null)
                {
                    return cached;
                }

                string endpoint = EspnEndpoints.GameSummary(league, gameId);

                logger.LogInformation("Fetching Fresh {League} Game Summary for {GameId}", league, gameId);

                ApiResult<GameSummaryResponseDto> result = await espnApiClient.GetAsync<GameSummaryResponseDto>(endpoint, cancellationToken);

                if (!result.Success || result.Value is null)
                {
                    logger.LogWarning("Unable to Fetch Game Summary for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);

                    return null;
                }

                TimeSpan lifeTime = GetCacheLifetime(result.Value);

                await cache.SetAsync(cacheKey, result.Value, lifeTime);
                
                return result.Value;
            }
            finally
            {
                gameLock.Release();
            }
        }

        public async Task InvalidateAsync(League league, string gameId)
        {
            string cacheKey = CacheKeys.GameSummary(league, gameId);
            
            await cache.RemoveAsync(cacheKey);
        }

        private TimeSpan GetCacheLifetime(GameSummaryResponseDto summary)
        {
            string? gameState = summary.Meta?.GameState?.Trim().ToLowerInvariant();

            return gameState switch
            {
                "post" => TimeSpan.FromMinutes(_cacheOptions.GameSummaryFinalMinutes),
                "pre" => TimeSpan.FromMinutes(_cacheOptions.GameSummaryScheduledMinutes),
                "in" => TimeSpan.FromSeconds(_cacheOptions.GameSummaryLiveSeconds),

                _ => TimeSpan.FromSeconds(_cacheOptions.GameSummaryLiveSeconds)
            };
        }
    }
}