using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class SituationMapper
    {
        public static GameSituation? Map(CompetitionDto competition, League league)
        {
            StatusDto status = competition.Status;
            
            return league switch
            {
                League.NFL or League.CFB => MapFootball(status),
                League.NBA or League.CBB => MapBasketball(status),
                League.MLB => MapBaseball(competition),
                League.NHL => MapHockey(status),
                League.PGA => null,

                _ => null
            };
        }

        private static GameSituation MapFootball(StatusDto status) => MapTimedSport(status);
        private static GameSituation MapBasketball(StatusDto status) => MapTimedSport(status);
        private static GameSituation MapHockey(StatusDto status) => MapTimedSport(status);

        private static GameSituation MapTimedSport(StatusDto status)
        {
            return new GameSituation
            {
                Primary = $"Q{status.Period}",
                Secondary = status.DisplayClock ?? string.Empty,
                Detail = status.Type.ShortDetail ?? string.Empty
            };
        }

        private static GameSituation MapBaseball(CompetitionDto competition)
        {
            StatusDto status = competition.Status;
            SituationDto? situation = competition.Situation;
            
            string secondaryText = string.Empty;
            if (situation != null)
            {
                secondaryText = situation switch
                {
                    { Balls: int b, Strikes: int s, Outs: int o } => $"{b}-{s}, {o} Out{(o == 1 ? "" : "s")}",
                    { Balls: int b, Strikes: int s } => $"{b}-{s}",
                    { Outs: int o } => $"{o} Out{(o == 1 ? "" : "s")}",
                    _ => string.Empty
                };
            }

            return new GameSituation
            {
                Primary = status.Type.ShortDetail ?? string.Empty,
                Secondary = secondaryText
            };
        }
    }
}