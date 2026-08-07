using SportsTracker.Backend.Integrations.ESPN.DTOs.Baseball;
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
                Headline = $"Q{status.Period}",
                Subheadline = status.DisplayClock,
                Detail = status.Type.ShortDetail
            };
        }

        private static GameSituation MapBaseball(CompetitionDto competition)
        {
            StatusDto status = competition.Status;
            SituationDto? situation = competition.BaseballSituation;

            if (situation is null)
            {
                return new GameSituation
                {
                    Headline = status.Type.ShortDetail
                };
            }

            string countAndOuts = FormatBaseballCountAndOuts(situation);
            string baseState = FormatBaseballBaseState(situation);
            string lastPlay = situation.LastPlay?.Text ?? string.Empty;
            
            return new GameSituation
            {
                Headline = status.Type.ShortDetail,
                Subheadline = countAndOuts,
                Detail = lastPlay,
                
                Baseball = new BaseballSituation
                {
                    Balls = situation.Balls,
                    Strikes = situation.Strikes,
                    Outs = situation.Outs,
                    
                    RunnerOnFirst = situation.OnFirst,
                    RunnerOnSecond = situation.OnSecond,
                    RunnerOnThird = situation.OnThird,
                }
            };
        }

        private static string FormatBaseballCountAndOuts(SituationDto situation)
        {
            return situation switch
            {
                { Balls: int balls, Strikes: int strikes, Outs: int outs } =>
                    $"{balls}-{strikes}, {FormatOuts(outs)}",

                { Balls: int balls, Strikes: int strikes } =>
                    $"{balls}-{strikes}",

                { Outs: int outs } =>
                    FormatOuts(outs),

                _ => string.Empty
            };
        }
        
        private static string FormatOuts(int outs)
        {
            return $"{outs} Out{(outs == 1 ? "" : "s")}";
        }

        private static string FormatBaseballBaseState(SituationDto situation)
        {
            List<string> occupiedBases = [];

            if (situation.OnFirst)
            {
                occupiedBases.Add("1st");
            }

            if (situation.OnSecond)
            {
                occupiedBases.Add("2nd");
            }
            
            if (situation.OnThird)
            {
                occupiedBases.Add("3rd");
            }
            
            return occupiedBases.Count == 0 ? string.Empty : $"On {string.Join(", ", occupiedBases)}";
        }
    }
}