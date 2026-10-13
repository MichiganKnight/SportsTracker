using System.Text.RegularExpressions;
using SportsTracker.App.Enums;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class GameResultFormatter
    {
        public static string? GetLabel(League league, bool completed, int period, string? name, string? shortDetail, string? description, string? detail, int homeScore, int awayScore)
        {
            if (!completed)
            {
                return null;
            }
            
            string text = string.Join(" ", name, shortDetail, description, detail);
            
            if (league == League.NFL && homeScore == awayScore)
            {
                return "FINAL / TIE";
            }

            if (league is League.NFL or League.CFB)
            {
                Match match = Regex.Match(text, @"(?<!\w)(?<count>\d+)(?:st|nd|rd|th)?\s*OT\b", RegexOptions.IgnoreCase);

                if (match.Success && int.TryParse(match.Groups["count"].Value, out int parsedCount) && parsedCount > 0)
                {
                    return parsedCount == 1 ? "FINAL / OT" : $"FINAL / {parsedCount}OT";
                }

                if (period > 4)
                {
                    int count = period - 4;
                    
                    return count == 1 ? "FINAL / OT" : $"FINAL / {count}OT";
                }

                if (Regex.IsMatch(text, @"\b(OT|Overtime)\b", RegexOptions.IgnoreCase))
                {
                    return "FINAL / OT";
                }
            }

            if (league == League.NHL)
            {
                if (Regex.IsMatch(text, @"\b(SO|Shootout)\b", RegexOptions.IgnoreCase) || string.Equals(name, "STATUS_FINAL_SHOOTOUT", StringComparison.OrdinalIgnoreCase))
                {
                    return "FINAL / SO";
                }

                if (period > 3)
                {
                    int count = period - 3;
                    
                    return count == 1 ? "FINAL / OT" : $"FINAL / {count}OT";
                }
            }

            if (league == League.MLB && period > 9)
            {
                return $"FINAL / {period}";
            }
            
            return "FINAL";
        }

        public static GameStatus GetStatus(string? name, string? resultLabel)
        {
            if (resultLabel is not null)
            {
                if (resultLabel == "FINAL / SO")
                {
                    return GameStatus.FinalShootout;
                }

                if (resultLabel.Contains("OT", StringComparison.OrdinalIgnoreCase))
                {
                    return GameStatus.FinalOvertime;
                }
                
                return GameStatus.Final;
            }

            return name switch
            {
                "STATUS_SCHEDULED" => GameStatus.Scheduled,
                "STATUS_IN_PROGRESS" => GameStatus.InProgress,
                "STATUS_HALFTIME" => GameStatus.Halftime,
                "STATUS_FINAL" => GameStatus.Final,
                "STATUS_FINAL_OVERTIME" => GameStatus.FinalOvertime,
                "STATUS_FINAL_SHOOTOUT" => GameStatus.FinalShootout,
                "STATUS_POSTPONED" => GameStatus.Postponed,
                "STATUS_DELAYED" => GameStatus.Delayed,
                "STATUS_CANCELLED" => GameStatus.Cancelled,
                
                _ => GameStatus.Scheduled
            };
        }
    }
}