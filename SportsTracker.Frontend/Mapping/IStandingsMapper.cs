using SportsTracker.Frontend.ViewModels.Standings;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Mapping
{
    public interface IStandingsMapper
    {
        StandingsViewModel Map(LeagueStandings standings, StandingsView view, DateTime? lastUpdatedUtc = null);
    }
}