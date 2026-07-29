using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IScoreboardService
    {
        Task<IReadOnlyList<Game>> GetScoreboardAsync(League league, CancellationToken cancellationToken = default);
    }
}