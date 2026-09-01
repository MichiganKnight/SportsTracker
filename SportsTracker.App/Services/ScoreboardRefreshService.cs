using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Hubs;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;

namespace SportsTracker.App.Services
{
    public interface IScoreboardRefreshService
    {
        Task<TimeSpan?> RefreshAsync(League league, CancellationToken cancellationToken = default);
    }
    
    public sealed class ScoreboardRefreshService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, IHubContext<ScoreboardHub> hub, IGameService gameService, ILogger<ScoreboardRefreshService> logger) : IScoreboardRefreshService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public async Task<TimeSpan?> RefreshAsync(League league, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Refreshing {League} Scoreboard...", league);

            string endpoint = EspnEndpoints.Scoreboard(league);
            
            ApiResult<ScoreboardResponseDto> result = await espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Refresh {League}: {Message}", league, result.Error?.Message);
                
                return null;
            }
            
            CachedScoreboard? previousScoreboard = await cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
            
            DateTime updatedUtc = DateTime.UtcNow;
            
            CachedScoreboard scoreboard = ScoreboardMapper.MapScoreboard(result.Value, league, updatedUtc);
            
            IReadOnlyList<Game> games = scoreboard.Games;
            
            TimeSpan refreshInterval = GetRefreshInterval(games);
            TimeSpan cacheLifetime = GetCacheLifetime(refreshInterval);

            await cache.SetAsync(CacheKeys.Scoreboard(league), scoreboard, cacheLifetime);

            await InvalidateGameSummariesAsync(league, games, previousScoreboard);
            
            await hub.Clients.All.SendAsync("ScoreboardUpdated", new ScoreboardUpdatedMessage
            {
                League = league.ToString(),
                UpdatedUtc = updatedUtc
            }, cancellationToken);
            
            logger.LogInformation("Cached {Count} Games for {League} | Next Refresh in {Interval}", games.Count, league, refreshInterval);
            
            return refreshInterval;
        }

        private async Task InvalidateGameSummariesAsync(League league, IReadOnlyList<Game> games, CachedScoreboard? previousScoreboard)
        {
            foreach (Game game in games)
            {
                Game? previousGame = previousScoreboard?.Games.FirstOrDefault(g => g.Id == game.Id);
                
                bool justFinished = game.IsFinal && previousGame is not null && !previousGame.IsFinal;

                if (!game.IsLive && !justFinished)
                {
                    continue;
                }
                
                await gameService.InvalidateSummaryAsync(league, game.Id);
            }
        }

        private TimeSpan GetRefreshInterval(IReadOnlyList<Game> games)
        {
            if (games.Any(game => game.IsLive))
            {
                return TimeSpan.FromSeconds(_cacheOptions.LiveScoreboardSeconds);
            }

            if (games.Any(game => game.IsUpcoming))
            {
                return TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes);
            }

            if (games.Count > 0 && games.All(game => game.IsFinal))
            {
                return TimeSpan.FromMinutes(_cacheOptions.FinalScoreboardMinutes);
            }

            return TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes);
        }

        private static TimeSpan GetCacheLifetime(TimeSpan refreshInterval)
        {
            return TimeSpan.FromTicks(refreshInterval.Ticks * 2);
        }
    }
}