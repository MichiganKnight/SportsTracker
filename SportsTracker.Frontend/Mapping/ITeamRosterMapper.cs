using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Mapping
{
    public interface ITeamRosterMapper
    {
        public TeamRosterPageViewModel Map(TeamDetailsViewModel team, TeamRoster roster);
    }
}