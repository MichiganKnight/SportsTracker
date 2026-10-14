using System.Text.Json;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Models.Sport;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class SituationMapper
    {
        private static readonly JsonSerializerOptions SituationJsonOptions = new(JsonSerializerDefaults.Web);
        
        public static GameSituation? Map(CompetitionDto competition, League league)
        {
            return league switch
            {
                League.NFL or League.CFB => MapFootball(competition),
                League.NBA or League.CBB => MapTimedSport(competition.Status, league),
                League.NHL => MapHockey(competition),
                League.MLB => MapBaseball(competition),
                League.PGA => null,
                
                _ => null
            };
        }
        
        private static GameSituation MapTimedSport(StatusDto status, League league)
        {
            bool isLive = status.Type.State.Equals("in", StringComparison.OrdinalIgnoreCase);
            
            string headline = league switch
            {
                League.NBA or League.CBB => FormatBasketballPeriod(status.Period),
                League.NHL => FormatHockeyPeriod(status),

                _ => status.Type.ShortDetail
            };
            
            return new GameSituation
            {
                Headline = headline,
                Subheadline = isLive ? status.DisplayClock : string.Empty,
                Detail = status.Type.ShortDetail
            };
        }

        private static GameSituation MapHockey(CompetitionDto competition)
        {
            StatusDto status = competition.Status;

            bool isLive = string.Equals(status.Type.State, "in", StringComparison.OrdinalIgnoreCase);
            
            HockeySituationDto? situation = DeserializeSituation<HockeySituationDto>(competition.Situation);
            HockeyLastPlayDto? lastPlay = situation?.LastPlay;
            
            string? homeTeamId = competition.Competitors.FirstOrDefault(x => string.Equals(x.HomeAway, "home", StringComparison.OrdinalIgnoreCase))?.Team?.Id;
            string? awayTeamId = competition.Competitors.FirstOrDefault(x => string.Equals(x.HomeAway, "away", StringComparison.OrdinalIgnoreCase))?.Team?.Id;

            return new GameSituation
            {
                Headline = FormatHockeyPeriod(status),
                Subheadline = isLive ? status.DisplayClock : string.Empty,
                Detail = status.Type.ShortDetail,

                Hockey = isLive ? new HockeySituation
                {
                    Period = status.Period,
                    Clock = status.DisplayClock,
                    
                    HomeTeamId = homeTeamId,
                    AwayTeamId = awayTeamId,
                    
                    LastPlay = lastPlay?.Text,
                    LastPlayType = lastPlay?.Type?.Abbreviation,
                    LastPlayTeamId = lastPlay?.Team?.Id,
                    LastPlayPlayer = lastPlay?.AthletesInvolved?.FirstOrDefault()?.DisplayName
                } : null
            };
        }

        private static GameSituation MapBaseball(CompetitionDto competition)
        {
            StatusDto status = competition.Status;
            BaseballSituationDto? situation = DeserializeSituation<BaseballSituationDto>(competition.Situation);

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

        private static GameSituation MapFootball(CompetitionDto competition)
        {
            StatusDto status = competition.Status;
            FootballSituation? situation = DeserializeSituation<FootballSituation>(competition.Situation);

            return new GameSituation
            {
                Headline = FormatFootballPeriod(status.Period),
                Subheadline = status.DisplayClock,
                Detail = null,
                Badge = situation?.PossessionText,
                
                Football = situation is null ? null : new FootballSituation
                {
                    Down = situation.Down,
                    Distance = situation.Distance,
                    
                    YardLine = situation.YardLine,
                    YardsToEndzone = situation.YardsToEndzone,
                    
                    DownDistanceText = situation.DownDistanceText,
                    ShortDownDistanceText = situation.ShortDownDistanceText,
                    
                    PossessionTeamId = situation.PossessionTeamId,
                    PossessionText = situation.PossessionText,
                    
                    IsRedZone = situation.IsRedZone == true
                }
            };
        }

        private static T? DeserializeSituation<T>(JsonElement? element)
        {
            if (element is null || element.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                return default;
            }

            return element.Value.Deserialize<T>(SituationJsonOptions);
        }

        private static string FormatFootballPeriod(int? period)
        {
            return period switch
            {
                1 => "1st Quarter",
                2 => "2nd Quarter",
                3 => "3rd Quarter",
                4 => "4th Quarter",
                
                > 4 => $"Overtime {period - 4}",
                
                _ => string.Empty
            };
        }

        private static string FormatBasketballPeriod(int? period)
        {
            return period switch
            {
                1 => "1st Quarter",
                2 => "2nd Quarter",
                3 => "3rd Quarter",
                4 => "4th Quarter",

                > 4 => $"Overtime {period - 4}",

                _ => string.Empty
            };
        }

        private static string FormatHockeyPeriod(StatusDto status)
        {
            if (status.Type.Completed)
            {
                return status.Type.ShortDetail;
            }

            if (status.Type.State.Equals("pre", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (status.Type.ShortDetail.Contains("Shootout", StringComparison.OrdinalIgnoreCase) || status.Type.ShortDetail.Contains("SO", StringComparison.OrdinalIgnoreCase))
            {
                return "Shootout";
            }
            
            return status.Period switch
            {
                1 => "1st Period",
                2 => "2nd Period",
                3 => "3rd Period",
                4 => "Overtime",
                
                > 4 => $"Overtime {status.Period - 3}",
                
                _ => status.Type.ShortDetail
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