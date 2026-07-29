using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class ScoreboardRefreshService : IScoreboardRefreshService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private ILogger<ScoreboardRefreshService> _logger;

        public ScoreboardRefreshService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<ScoreboardRefreshService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        public async Task RefreshAsync(League league, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Refreshing {League} Scoreboard...", league);

            string endpoint = EspnEndpoints.Scoreboard(league);
            
            ApiResult<ScoreboardResponseDto> result = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (!result.Success)
            {
                _logger.LogWarning("Unable to Refresh {League}: {Message}", league, result.Error?.Message);
                
                return;
            }
            
            IReadOnlyList<Game> games = ScoreboardMapper.ToGames(result.Value!, league).ToList();

            await _cache.SetAsync(CacheKeys.Scoreboard(league), games, TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes));
            
            _logger.LogInformation("Cached {Count} Games for {League}", games.Count, league);
        }
    }
}