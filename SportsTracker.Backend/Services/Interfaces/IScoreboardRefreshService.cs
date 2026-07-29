using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IScoreboardRefreshService
    {
        Task RefreshAsync(League league, CancellationToken cancellationToken = default);
    }
}