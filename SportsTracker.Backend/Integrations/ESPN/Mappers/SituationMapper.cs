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
            return league switch
            {
                League.NFL or League.CFB or League.NBA or League.CBB or League.NHL => MapTimedSport(competition.Status),
                League.MLB => MapBaseball(competition),
                League.PGA => null,
                _ => null
            };
        }
        
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
            
            return new GameSituation
            {
                Headline = status.Type.ShortDetail,
                Subheadline = FormatBaseballCountAndOuts(situation),
                Detail = situation.LastPlay?.Text,
                Badge = FormatBaseballBaseState(situation),
                
                Baseball = new BaseballSituation
                {
                    Balls = situation.Balls,
                    Strikes = situation.Strikes,
                    Outs = situation.Outs,
                    
                    OnFirst = situation.OnFirst,
                    OnSecond = situation.OnSecond,
                    OnThird = situation.OnThird,
                    
                    Batter = MapNullableAthlete(situation.Batter),
                    Pitcher = MapNullableAthlete(situation.Pitcher),

                    DueUp = MapDueUp(situation),

                    LastPlay = situation.LastPlay?.Text
                }
            };
        }

        private static List<Athlete> MapDueUp(BaseballSituationDto situation)
        {
            return situation.DueUp?
                .Where(x => x.Athlete is not null)
                .Select(x => MapAthlete(x.Athlete!))
                .ToList() ?? [];
        }

        private static Athlete? MapNullableAthlete(AthleteDto? athlete)
        {
            return athlete is null ? null : MapAthlete(athlete);
        }

        private static Athlete MapAthlete(AthleteDto athlete)
        {
            return new Athlete
            {
                Id = athlete.Id ?? string.Empty,
                Name = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName,
                Jersey = athlete.Jersey,
                Headshot = athlete.Headshot,
                TeamId = athlete.Team?.Id
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