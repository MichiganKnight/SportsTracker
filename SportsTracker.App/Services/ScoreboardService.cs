using SportsTracker.App.Cache;
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
        Task<CachedScoreboard?> GetScoreboardAsync(League league, int season, int seasonType, CancellationToken cancellationToken = default);
        Task<CachedScoreboard?> GetScoreboardAsync(League league, int season, int seasonType, int week, CancellationToken cancellationToken = default);
    }
    
    public sealed class ScoreboardService(IEspnApiClient espnApiClient, ICacheService cache, ILogger<ScoreboardService> logger) : EspnCachedServiceBase(espnApiClient, cache), IScoreboardService
    {
        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Scoreboard(league);

            return await GetOrFetchAsync<ScoreboardResponseDto, CachedScoreboard>(league, "Scoreboard", cacheKey, EspnEndpoints.Scoreboard(league), dto => ScoreboardMapper.MapScoreboard(dto, league, DateTime.UtcNow), GetScoreboardCacheLifetime,
                logger, cancellationToken);
        }

        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, DateOnly date, CancellationToken cancellationToken = default)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (date == today)
            {
                return await GetScoreboardAsync(league, cancellationToken);
            }
            
            string cacheKey = CacheKeys.Scoreboard(league, date);

            return await GetOrFetchAsync<ScoreboardResponseDto, CachedScoreboard>(league, $"Scoreboard for {date}", cacheKey, EspnEndpoints.Scoreboard(league, date), dto => ScoreboardMapper.MapScoreboard(dto, league, DateTime.UtcNow),
                _ => GetCacheLifetime(date), logger, cancellationToken);
        }

        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, int season, int seasonType, int week, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Scoreboard(league, season, seasonType, week);

            return await GetOrFetchAsync<ScoreboardResponseDto, CachedScoreboard>(league, "Scoreboard", cacheKey, EspnEndpoints.Scoreboard(league, season, seasonType, week),
                dto => ScoreboardMapper.MapScoreboard(dto, league, DateTime.UtcNow), GetScoreboardCacheLifetime , logger, cancellationToken);
        }

        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, int season, int seasonType, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Scoreboard(league, season, seasonType);

            return await GetOrFetchAsync<ScoreboardResponseDto, CachedScoreboard>(league, "Scoreboard", cacheKey, EspnEndpoints.Scoreboard(league, season, seasonType),
                dto => ScoreboardMapper.MapScoreboard(dto, league, DateTime.UtcNow), GetScoreboardCacheLifetime , logger, cancellationToken);
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

        private static TimeSpan GetScoreboardCacheLifetime(CachedScoreboard scoreboard)
        {
            if (scoreboard.Games.Count == 0)
            {
                return TimeSpan.FromMinutes(30);
            }
            
            bool allFinal = scoreboard.Games.All(game => game.Status == GameStatus.Final);

            if (allFinal)
            {
                return TimeSpan.FromDays(1);
            }

            bool anyLive = scoreboard.Games.Any(game => game.Status is GameStatus.InProgress or GameStatus.Halftime);
            
            return anyLive ? TimeSpan.FromMinutes(1) : TimeSpan.FromMinutes(30);
        }
    }
}