using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Cache
{
    public static class CacheKeys
    {
        public static string Scoreboard(League league) => $"scoreboard:{league}";
        public static string Standings(League league) => $"standings:{league}";
        public static string Groups(League league) => $"groups:{league}";
        public static string GameDetails(League league, string gameId) => $"game-details:{league}:{gameId}";
        public static string GameSummary(League league, string gameId) => $"game-summary:{league}:{gameId}";
        public static string TeamDetails(League league, string teamId) => $"team-details:{league}:{teamId}";
        public static string TeamSchedule(League league, string teamId) => $"team-schedule:{league}:{teamId}";
        public static string TeamRoster(League league, string teamId) => $"team-roster:{league}:{teamId}";
    }
}