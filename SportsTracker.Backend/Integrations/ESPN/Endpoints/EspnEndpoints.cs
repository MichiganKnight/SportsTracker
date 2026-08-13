using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;

namespace SportsTracker.Backend.Integrations.ESPN.Endpoints
{
    public static class EspnEndpoints
    {
        public static string Scoreboard(League league)
        {
            LeagueInfo info = GetLeagueInfo(league);

            return Site(info, "scoreboard");
        }

        public static string Standings(League league)
        {
            LeagueInfo info = LeagueConfiguration.Leagues[league];

            return $"apis/v2/sports/{info.EspnSport}/{info.EspnLeague}/standings";
        }

        public static string Groups(League league)
        {
            LeagueInfo info = GetLeagueInfo(league);

            return Site(info, "groups");
        }

        public static string GameDetails(League league, string gameId)
        {
            LeagueInfo info = GetLeagueInfo(league);

            return Site(info, $"scoreboard/{gameId}");
        }

        public static string GameSummary(League league, string gameId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, $"summary?event={Uri.EscapeDataString(gameId)}");
        }

        private static LeagueInfo GetLeagueInfo(League league)
        {
            return !LeagueConfiguration.Leagues.TryGetValue(league, out LeagueInfo? info) ? throw new NotSupportedException($"ESPN Endpoints Not Configured for {league}") : info;
        }

        private static string Site(LeagueInfo info, string path)
        {
            return $"apis/site/v2/sports/{info.EspnSport}/{info.EspnLeague}/{path}";
        }
    }
}