using SportsTracker.Backend.Integrations.ESPN.DTOs.Baseball;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;
using SportsTracker.Shared.Models.Sport;

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
            BaseballSituationDto? situation = competition.BaseballSituation;

            if (situation is null)
            {
                return new GameSituation
                {
                    Headline = status.Type.ShortDetail
                };
            }

            string countAndOuts = FormatBaseballCountAndOuts(situation);
            string baseState = FormatBaseballBaseState(situation);
            
            Athlete? batter = situation.DueUp.Count > 0 ? AthleteMapper.Map(situation.DueUp[0].Athlete) : null;
            
            return new GameSituation
            {
                Headline = status.Type.ShortDetail,
                Subheadline = countAndOuts,
                Detail = situation.LastPlay?.Text,
                Badge = baseState,
                
                Baseball = new BaseballSituation
                {
                    Inning = status.Period,
                    InningState = status.Type.ShortDetail,
                    
                    Balls = situation.Balls,
                    Strikes = situation.Strikes,
                    Outs = situation.Outs,
                    
                    RunnerOnFirst = situation.OnFirst,
                    RunnerOnSecond = situation.OnSecond,
                    RunnerOnThird = situation.OnThird,
                    
                    Batter = batter,
                    
                    LastPlay = situation.LastPlay?.Text
                }
            };
        }

        private static string FormatBaseballCountAndOuts(BaseballSituationDto baseballSituation)
        {
            return baseballSituation switch
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

        private static string FormatBaseballBaseState(BaseballSituationDto baseballSituation)
        {
            List<string> occupiedBases = [];

            if (baseballSituation.OnFirst)
            {
                occupiedBases.Add("1st");
            }

            if (baseballSituation.OnSecond)
            {
                occupiedBases.Add("2nd");
            }
            
            if (baseballSituation.OnThird)
            {
                occupiedBases.Add("3rd");
            }
            
            return occupiedBases.Count == 0 ? string.Empty : $"On {string.Join(", ", occupiedBases)}";
        }
    }
}