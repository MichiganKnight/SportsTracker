using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Cache
{
    public static class CacheKeys
    {
        public static string Scoreboard(League league) => $"scoreboard:{league}";
    }
}