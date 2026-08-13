using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Services.Api
{
    public interface IStandingsApiClient
    {
        Task<ApiResponse<LeagueStandings>?> GetStandingsAsync(League league, CancellationToken cancellationToken = default);
    }
}