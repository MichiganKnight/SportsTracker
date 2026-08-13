using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Mapping
{
    public interface ITeamDetailsMapper
    {
        TeamDetailsViewModel Map(TeamDetails team);
    }
}