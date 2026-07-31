using SportsTracker.Frontend.ViewModels.Pages;
using SportsTracker.Frontend.ViewModels.Shared;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public interface ILeagueMapper
    {
        LeaguePageViewModel Map(CachedScoreboard scoreboard);
    }
}