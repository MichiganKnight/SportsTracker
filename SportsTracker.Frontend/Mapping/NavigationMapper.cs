using SportsTracker.Frontend.ViewModels.NavigationInfo;
using SportsTracker.Shared.Metadata;

namespace SportsTracker.Frontend.Mapping
{
    public interface INavigationMapper
    {
        IReadOnlyList<NavigationItemViewModel> Map();
    }
    
    public class NavigationMapper : INavigationMapper
    {
        public IReadOnlyList<NavigationItemViewModel> Map()
        {
            return LeagueConfiguration.All
                .OrderBy(l => LeagueConfiguration.Get(l).DisplayOrder)
                .Select(l =>
                {
                    LeagueInfo info = LeagueConfiguration.Get(l);

                    return new NavigationItemViewModel
                    {
                        League = l,
                        Name = info.DisplayName,
                        Icon = info.Icon
                    };
                })
                .ToList();
        }
    }
}