using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.GameDetails;
using SportsTracker.Shared.Models.GameInfo;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Services.Api
{
    public interface ISportsApiClient
    {
        Task<ApiResponse<IReadOnlyList<Game>>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<CachedScoreboard>?> GetLeagueAsync(League league, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<LeagueStandings>?> GetStandingsAsync(League league, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<GameDetails>?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<GameBoxScore>?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
}