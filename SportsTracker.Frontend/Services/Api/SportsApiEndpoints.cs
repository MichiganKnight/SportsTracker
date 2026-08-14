using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.Services.Api
{
    internal static class SportsApiEndpoints
    {
        public static string League(League league)
        {
            return $"scoreboard/{league}";
        }
        
        public static string Game(League league, string gameId)
        {
            return $"games/{league}/{gameId}";
        }
        
        public static string BoxScore(League league, string gameId)
        {
            return $"games/{league}/{gameId}/boxscore";
        }
        
        public static string PlayByPlay(League league, string gameId)
        {
            return $"games/{league}/{gameId}/playbyplay";
        }
        
        public static string Standings(League league)
        {
            return $"standings/{league}";
        }

        public static string Team(League league, string teamId)
        {
            return $"teams/{league}/{Uri.EscapeDataString(teamId)}";
        }
        
        public static string TeamSchedule(League league, string teamId)
        {
            return $"teams/{league}/{Uri.EscapeDataString(teamId)}/schedule";
        }
        
        public static string TeamRoster(League league, string teamId)
        {
            return $"teams/{league}/{Uri.EscapeDataString(teamId)}/roster";
        }
        
        public static string Health => "health";
    }
}