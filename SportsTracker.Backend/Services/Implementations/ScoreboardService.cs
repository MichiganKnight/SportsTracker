using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Implementations
{
    public class ScoreboardService : IScoreboardService
    {
        private readonly ICacheService _cache;
        
        public ScoreboardService(ICacheService cache)
        {
            _cache = cache;
        }
        
        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return await _cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
        }
    }
}