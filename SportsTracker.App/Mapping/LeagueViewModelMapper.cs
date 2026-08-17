using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.Golf;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface ILeagueViewModelMapper
    {
        LeaguePageViewModel Map(CachedScoreboard scoreboard);
    }
    
    public sealed class LeagueViewModelMapper(IGameCardViewModelMapper gameCardViewModelMapper, IGolfEventCardViewModelMapper golfEventCardViewModelMapper) : ILeagueViewModelMapper
    {
        public LeaguePageViewModel Map(CachedScoreboard scoreboard)
        {
            if (scoreboard.League == League.PGA)
            {
                return new LeaguePageViewModel
                {
                    League = scoreboard.League,
                    LeagueName = LeagueConfiguration.Get(scoreboard.League).DisplayName,
                    
                    Icon = LeagueConfiguration.Get(scoreboard.League).Icon,
                    
                    LastUpdatedUtc = scoreboard.LastUpdatedUtc,

                    GolfEvents = MapGolfEvents(scoreboard.Games)
                };
            }

            return new LeaguePageViewModel
            {
                League = scoreboard.League,
                LeagueName = LeagueConfiguration.Get(scoreboard.League).DisplayName,
                Icon = LeagueConfiguration.Get(scoreboard.League).Icon,
                LastUpdatedUtc = scoreboard.LastUpdatedUtc,
                
                Live = CreateSection("Live Games", "bi bi-broadcast-pin", GetLiveGames(scoreboard.Games)),
                Upcoming = CreateSection("Upcoming Games", "bi bi-clock", GetUpcomingGames(scoreboard.Games)),
                Final = CreateSection("Final Games", "bi bi-flag", GetFinalGames(scoreboard.Games))
            };
        }

        private IReadOnlyList<GolfEventCardViewModel> MapGolfEvents(IReadOnlyList<Game> games)
        {
            return games
                .Where(game => game.Golf is not null)
                .OrderByDescending(game => game.IsLive)
                .ThenBy(game => game.IsUpcoming ? game.StartTime : DateTime.MaxValue)
                .ThenByDescending(game => game.StartTime)
                .Select(golfEventCardViewModelMapper.Map)
                .ToList();
        }

        private GameSectionViewModel CreateSection(string title, string icon, IEnumerable<Game> games)
        {
            return new GameSectionViewModel
            {
                Title = title,
                Icon = icon,
                Games = games.Select(gameCardViewModelMapper.Map).ToList()
            };
        }

        private static IEnumerable<Game> GetLiveGames(IEnumerable<Game> games)
        {
            return games.Where(g => g.IsLive);
        }

        private static IEnumerable<Game> GetUpcomingGames(IEnumerable<Game> games)
        {
            return games
                .Where(g => g.IsUpcoming)
                .OrderBy(g => g.StartTime);
        }
        
        private static IEnumerable<Game> GetFinalGames(IEnumerable<Game> games)
        {
            return games
                .Where(g => g.IsFinal)
                .OrderByDescending(g => g.StartTime);
        }
    }
}