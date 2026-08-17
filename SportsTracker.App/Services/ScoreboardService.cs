using SportsTracker.App.Cache;
using SportsTracker.App.Enums;
using SportsTracker.App.Models;

namespace SportsTracker.App.Services
{
    public interface IScoreboardService
    {
        Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default);
    }
    
    public class ScoreboardService(ICacheService cache) : IScoreboardService
    {
        public async Task<CachedScoreboard?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return await cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
        }
    }
}