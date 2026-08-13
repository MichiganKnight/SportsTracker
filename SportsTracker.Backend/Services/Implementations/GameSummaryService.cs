using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class GameSummaryService : IGameSummaryService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<GameSummaryService> _logger;

        public GameSummaryService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GameSummaryService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        public async Task<GameSummaryResponseDto?> GetGameSummaryAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.GameSummary(league, gameId);
            
            GameSummaryResponseDto? cached = await _cache.GetAsync<GameSummaryResponseDto>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }

            string endpoint = EspnEndpoints.GameSummary(league, gameId);
            
            _logger.LogInformation("Fetching {League} Game Summary for {GameId}", league, gameId);

            ApiResult<GameSummaryResponseDto> result = await _espnApiClient.GetAsync<GameSummaryResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                _logger.LogWarning("Unable to Fetch Game Summary for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);
                
                return null;
            }

            await _cache.SetAsync(cacheKey, result.Value, TimeSpan.FromSeconds(_cacheOptions.GameSummaryLiveSeconds));
            
            return result.Value;
        }
    }
}