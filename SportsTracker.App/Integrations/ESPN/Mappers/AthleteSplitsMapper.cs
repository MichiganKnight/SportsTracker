using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class AthleteSplitsMapper
    {
        public static AthleteSplits? Map(AthleteSplitsResponseDto? dto)
        {
            if (dto is null)
            {
                return null;
            }
            
            List<AthleteSplitCategory> categories = dto.SplitCategories
                .Where(category => category.Splits.Count > 0)
                .Select(category => MapCategory(dto, category))
                .Where(category => category is not null)
                .Cast<AthleteSplitCategory>()
                .ToList();
            
            return new AthleteSplits
            {
                DisplayName = dto.DisplayName ?? "Splits",
                
                Categories = categories
            };
        }

        private static AthleteSplitCategory? MapCategory(AthleteSplitsResponseDto dto, AthleteSplitCategoryDto category)
        {
            if (category.Splits.Count == 0)
            {
                return null;
            }
            
            AthleteSplitColumnSetDto columnSet = GetColumnSet(dto, category);
            IReadOnlyList<AthleteSplitColumn> columns = BuildColumns(columnSet);
            
            List<AthleteSplitRow> rows = category.Splits
                .Select(split => new AthleteSplitRow
                {
                    DisplayName = split.DisplayName ?? split.Abbreviation ?? string.Empty,
                    Abbreviation = split.Abbreviation,
                    
                    Stats = NormalizeStats(split.Stats, columns.Count)
                })
                .ToList();
            
            return new AthleteSplitCategory
            {
                Name = category.Name ?? string.Empty,
                DisplayName = GetCategoryDisplayName(category),
                
                Columns = columns,
                Rows = rows
            };
        }

        private static AthleteSplitColumnSetDto GetColumnSet(AthleteSplitsResponseDto dto, AthleteSplitCategoryDto category)
        {
            if (!string.IsNullOrWhiteSpace(category.ExtraAthleteSplitsType) && dto.ExtraPlayerPageAthleteSplits is not null &&
                dto.ExtraPlayerPageAthleteSplits.TryGetValue(category.ExtraAthleteSplitsType, out AthleteSplitColumnSetDto? extraColumns) && extraColumns is not null)
            {
                return extraColumns;
            }

            return new AthleteSplitColumnSetDto
            {
                Labels = dto.Labels,
                Names = dto.Names,
                DisplayNames = dto.DisplayNames
            };
        }

        private static IReadOnlyList<AthleteSplitColumn> BuildColumns(AthleteSplitColumnSetDto columnSet)
        {
            int count = new[]
            {
                columnSet.Labels.Count,
                columnSet.Names.Count,
                columnSet.DisplayNames.Count
            }.Max();
            
            List<AthleteSplitColumn> columns = [];

            for (int i = 0; i < count; i++)
            {
                columns.Add(new AthleteSplitColumn
                {
                    Name = GetValue(columnSet.Names, i),
                    Label = GetValue(columnSet.Labels, i),
                    DisplayName = GetValue(columnSet.DisplayNames, i)
                });
            }
            
            return columns;
        }

        private static IReadOnlyList<string> NormalizeStats(IReadOnlyList<string> stats, int columnCount)
        {
            List<string> normalized = stats.Take(columnCount).ToList();

            while (normalized.Count < columnCount)
            {
                normalized.Add("-");
            }
            
            return normalized;
        }
        
        private static string GetCategoryDisplayName(AthleteSplitCategoryDto category)
        {
            string displayName = category.DisplayName ?? category.Name ?? "Splits";

            if (string.Equals(displayName, "split", StringComparison.OrdinalIgnoreCase))
            {
                return "Overall";
            }
            
            return displayName;
        }

        private static string GetValue(IReadOnlyList<string> values, int index)
        {
            return index < values.Count ? values[index] : string.Empty;
        }
    }
}