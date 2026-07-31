using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Services.Api
{
    public interface ISportsApiClient
    {
        Task<ApiResponse<CachedScoreboard>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default);
    }
}