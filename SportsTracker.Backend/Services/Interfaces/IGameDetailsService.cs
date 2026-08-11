using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IGameDetailsService
    {
        Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
}