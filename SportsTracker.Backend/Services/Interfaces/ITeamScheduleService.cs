using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface ITeamScheduleService
    {
        Task<TeamSchedule?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
}