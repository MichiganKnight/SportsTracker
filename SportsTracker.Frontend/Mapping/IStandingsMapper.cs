using SportsTracker.Frontend.ViewModels.Standings;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Mapping
{
    public interface IStandingsMapper
    {
        StandingsViewModel Map(LeagueStandings standings, DateTime? lastUpdatedUtc = null);
    }
}