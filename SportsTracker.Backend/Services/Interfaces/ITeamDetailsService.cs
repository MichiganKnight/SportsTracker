using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface ITeamDetailsService
    {
        Task<TeamDetails?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
}