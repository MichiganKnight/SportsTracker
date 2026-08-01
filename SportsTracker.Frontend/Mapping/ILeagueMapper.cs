using SportsTracker.Frontend.ViewModels.LeagueInfo;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public interface ILeagueMapper
    {
        LeaguePageViewModel Map(CachedScoreboard scoreboard);
    }
}