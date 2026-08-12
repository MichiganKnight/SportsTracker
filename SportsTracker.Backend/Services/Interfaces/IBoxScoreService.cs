using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IBoxScoreService
    {
        Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
}