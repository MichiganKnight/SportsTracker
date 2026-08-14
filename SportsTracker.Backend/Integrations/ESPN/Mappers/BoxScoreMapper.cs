using System.Text;
using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class BoxScoreMapper
    {
        public static GameBoxScore? Map(BoxScoreDto dto, string gameId, League league)
        {
            if (dto?.Players is null || dto.Players.Count == 0)
            {
                return null;
            }

            return new GameBoxScore
            {
                GameId = gameId,
                League = league,

                Teams = dto.Players.Where(team => team.Team is not null).OrderBy(team => team.DisplayOrder ?? int.MaxValue).Select(MapTeam).ToList()
            };
        }

        private static TeamBoxScore MapTeam(BoxScorePlayerTeamDto dto)
        {
            BoxScoreTeamDto team = dto.Team!;

            return new TeamBoxScore
            {
                TeamId = team.Id ?? string.Empty,

                TeamName = team.DisplayName ?? team.Name ?? string.Empty,
                Abbreviation = team.Abbreviation ?? string.Empty,

                Logo = team.Logo,

                Tables = dto.Statistics.Where(ShouldIncludeTable).Select(MapTable).ToList() ?? []
            };
        }

        private static bool ShouldIncludeTable(BoxScoreStatTableDto table)
        {
            string? type = GetTableType(table);
            
            if (string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            if (table.Athletes is null || table.Athletes.Count == 0)
            {
                return false;
            }
            
            return true;
        }

        private static string? GetTableType(BoxScoreStatTableDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.Type) ? dto.Type : dto.Name;
        }

        private static PlayerStatTable MapTable(BoxScoreStatTableDto dto)
        {
            IReadOnlyList<BoxScoreColumn> columns = MapColumns(dto);
            
            string type = GetTableType(dto) ?? string.Empty;

            return new PlayerStatTable
            {
                Type = type,

                DisplayName = GetTableDisplayName(dto),

                Columns = columns,

                Players = dto.Athletes?
                    .Where(athlete => athlete.Athlete is not null)
                    .Select(athlete => MapPlayer(athlete, columns.Count))
                    .ToList() ?? [],

                Totals = NormalizeStats(dto.Totals, columns.Count)
            };
        }

        private static IReadOnlyList<BoxScoreColumn> MapColumns(BoxScoreStatTableDto dto)
        {
            List<string> keys = dto.Keys ?? [];
            List<string> labels = dto.Labels ?? dto.Names ?? [];
            List<string> descriptions = dto.Descriptions ?? [];
            
            int count = Math.Max(keys.Count, labels.Count);
            
            List<BoxScoreColumn> columns = [];

            for (int i = 0; i < count; i++)
            {
                string key = i < keys.Count ? keys[i] : string.Empty;
                string label = i < labels.Count ? labels[i] : key;

                string? description = i < descriptions.Count ? descriptions[i] : null;
                
                columns.Add(new BoxScoreColumn
                {
                    Key = key,
                    Label = label,
                    Description = description
                });
            }
            
            return columns;
        }

        private static PlayerStatRow MapPlayer(BoxScoreAthleteEntryDto dto, int columnCount)
        {
            BoxScoreAthleteDto athlete = dto.Athlete!;

            return new PlayerStatRow
            {
                AthleteId = athlete.Id ?? string.Empty,
                Name = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName ?? athlete.DisplayName ?? athlete.FullName ?? string.Empty,

                Headshot = athlete.Headshot?.Href,

                Position = dto.Position?.Abbreviation ?? dto.Position?.DisplayName,

                Starter = dto.Starter == true,

                BatOrder = dto.BatOrder,
                
                Note = dto.Notes?.FirstOrDefault(note => string.Equals(note.Type, "pitchingDecision", StringComparison.OrdinalIgnoreCase))?.Text, 

                Stats = NormalizeStats(dto.Stats, columnCount)
            };
        }

        private static List<string> NormalizeStats(IReadOnlyList<string>? stats, int columnCount)
        {
            if (columnCount <= 0)
            {
                return [];
            }
            
            List<string> values = stats?.ToList() ?? [];

            while (values.Count < columnCount)
            {
                values.Add(string.Empty);
            }
            
            if (values.Count > columnCount)
            {
                values = values.Take(columnCount).ToList();
            }
            
            return values;
        }

        private static string GetTableDisplayName(BoxScoreStatTableDto dto)
        {
            /*if (!string.IsNullOrWhiteSpace(dto.Text))
            {
                string text = dto.Text;

                int spaceIndex = text.IndexOf(' ');

                if (spaceIndex >= 0 && spaceIndex < text.Length - 1)
                {
                    return text[(spaceIndex + 1)..];
                }
                
                return text;
            }*/
            
            return FormatTableName(GetTableType(dto));
        }

        private static string FormatTableName(string? type)
        {
            return type?.ToLowerInvariant() switch
            {
                "batting" => "Batting",
                "pitching" => "Pitching",
                
                "passing" => "Passing",
                "rushing" => "Rushing",
                "receiving" => "Receiving",
                "fumbles" => "Fumbles",
                "defensive" => "Defense",
                "interceptions" => "Interceptions",
                "kickreturns" => "Kick Returns",
                "puntreturns" => "Punt Returns",
                "kicking" => "Kicking",
                "punting" => "Punting",
                
                _ => FormatPascalCase(type)
            };
        }

        private static string FormatPascalCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            StringBuilder builder = new();

            for (int i = 0; i < value.Length; i++)
            {
                char current = value[i];

                if (i > 0 && char.IsUpper(current) && char.IsLower(value[i - 1]))
                {
                    builder.Append(' ');
                }
                
                builder.Append(current);
            }
            
            string formatted = builder.ToString();
            
            return char.ToUpperInvariant(formatted[0]) + formatted[1..];
        }
    }
}