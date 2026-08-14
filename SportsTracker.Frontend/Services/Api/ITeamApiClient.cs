using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Services.Api
{
    public interface ITeamApiClient
    {
        Task<ApiResponse<TeamDetails>?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<TeamSchedule>?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
}