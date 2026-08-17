using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models.PlayByPlay;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class PlayByPlayMapper
    {
        public static GamePlayByPlay Map(GameSummaryResponseDto response, string gameId, League league)
        {
            List<PlayDto> sourcePlays = GetPlays(response);
            List<GamePlay> plays = sourcePlays.Where(ShouldIncludePlay).Select(play => MapPlay(play, league)).ToList();

            return new GamePlayByPlay
            {
                GameId = gameId,
                League = league,
                Plays = plays
            };
        }

        private static List<PlayDto> GetPlays(GameSummaryResponseDto response)
        {
            if (response.Plays is { Count: > 0 })
            {
                return response.Plays;
            }

            if (response.Drives is null)
            {
                return [];
            }
            
            List<PlayDto> plays = response.Drives.Previous?.SelectMany(drive => drive.Plays ?? []).ToList() ?? [];

            if (response.Drives.Current?.Plays is { Count: > 0 })
            {
                plays.AddRange(response.Drives.Current.Plays);
            }
            
            return plays;
        }

        private static GamePlay MapPlay(PlayDto dto, League league)
        {
            (string? context, string? text) = ExtractPlayContext(dto.Text, league);
            
            return new GamePlay
            {
                Id = dto.Id ?? string.Empty,

                Type = dto.Type?.Type ?? dto.Type?.Text ?? dto.Type?.Abbreviation ?? dto.Type?.AlternativeText,

                Text = text,

                ShortText = dto.Type?.AlternativeText,

                Period = FormatPeriod(dto.Period, league),

                Clock = dto?.Clock?.DisplayValue,

                SequenceNumber = ParseSequenceNumber(dto.SequenceNumber),
                
                AwayScore = dto.AwayScore,
                HomeScore = dto.HomeScore,

                ScoringPlay = dto.ScoringPlay == true,

                TeamId = league switch
                {
                    League.NFL or League.CFB => dto.Start?.Team?.Id ?? dto.Team?.Id,
                    
                    _ => dto.Team?.Id
                },
                
                Category = MapCategory(dto, league),
                
                GroupId = league == League.MLB ? dto.AtBatId : null,
                
                Situation = league switch
                {
                    League.NFL or League.CFB => dto.Start?.DownDistanceText,
                    
                    _ => null
                },
                
                Context = context
            };
        }

        private static string MapCategory(PlayDto dto, League league)
        {
            if (dto.ScoringPlay == true)
            {
                return "scoring";
            }

            if (league == League.MLB)
            {
                return dto.SummaryType?.ToUpperInvariant() switch
                {
                    "P" => "pitch",
                    "N" => "atbat",
                    "S" => "atbat scoring",

                    _ => "other"
                };
            }
            
            if (league is League.NFL or League.CFB)
            {
                string? type = dto.Type?.Text ?? dto.Type?.Abbreviation;

                return type?.ToLowerInvariant() switch
                {
                    "rush" => "rush",

                    "pass reception" => "pass",
                    "pass incompletion" => "pass",
                    "pass complete" => "pass",
                    "pass" => "pass",

                    "interception" => "turnover",
                    "fumble recovery" => "turnover",
                    "fumble" => "turnover",

                    _ => "other"
                };
            }

            return "other";
        }
        
        private static (string? context, string? text) ExtractPlayContext(string? text, League league)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return (null, text);
            }

            if (league is not (League.NFL or League.CFB))
            {
                return (null, text);
            }

            if (!text.StartsWith("("))
            {
                return (null, text);
            }
            
            int closingParenthesis = text.IndexOf(')');

            if (closingParenthesis <= 1)
            {
                return (null, text);
            }
            
            string context = text[1..closingParenthesis].Trim();
            string playText = text[(closingParenthesis + 1)..].TrimStart();
            
            return (context, playText);
        }

        private static string? FormatPeriod(PlayPeriodDto? period, League league)
        {
            if (period is null)
            {
                return null;
            }

            if (league == League.NFL || league == League.CFB)
            {
                return period.Number switch
                {
                    1 => "1st Quarter",
                    2 => "2nd Quarter",
                    3 => "3rd Quarter",
                    4 => "4th Quarter",

                    > 4 => $"Overtime {period.Number - 4}",

                    _ => null
                };
            }

            if (!string.IsNullOrWhiteSpace(period.DisplayValue))
            {
                return period.Type switch
                {
                    "Top" => $"Top {period.Number}",
                    "Bottom" => $"Bottom {period.Number}",

                    _ => period.DisplayValue
                };
            }

            if (period.Number.HasValue)
            {
                return period.Type is null ? period.Number.Value.ToString() : $"{period.Type} {period.Number.Value}";
            }

            return period.Type;
        }

        private static int? ParseSequenceNumber(string? sequenceNumber)
        {
            return int.TryParse(sequenceNumber, out int value) ? value : null;
        }

        private static bool ShouldIncludePlay(PlayDto play)
        {
            if (string.IsNullOrWhiteSpace(play.Text))
            {
                return false;
            }

            return play.Type?.Type switch
            {
                "start-inning" => false,
                "end-inning" => false,
                
                "start-batterpitcher" => false,
                "end-batterpitcher" => false,
                
                _ => true
            };
        }
    }
}