using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Hubs;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class ScoreboardRefreshService : IScoreboardRefreshService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly IHubContext<ScoreboardHub> _hub;
        private ILogger<ScoreboardRefreshService> _logger;

        public ScoreboardRefreshService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, IHubContext<ScoreboardHub> hub, ILogger<ScoreboardRefreshService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _hub = hub;
            _logger = logger;
        }

        public async Task<TimeSpan?> RefreshAsync(League league, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Refreshing {League} Scoreboard...", league);

            string endpoint = EspnEndpoints.Scoreboard(league);
            
            ApiResult<ScoreboardResponseDto> result = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                _logger.LogWarning("Unable to Refresh {League}: {Message}", league, result.Error?.Message);
                
                return null;
            }
            
            IReadOnlyList<Game> games = ScoreboardMapper.ToGames(result.Value!, league).ToList();
            
            DateTime updatedUtc = DateTime.UtcNow;
            TimeSpan refreshInterval = GetRefreshInterval(games);
            TimeSpan cacheLifetime = GetCacheLifetime(refreshInterval);

            await _cache.SetAsync(CacheKeys.Scoreboard(league), new CachedScoreboard
            {
                League = league,
                Games = games,
                LastUpdatedUtc = updatedUtc
            }, cacheLifetime);
            
            await _hub.Clients.All.SendAsync("ScoreboardUpdated", new ScoreboardUpdatedMessage
            {
                League = league.ToString(),
                UpdatedUtc = updatedUtc
            }, cancellationToken);
            
            _logger.LogInformation("Cached {Count} Games for {League} | Next Refresh in {Interval}", games.Count, league, refreshInterval);
            
            return refreshInterval;
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