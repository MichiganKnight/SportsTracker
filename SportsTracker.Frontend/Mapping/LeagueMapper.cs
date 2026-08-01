using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.LeagueInfo;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class LeagueMapper : BaseMapper, ILeagueMapper
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
                Games = games.Select(MapGame).ToList()
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