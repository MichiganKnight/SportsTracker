using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IScoreboardRefreshService
    {
        Task<TimeSpan?> RefreshAsync(League league, CancellationToken cancellationToken = default);
    }
}