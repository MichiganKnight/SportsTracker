using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IPlayByPlayService
    {
        Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
}