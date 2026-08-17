using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.ViewModels.DashboardInfo;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.Golf;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface IDashboardViewModelMapper
    {
        DashboardViewModel Map(Dictionary<League, IReadOnlyList<Game>> scoreboards);

        LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games);
    }
    
    public sealed class DashboardViewModelMapper(IGameCardViewModelMapper gameCardViewModelMapper, IGolfEventCardViewModelMapper golfEventCardViewModelMapper) : IDashboardViewModelMapper
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

            if (league == League.PGA)
            {
                return new LeagueSectionViewModel
                {
                    League = league,
                    LeagueName = info.DisplayName,
                    Icon = info.Icon,

                    GolfEvents = SelectDashboardGolfEvents(games),

                    LiveEvents = games.Count(game => game.IsLive),
                    TotalEvents = games.Count
                };
            }

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = SelectDashboardGames(games),

                LiveEvents = games.Count(g => g.IsLive),
                TotalEvents = games.Count
            };
        }

        private IReadOnlyList<GolfEventCardViewModel> SelectDashboardGolfEvents(IReadOnlyList<Game> games)
        {
            List<Game> selected = [];

            AddGames(selected, games.Where(game => game.Golf is not null && game.IsLive).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.Golf is not null && game.IsUpcoming).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.Golf is not null && game.IsFinal).OrderByDescending(game => game.StartTime));

            return selected
                .Take(MaxDashboardGames)
                .Select(golfEventCardViewModelMapper.Map)
                .ToList();
        }
        
        private IReadOnlyList<GameCardViewModel> SelectDashboardGames(IReadOnlyList<Game> games)
        {
            List<Game> selected = [];
            
            AddGames(selected, games.Where(game => game.IsLive).OrderBy(game => game.StartTime)); 
            AddGames(selected, games.Where(game => game.IsUpcoming).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.IsFinal).OrderByDescending(game => game.StartTime));
            
            return selected.Take(MaxDashboardGames).Select(gameCardViewModelMapper.Map).ToList();
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