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
        DashboardViewModel Map(
            Dictionary<League, IReadOnlyList<Game>> scoreboards);

        LeagueSectionViewModel MapDashboardLeague(League league, IReadOnlyList<Game>? games);

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
                
                sections.Add(MapDashboardLeague(league, games));
            }

            return new DashboardViewModel
            {
                Leagues = sections
            };
        }

        public LeagueSectionViewModel MapDashboardLeague(League league, IReadOnlyList<Game>? games)
        {
            return MapLeagueInternal(league, games, MaxDashboardGames);
        }

        public LeagueSectionViewModel MapLeague(League league, IReadOnlyList<Game>? games)
        {
            return MapLeagueInternal(league, games, null);
        }

        private LeagueSectionViewModel MapLeagueInternal(League league, IReadOnlyList<Game>? games, int? maxEvents)
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

                    GolfEvents = SelectGolfEvents(games, maxEvents),

                    LiveEvents = games.Count(game => game.IsLive),
                    TotalEvents = games.Count
                };
            }

            return new LeagueSectionViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,

                Games = SelectGames(games, maxEvents),

                LiveEvents = games.Count(g => g.IsLive),
                TotalEvents = games.Count
            };
        }

        private IReadOnlyList<GolfEventCardViewModel> SelectGolfEvents(IReadOnlyList<Game> games, int? maxEvents)
        {
            List<Game> selected = [];

            AddGames(selected, games.Where(game => game.Golf is not null && game.IsLive).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.Golf is not null && game.IsUpcoming).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.Golf is not null && game.IsFinal).OrderByDescending(game => game.StartTime));

            IEnumerable<Game> result = selected;

            if (maxEvents.HasValue)
            {
                result = result.Take(maxEvents.Value);
            }

            return result.Select(golfEventCardViewModelMapper.Map).ToList();
        }
        
        private IReadOnlyList<GameCardViewModel> SelectGames(IReadOnlyList<Game> games, int? maxEvents)
        {
            List<Game> selected = [];

            AddGames(selected, games.Where(game => game.IsLive).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.IsUpcoming).OrderBy(game => game.StartTime));
            AddGames(selected, games.Where(game => game.IsFinal).OrderByDescending(game => game.StartTime));
            
            AddGames(selected, games.Where(game => !game.IsLive && !game.IsUpcoming && !game.IsFinal).OrderBy(game => game.StartTime));

            IEnumerable<Game> result = selected;

            if (maxEvents.HasValue)
            {
                result = result.Take(maxEvents.Value);
            }

            return result.Select(gameCardViewModelMapper.Map).ToList();
        }

        private static void AddGames(List<Game> selected, IEnumerable<Game> games)
        {
            foreach (Game game in games)
            {
                if (selected.Any(existing => existing.Id == game.Id))
                {
                    continue;
                }
                
                selected.Add(game);
            }
        }
    }
}