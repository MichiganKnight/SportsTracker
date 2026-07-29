using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Implementations
{
    public class ScoreboardService : IScoreboardService
    {
        private readonly ICacheService _cache;
        private readonly IScoreboardRefreshService _refreshService;
        
        public ScoreboardService(ICacheService cache, IScoreboardRefreshService refreshService)
        {
            _cache = cache;
            _refreshService = refreshService;
        }
        
        public async Task<IReadOnlyList<Game>> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return await _cache.GetAsync<IReadOnlyList<Game>>(CacheKeys.Scoreboard(league)) ?? [];
        }
    }
}