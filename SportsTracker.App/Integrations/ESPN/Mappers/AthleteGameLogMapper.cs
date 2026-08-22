using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class AthleteGameLogMapper
    {
        public static AthleteGameLog Map(AthleteGameLogResponseDto dto)
        {
            IReadOnlyList<AthleteGameLogColumn> columns = MapColumns(dto);

            return new AthleteGameLog
            {
                Columns = columns,

                Seasons = dto.SeasonTypes
                    .Select(seasonType => MapSeason(seasonType, dto.Events, columns.Count))
                    .Where(season => season.Categories.Count > 0)
                    .ToList()
            };
        }

        private static IReadOnlyList<AthleteGameLogColumn> MapColumns(AthleteGameLogResponseDto dto)
        {
            int count = new[]
            {
                dto.Labels.Count,
                dto.Names.Count,
                dto.DisplayNames.Count
            }.Max();
            
            List<AthleteGameLogColumn> columns = [];

            for (int i = 0; i < count; i++)
            {
                string name = i < dto.Names.Count ? dto.Names[i] : string.Empty;
                string label = i < dto.Labels.Count ? dto.Labels[i] : name;
                string displayName = i < dto.DisplayNames.Count ? dto.DisplayNames[i] : label;
                
                columns.Add(new AthleteGameLogColumn
                {
                    Name = name,
                    Label = label,
                    DisplayName = displayName
                });
            }
            
            return columns;
        }

        private static AthleteGameLogSeason MapSeason(AthleteGameLogSeasonTypeDto dto, IReadOnlyDictionary<string, AthleteGameLogEventDto> events, int columnCount)
        {
            return new AthleteGameLogSeason
            {
                DisplayName = dto.DisplayName ?? string.Empty,
                TeamAbbreviation = dto.DisplayName,

                Categories = dto.Categories
                    .Select(category => MapCategory(category, events, columnCount))
                    .Where(category => category.Games.Count > 0)
                    .ToList()
            };
        }

        private static AthleteGameLogCategory MapCategory(AthleteGameLogCategoryDto dto, IReadOnlyDictionary<string, AthleteGameLogEventDto> events, int columnCount)
        {
            List<AthleteGameLogGame> games = [];

            foreach (AthleteGameLogEntryDto entry in dto.Events)
            {
                if (string.IsNullOrWhiteSpace(entry.EventId))
                {
                    continue;
                }

                if (!events.TryGetValue(entry.EventId, out AthleteGameLogEventDto? game))
                {
                    continue;
                }
                
                games.Add(MapGame(game, entry.Stats, columnCount));
            }
            
            return new AthleteGameLogCategory
            {
                DisplayName = dto.DisplayName ?? string.Empty,
                SplitType = dto.SplitType,
                
                Games = games.OrderByDescending(game => game.GameDate).ToList(),
                
                Totals = NormalizeStats(dto.Totals, columnCount)
            };
        }

        private static AthleteGameLogGame MapGame(AthleteGameLogEventDto dto, IReadOnlyList<string> stats, int columnCount)
        {
            return new AthleteGameLogGame
            {
                EventId = dto.Id ?? string.Empty,

                GameDate = dto.GameDate,

                Result = dto.GameResult ?? string.Empty,
                Score = dto.Score ?? string.Empty,
                AtVs = dto.AtVs ?? string.Empty,

                EventNote = dto.EventNote,

                OpponentId = dto.Opponent?.Id ?? string.Empty,
                OpponentName = dto.Opponent?.DisplayName ?? string.Empty,
                OpponentAbbreviation = dto.Opponent?.Abbreviation ?? string.Empty,
                OpponentLogo = dto.Opponent?.Logo,

                Stats = NormalizeStats(stats, columnCount)
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
    }
}