using SportsTracker.Frontend.ViewModels.DashboardInfo;
using SportsTracker.Frontend.ViewModels.LeagueInfo;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Frontend.Mapping
{
    public interface IDashboardMapper
    {
        DashboardViewModel Map(Dictionary<League, IReadOnlyList<Game>> scoreboards);

        LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games);
    }
}