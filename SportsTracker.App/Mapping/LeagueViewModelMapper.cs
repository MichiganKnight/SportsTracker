using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface ILeagueViewModelMapper
    {
        LeaguePageViewModel Map(CachedScoreboard? scoreboard);
    }
    
    public sealed class LeagueViewModelMapper(IGameCardViewModelMapper gameCardViewModelMapper) : ILeagueViewModelMapper
    {
        public LeaguePageViewModel Map(CachedScoreboard scoreboard)
        {
            League league = scoreboard.League;
            LeagueInfo info = LeagueConfiguration.Get(league);

            return new LeaguePageViewModel
            {
                League = league,
                LeagueName = info.DisplayName,
                Icon = info.Icon,
                LastUpdatedUtc = scoreboard.LastUpdatedUtc,
                
                Live = CreateSection("Live Games", "bi bi-broadcast-pin", GetLiveGames(scoreboard.Games)),
                Upcoming = CreateSection("Upcoming Games", "bi bi-clock", GetUpcomingGames(scoreboard.Games)),
                Final = CreateSection("Final Games", "bi bi-flag", GetFinalGames(scoreboard.Games))
            };
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