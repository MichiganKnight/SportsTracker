using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Cache
{
    public static class CacheKeys
    {
        public static string Scoreboard(League league) => $"scoreboard:{league}";
        public static string Standings(League league) => $"standings:{league}";
        public static string Groups(League league) => $"groups:{league}";
        public static string GameDetails(League league, string gameId) => $"game-details:{league}:{gameId}";
        public static string BoxScore(League league, string gameId) => $"box-score:{league}:{gameId}";
        public static string PlayByPlay(League league, string gameId) => $"play-by-play:{league}:{gameId}";
    }
}