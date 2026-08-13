using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Implementations
{
    public class ScoreboardService(ICacheService cache) : IScoreboardService
    {
        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return await cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
        }
    }
}