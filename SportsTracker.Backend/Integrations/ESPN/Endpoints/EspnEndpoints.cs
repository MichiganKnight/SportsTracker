using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;

namespace SportsTracker.Backend.Integrations.ESPN.Endpoints
{
    public static class EspnEndpoints
    {
        public static string Scoreboard(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"apis/site/v2/sports/{info.EspnSport}/{info.EspnLeague}/scoreboard";
        }

        public static string Standings(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"apis/v2/sports/{info.EspnSport}/{info.EspnLeague}/standings";
        }

        public static string Groups(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"apis/site/v2/sports/{info.EspnSport}/{info.EspnLeague}/groups";
        }
    }
}