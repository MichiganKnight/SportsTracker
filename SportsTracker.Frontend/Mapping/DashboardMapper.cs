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
    }
}