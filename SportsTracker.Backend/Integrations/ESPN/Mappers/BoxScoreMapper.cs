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
            return table.Type is "batting" or "pitching";
        }

        private static PlayerStatTable MapTable(BoxScoreStatTableDto dto)
        {
            IReadOnlyList<BoxScoreColumn> columns = MapColumns(dto);

            return new PlayerStatTable
            {
                Type = dto.Type ?? string.Empty,

                DisplayName = FormatTableName(dto.Type),

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

                Stats = NormalizeStats(dto.Stats, columnCount)
            };
        }

        private static IReadOnlyList<string> NormalizeStats(IReadOnlyList<string>? stats, int columnCount)
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

        private static string FormatTableName(string? type)
        {
            return type?.ToLowerInvariant() switch
            {
                "batting" => "Batting",
                "pitching" => "Pitching",
                _ => type ?? string.Empty
            };
        }
    }
}