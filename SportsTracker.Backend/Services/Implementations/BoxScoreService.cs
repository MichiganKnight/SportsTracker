using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class BoxScoreService : IBoxScoreService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<BoxScoreService> _logger;

        public BoxScoreService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<BoxScoreService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        public async Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.BoxScore(league, gameId);
            
            GameBoxScore? cached = await _cache.GetAsync<GameBoxScore>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            string endpoint = EspnEndpoints.GameSummary(league, gameId);
            
            _logger.LogInformation("Fetching {League} Box Score for {GameId}", league, gameId);

            ApiResult<GameSummaryResponseDto> result = await _espnApiClient.GetAsync<GameSummaryResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value?.Boxscore is null)
            {
                _logger.LogWarning("Unable to Fetch Box Score for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);
                
                return null;
            }
            
            GameBoxScore? boxScore = BoxScoreMapper.Map(result.Value.Boxscore, gameId, league);

            if (boxScore is null)
            {
                _logger.LogWarning("Unable to Map Box Score for {League} {GameId}", league, gameId);
                
                return null;
            }

            TimeSpan cachedLifetime = GetCacheLifetime(league, gameId);
            
            await _cache.SetAsync(cacheKey, boxScore, cachedLifetime);
            
            return boxScore;
        }

        private TimeSpan GetCacheLifetime(League league, string gameId)
        {
            return TimeSpan.FromSeconds(_cacheOptions.BoxScoreLiveSeconds);
        }
    }
}