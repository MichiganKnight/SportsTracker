using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.ViewModels.NavigationInfo;

namespace SportsTracker.App.Mapping
{
    public interface INavigationViewModelMapper
    {
        IReadOnlyList<NavigationItemViewModel> Map(IReadOnlyDictionary<League, CachedScoreboard> scoreboards);
    }
    
    public class NavigationViewModelMapper : INavigationViewModelMapper
    {
        public IReadOnlyList<NavigationItemViewModel> Map(IReadOnlyDictionary<League, CachedScoreboard> scoreboards)
        {
            return LeagueConfiguration.All
                .OrderBy(league => LeagueConfiguration.Get(league).DisplayOrder)
                .Select(league =>
                {
                    LeagueInfo info = LeagueConfiguration.Get(league);
                    
                    scoreboards.TryGetValue(league, out CachedScoreboard? scoreboard);

                    return new NavigationItemViewModel
                    {
                        League = league,
                        Name = info.DisplayName,
                        
                        Icon = info.Icon,
                        
                        Logo = scoreboard?.LeagueLogo,
                        DarkLogo = scoreboard?.LeagueDarkLogo
                    };
                })
                .ToList();
        }
    }
}