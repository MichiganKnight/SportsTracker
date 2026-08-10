using SportsTracker.Shared.Models.Groups;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IStandingsGroupingService
    {
        LeagueStandings AddDivisionGroups(LeagueStandings standings, IReadOnlyList<SportsGroup> groups);
    }
}