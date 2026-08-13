using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.GameDetails;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Services.Api
{
    public interface IGameApiClient
    {
        Task<ApiResponse<GameDetails>?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<GameBoxScore>?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<GamePlayByPlay>?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
}