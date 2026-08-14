using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface ITeamRosterService
    {
        Task<TeamRoster?> GetTeamRosterAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
}