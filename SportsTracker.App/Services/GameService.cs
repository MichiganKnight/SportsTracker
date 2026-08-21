using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.BoxScore;
using SportsTracker.App.Models.GameDetails;
using SportsTracker.App.Models.PlayByPlay;

namespace SportsTracker.App.Services
{
    public interface IGameService
    {
        // Game Details
        Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        // Game Content
        Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default);
        Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        // Game Summary
        Task<GameSummaryResponseDto?> GetGameSummaryAsync(League league, string gameId, CancellationToken cancellationToken = default);
        Task InvalidateSummaryAsync(League league, string gameId);
    }

    public sealed class GameService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GameService> logger) : EspnCachedServiceBase(espnApiClient, cache), IGameService
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;
        
        public Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<GameDetailsResponseDto, GameDetails>(league, $"Game Details for {gameId}", CacheKeys.GameDetails(league, gameId), EspnEndpoints.GameDetails(league, gameId), dto => GameDetailsMapper.Map(dto, league),
                details => details.IsLive ? TimeSpan.FromSeconds(_cacheOptions.GameDetailsLiveSeconds) : details.IsFinal ? TimeSpan.FromMinutes(_cacheOptions.GameDetailsFinalMinutes) : TimeSpan.FromMinutes(_cacheOptions.GameDetailsScheduledMinutes), logger, cancellationToken);
        }

        public async Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await GetGameSummaryAsync(league, gameId, cancellationToken);
            
            return summary?.Boxscore is null ? null : BoxScoreMapper.Map(summary.Boxscore, gameId, league); 
        }

        public async Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await GetGameSummaryAsync(league, gameId, cancellationToken);

            return summary is null ? null : PlayByPlayMapper.Map(summary, gameId, league);
        }

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
                
                logger.LogInformation("Fetching {League} Game Summary for {GameId}", league, gameId);

                ApiResult<GameSummaryResponseDto> result = await espnApiClient.GetAsync<GameSummaryResponseDto>(EspnEndpoints.GameSummary(league, gameId), cancellationToken);

                if (!result.Success || result.Value is null)
                {
                    logger.LogWarning("Unable to Fetch Game Summary for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);
                    
                    return null;
                }
                
                await cache.SetAsync(cacheKey, result.Value, GetSummaryLifetime(result.Value));
                
                return result.Value;
            }
            finally
            {
                gameLock.Release();
            }
        }

        public Task InvalidateSummaryAsync(League league, string gameId)
        {
            return cache.RemoveAsync(CacheKeys.GameSummary(league, gameId));
        }

        private TimeSpan GetSummaryLifetime(GameSummaryResponseDto summary)
        {
            return summary.Meta?.GameState?.Trim().ToLowerInvariant() switch
            {
                "post" => TimeSpan.FromMinutes(_cacheOptions.GameSummaryFinalMinutes),
                "pre" => TimeSpan.FromMinutes(_cacheOptions.GameSummaryScheduledMinutes),
                "in" => TimeSpan.FromSeconds(_cacheOptions.GameSummaryLiveSeconds),

                _ => TimeSpan.FromSeconds(_cacheOptions.GameSummaryLiveSeconds)
            };
        }
    }
}