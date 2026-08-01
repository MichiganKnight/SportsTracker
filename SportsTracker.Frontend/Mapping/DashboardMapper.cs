using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class DashboardMapper : BaseMapper, IDashboardMapper
    {
        private const int MaxDashboardGames = 3;

        public DashboardViewModel Map(IReadOnlyDictionary<League, IReadOnlyList<Game>?> scoreboards)
        {
            List<LeagueSectionViewModel> sections = [];

            foreach (League league in LeagueConfiguration.All.OrderBy(l => LeagueConfiguration.Get(l).DisplayOrder))
            {
                LeagueInfo info = LeagueConfiguration.Get(league);
                
                scoreboards.TryGetValue(league, out IReadOnlyList<Game>? games);

                sections.Add(new LeagueSectionViewModel
                {
                    League = league,
                    LeagueName = info.DisplayName,
                    Icon = info.Icon,
                    
                    Games = SelectDashboardGames(games ?? []),
                    
                    LiveGames = (games ?? []).Count(g => g.IsLive),
                    TotalGames = (games ?? []).Count
                });
            }

            return new DashboardViewModel
            {
                Leagues = sections
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