using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;

namespace SportsTracker.App.Services
{
    public interface IScoreboardService
    {
        Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default);

        Task<CachedScoreboard?> GetScoreboardAsync(League league, DateOnly date, CancellationToken cancellationToken = default);
    }
    
    public sealed class ScoreboardService(IEspnApiClient espnApiClient, ICacheService cache, ILogger<ScoreboardService> logger) : IScoreboardService
    {
        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return await cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
        }

        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, DateOnly date, CancellationToken cancellationToken = default)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (date == today)
            {
                return await GetScoreboardAsync(league, cancellationToken);
            }
            
            string cacheKey = CacheKeys.Scoreboard(league, date);
            
            CachedScoreboard? cached = await cache.GetAsync<CachedScoreboard>(cacheKey);
            
            if (cached is not null)
            {
                return cached;
            }
            
            logger.LogInformation("Fetching {League} Scoreboard for {Date}", league, date);

            string endpoint = EspnEndpoints.Scoreboard(league, date);

            ApiResult<ScoreboardResponseDto> result = await espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Fetch {League} for {Date}: {Message}", league, date, result.Error?.Message);
                
                return null;
            }

            CachedScoreboard scoreboard = ScoreboardMapper.MapScoreboard(result.Value, league, DateTime.UtcNow);
            
            await cache.SetAsync(cacheKey, scoreboard, GetCacheLifetime(date));
            
            logger.LogInformation("Cached {Count} Games for {League} on {Date}", scoreboard.Games.Count, league, date);
            
            return scoreboard;
        }
        
        private static TimeSpan GetCacheLifetime(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (date < today.AddDays(-1))
            {
                return TimeSpan.FromDays(1);
            }

            return date < today ? TimeSpan.FromHours(6) : TimeSpan.FromMinutes(30);
        }
    }
}