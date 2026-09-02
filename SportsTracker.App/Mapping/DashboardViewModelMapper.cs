using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.ViewModels.DashboardInfo;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface IDashboardViewModelMapper
    {
        DashboardViewModel Map(Dictionary<League, IReadOnlyList<Game>> scoreboards);

        LeagueSectionViewModel MapDashboardLeague(League league, IReadOnlyList<Game>? games);

        LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games);
    }
    
    public sealed class DashboardViewModelMapper(IGameCardViewModelMapper gameCardViewModelMapper, IGolfEventCardViewModelMapper golfEventCardViewModelMapper) : IDashboardViewModelMapper
    {
        private const int MaxDashboardGames = 3;

        public DashboardViewModel Map(Dictionary<League, IReadOnlyList<Game>> scoreboards)
        {
            List<LeagueSectionViewModel> sections = [];

            foreach (League league in LeagueConfiguration.All.OrderBy(league => LeagueConfiguration.Get(league).DisplayOrder))
            {
                scoreboards.TryGetValue(league, out IReadOnlyList<Game>? games);
                
                sections.Add(MapDashboardLeague(league, games));
            }

            return new DashboardViewModel
            {
                Leagues = sections
            };
        }

        public LeagueSectionViewModel MapDashboardLeague(League league, IReadOnlyList<Game>? games)
        {
            games ??= [];

            LeagueInfo info = LeagueConfiguration.Get(league);

            if (league == League.PGA)
            {
                return new LeagueSectionViewModel
                {
                    League = league,
                    LeagueName = info.DisplayName,
                    Icon = info.Icon,

                    GolfEvents = SelectDashboardGames(league, games).Select(golfEventCardViewModelMapper.Map).ToList(),

                    LiveEvents = games.Count(game => game.IsLive),
                    
                    TotalEvents = games.Count
                };
            }

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = SelectDashboardGames(league, games).Select(gameCardViewModelMapper.Map).ToList(),

                LiveEvents = games.Count(game => game.IsLive),
                
                TotalEvents = games.Count
            };
        }

        public LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games)
        {
            games ??= [];

            LeagueInfo info = LeagueConfiguration.Get(league);

            if (league == League.PGA)
            {
                return new LeagueSectionViewModel
                {
                    League = league,
                    LeagueName = info.DisplayName,
                    Icon = info.Icon,

                    GolfEvents = games.Where(game => game.Golf is not null).Select(golfEventCardViewModelMapper.Map).ToList(),

                    LiveEvents = games.Count(game => game.IsLive),

                    TotalEvents = games.Count
                };
            }

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = games.Select(gameCardViewModelMapper.Map).ToList(),

                LiveEvents = games.Count(game => game.IsLive),
                
                TotalEvents = games.Count
            };
        }

        private static IReadOnlyList<Game> SelectDashboardGames(League league, IReadOnlyList<Game> games)
        {
            IEnumerable<Game> availableGames = league == League.PGA ? games.Where(game => game.Golf is not null) : games;

            return availableGames
                .OrderBy(GetStatusOrder)
                .ThenBy(game => game.IsFinal ? DateTime.MaxValue : game.StartTime)
                .ThenByDescending(game => game.IsFinal ? game.StartTime : DateTime.MinValue)
                .Take(MaxDashboardGames)
                .ToList();
        }

        private static int GetStatusOrder(Game game)
        {
            if (game.IsLive)
            {
                return 0;
            }

            if (game.IsUpcoming)
            {
                return 1;
            }

            if (game.IsFinal)
            {
                return 2;
            }
            
            return 3;
        }
    }
}