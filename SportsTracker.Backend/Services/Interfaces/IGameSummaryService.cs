using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IGameSummaryService
    {
        Task<GameSummaryResponseDto?> GetGameSummaryAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task InvalidateAsync(League league, string gameId);
    }
}