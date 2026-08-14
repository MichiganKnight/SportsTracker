using SportsTracker.Frontend.ViewModels.NavigationInfo;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public interface INavigationMapper
    {
        IReadOnlyList<NavigationItemViewModel> Map(IReadOnlyDictionary<League, CachedScoreboard> scoreboards);
    }
    
    public class NavigationMapper : INavigationMapper
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