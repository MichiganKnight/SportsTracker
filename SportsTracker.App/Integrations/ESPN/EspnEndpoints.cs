using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;

namespace SportsTracker.App.Integrations.ESPN
{
    public static class EspnEndpoints
    {
        public static string Scoreboard(League league)
        {
            LeagueInfo info = GetLeagueInfo(league);

            return Site(info, "scoreboard");
        }

        public static string Scoreboard(League league, DateOnly date)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, $"scoreboard?dates={date:yyyyMMdd}");
        }

        public static string Standings(League league)
        {
            LeagueInfo info = GetLeagueInfo(league);

            return $"apis/v2/sports/{info.EspnSport}/{info.EspnLeague}/standings";
        }

        public static string LeagueLeadersByAthlete(League league, string category, string statistic, int season, int seasonType = 2, int limit = 50, bool descending = true)
        {
            LeagueInfo info = GetLeagueInfo(league);

            string direction = descending ? "desc" : "asc";
            string sort = $"{category}.{statistic}:{direction}";
            
            return $"apis/common/v3/sports/{info.EspnSport}/{info.EspnLeague}/statistics/byathlete?category={category}&season={season}&seasonType={seasonType}&sort={Uri.EscapeDataString(sort)}&limit={limit}";
        }

        public static string Rankings(League league)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, "rankings");
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
        
        public static string TeamDetails(League league, string teamId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, $"teams/{Uri.EscapeDataString(teamId)}");
        }

        public static string TeamSchedule(League league, string teamId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, $"teams/{Uri.EscapeDataString(teamId)}/schedule");
        }

        public static string TeamRoster(League league, string teamId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Site(info, $"teams/{Uri.EscapeDataString(teamId)}/roster");
        }

        public static string AthleteDetails(League league, string athleteId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Athlete(info, athleteId);
        } 

        public static string AthleteOverview(League league, string athleteId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Athlete(info, athleteId, "overview");
        }

        public static string AthleteStats(League league, string athleteId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Athlete(info, athleteId, "stats");
        }

        public static string AthleteGameLog(League league, string athleteId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Athlete(info, athleteId, "gamelog");
        }

        public static string AthleteSplits(League league, string athleteId)
        {
            LeagueInfo info = GetLeagueInfo(league);
            
            return Athlete(info, athleteId, "splits");
        }

        private static LeagueInfo GetLeagueInfo(League league)
        {
            return !LeagueConfiguration.Leagues.TryGetValue(league, out LeagueInfo? info) ? throw new NotSupportedException($"ESPN Endpoints Not Configured for {league}") : info;
        }

        public static string Search(string query, int limit = 25)
        {
            return $"apis/search/v2?query={Uri.EscapeDataString(query)}&limit={limit}";
        }

        private static string Site(LeagueInfo info, string path)
        {
            return $"apis/site/v2/sports/{info.EspnSport}/{info.EspnLeague}/{path}";
        }
        
        private static string Athlete(LeagueInfo info, string athleteId, string? path = null)
        {
            string endpoint =  $"apis/common/v3/sports/{info.EspnSport}/{info.EspnLeague}/athletes/{Uri.EscapeDataString(athleteId)}";

            if (!string.IsNullOrWhiteSpace(path))
            {
                endpoint += $"/{path}";
            }
            
            return endpoint;
        }
    }
}