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
    public class ScoreboardService : IScoreboardService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<ScoreboardService> _logger;
        
        public ScoreboardService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<ScoreboardService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }
        
        public async Task<IReadOnlyList<Game>> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Scoreboard(league);
            
            _logger.LogInformation("Loading {league} Scoreboard", league);

            return await _cache.GetOrCreateAsync(cacheKey, TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes), async () =>
            {
                _logger.LogInformation("Cache Miss for {league}. Fetching ESPN...", league);

                string endpoint = EspnEndpoints.Scoreboard(league);

                ApiResult<ScoreboardResponseDto> result = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogWarning("Unable to Retrieve Scoreboard for {league}. Status: {Status}. Error: {Error}", league, result.StatusCode, result.Error?.Message);

                    return [];
                }

                IReadOnlyList<Game> games = ScoreboardMapper.ToGames(result.Value!, league).ToList();

                _logger.LogInformation("Retrieved {GameCount} Games for {league}", games.Count, league);

                return games;
            }) ?? [];
        }
    }
}