using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;

namespace SportsTracker.Backend.Integrations.ESPN.Endpoints
{
    public static class EspnEndpoints
    {
        public static string Scoreboard(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"{info.EspnSport}/{info.EspnLeague}/scoreboard";
        }

        public static string Teams(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"{info.EspnSport}/{info.EspnLeague}/teams";
        }

        public static string Standings(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"{info.EspnSport}/{info.EspnLeague}/standings";
        }
    }
}