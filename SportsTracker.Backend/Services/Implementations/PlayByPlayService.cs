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
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class PlayByPlayService : IPlayByPlayService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<PlayByPlayService> _logger;

        public PlayByPlayService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<PlayByPlayService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        public async Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.PlayByPlay(league, gameId);
            
            GamePlayByPlay? cached = await _cache.GetAsync<GamePlayByPlay>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }

            string endpoint = EspnEndpoints.GameSummary(league, gameId);
            
            _logger.LogInformation("Fetching {League} Play-by-Play for {GameId}", league, gameId);

            ApiResult<GameSummaryResponseDto> result = await _espnApiClient.GetAsync<GameSummaryResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                _logger.LogWarning("Unable to Fetch Play-by-Play for {League} {GameId}: {Message}", league, gameId, result.Error?.Message);
                
                return null;
            }
            
            GamePlayByPlay? playByPlay = PlayByPlayMapper.Map(result.Value, gameId, league);

            if (playByPlay is null)
            {
                _logger.LogWarning("Unable to Map Play-by-Play for {League} {GameId}", league, gameId);
                
                return null;
            }

            await _cache.SetAsync(cacheKey, playByPlay, TimeSpan.FromSeconds(_cacheOptions.PlayByPlayLiveSeconds));
            
            return playByPlay;
        }
    }
}