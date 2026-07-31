using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class DashboardMapper : BaseMapper, IDashboardMapper
    {
        private const int MaxDashboardGames = 3;

        public DashboardViewModel Map(IReadOnlyDictionary<League, CachedScoreboard> scoreboards)
        {
            List<LeagueSectionViewModel> sections = LeagueConfiguration.All
                .OrderBy(l => LeagueConfiguration.Get(l).DisplayOrder)
                .Select(league =>
                {
                    scoreboards.TryGetValue(league, out CachedScoreboard? scoreboard);

                    return BuildLeagueSection(
                        league,
                        scoreboard ?? CreateEmptyScoreboard(league));
                })
                .ToList();

            return new DashboardViewModel
            {
                Leagues = sections,
                LastUpdatedUtc = sections.Count > 0 ? sections.Max(s => s.LastUpdatedUtc) : DateTime.MinValue
            };
        }

        private LeagueSectionViewModel BuildLeagueSection(League league, CachedScoreboard scoreboard)
        {
            LeagueInfo info = LeagueConfiguration.Get(league);

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = SelectDashboardGames(scoreboard.Games),

                TotalGames = scoreboard.Games.Count,

                LastUpdatedUtc = scoreboard.LastUpdatedUtc
            };
        }

        private IReadOnlyList<GameCardViewModel> SelectDashboardGames(IReadOnlyList<Game> games)
        {
            List<Game> selected = [];
            
            selected.AddRange(games.Where(g => g.IsLive)
                    .OrderBy(g => g.StartTime)
                    .Take(MaxDashboardGames));
            
            if (selected.Count < MaxDashboardGames)
            {
                selected.AddRange(games.Where(g => g.IsUpcoming)
                        .OrderBy(g => g.StartTime)
                        .Take(MaxDashboardGames - selected.Count));
            }
            
            if (selected.Count < MaxDashboardGames)
            {
                selected.AddRange(games.Where(g => g.IsFinal)
                        .OrderByDescending(g => g.StartTime)
                        .Take(MaxDashboardGames - selected.Count));
            }

            return selected.Select(MapGame).ToList();
        }

        private static CachedScoreboard CreateEmptyScoreboard(League league)
        {
            return new CachedScoreboard
            {
                League = league,
                Games = [],
                LastUpdatedUtc = DateTime.MinValue
            };
        }
    }
}