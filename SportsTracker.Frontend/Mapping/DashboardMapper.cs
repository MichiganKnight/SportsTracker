using SportsTracker.Frontend.ViewModels.DashboardInfo;
using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.LeagueInfo;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class DashboardMapper : BaseMapper, IDashboardMapper
    {
        private const int MaxDashboardGames = 3;

        public DashboardViewModel Map(Dictionary<League, IReadOnlyList<Game>> scoreboards)
        {
            List<LeagueSectionViewModel> sections = [];

            foreach (League league in LeagueConfiguration.All.OrderBy(l => LeagueConfiguration.Get(l).DisplayOrder))
            {
                scoreboards.TryGetValue(league, out IReadOnlyList<Game>? games);
                
                sections.Add(MapLeague(league, games));
            }

            return new DashboardViewModel
            {
                Leagues = sections
            };
        }

        public LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games)
        {
            LeagueInfo info = LeagueConfiguration.Get(league);

            games ??= [];

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = SelectDashboardGames(games),

                LiveGames = games.Count(g => g.IsLive),
                TotalGames = games.Count
            };
        }
        
        private IReadOnlyList<GameCardViewModel> SelectDashboardGames(IReadOnlyList<Game> games)
        {
            List<Game> selected = [];
            
            AddGames(selected, games.Where(g => g.IsLive).OrderBy(g => g.StartTime));
            AddGames(selected, games.Where(g => g.IsUpcoming).OrderBy(g => g.StartTime));
            AddGames(selected, games.Where(g => g.IsFinal).OrderByDescending(g => g.StartTime));

            return selected.Take(MaxDashboardGames).Select(MapGame).ToList();
        }

        private static void AddGames(List<Game> selected, IEnumerable<Game> source)
        {
            foreach (Game game in source)
            {
                if (selected.Count >= MaxDashboardGames)
                {
                    break;
                }

                if (selected.Any(g => g.Id == game.Id))
                {
                    continue;
                }
                
                selected.Add(game);
            }
        }
    }
}