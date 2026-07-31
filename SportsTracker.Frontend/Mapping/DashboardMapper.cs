using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Frontend.ViewModels.Shared;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class DashboardMapper : BaseMapper, IDashboardMapper
    {
        public DashboardViewModel Map(IReadOnlyDictionary<League, CachedScoreboard> scoreboards)
        {
            List<LeagueSectionViewModel> sections = LeagueConfiguration.All
                .OrderBy(l => LeagueConfiguration.Get(l).DisplayOrder)
                .Select(league =>
                {
                    scoreboards.TryGetValue(league, out CachedScoreboard? scoreboard);
                    
                    return BuildLeagueSection(league, scoreboard ?? new CachedScoreboard());
                })
                .ToList();
            
            return new DashboardViewModel
            {
                Leagues = sections,
                
                LastUpdatedUtc = sections.Any() ? sections.Max(s => s.LastUpdatedUtc) : DateTime.MinValue
            };
        }

        private static LeagueSectionViewModel BuildLeagueSection(League league, CachedScoreboard scoreboard)
        {
            LeagueInfo info = LeagueConfiguration.Get(league);

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,
                Route = $"/league/{info.Route}",

                Games = scoreboard.Games.Select(MapGame).ToList()
            };
        }
    }
}