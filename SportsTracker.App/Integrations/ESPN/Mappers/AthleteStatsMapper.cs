using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class AthleteStatsMapper
    {
        public static AthleteStats Map(AthleteStatsResponseDto dto)
        {
            return new AthleteStats
            {
                Filters = dto.Filters.Select(MapFilter).ToList(),
                
                Categories = dto.Categories
                    .Where(category => category.Statistics.Count > 0)
                    .Select(MapCategory)
                    .ToList(),
                
                Teams = dto.Teams
                    .Values
                    .Where(team => !string.IsNullOrWhiteSpace(team.Id))
                    .GroupBy(team => team.Id!)
                    .ToDictionary(group => group.Key, group => MapTeam(group.First())),
                
                Glossary = dto.Glossary
                    .Where(item => !string.IsNullOrWhiteSpace(item.Abbreviation) && !string.IsNullOrWhiteSpace(item.DisplayName))
                    .GroupBy(item => item.Abbreviation!, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(group => group.Key, group => group.First().DisplayName!, StringComparer.OrdinalIgnoreCase)
            };
        }

        private static AthleteStatsFilter MapFilter(AthleteStatsFilterDto dto)
        {
            return new AthleteStatsFilter
            {
                DisplayName = dto.DisplayName ?? dto.Name ?? string.Empty,

                SelectedValue = dto.Value ?? string.Empty,

                Options = dto.Options
                    .Select(option => new AthleteStatsFilterOption
                    {
                        Value = option.Value ?? string.Empty,
                        DisplayValue = option.DisplayValue ?? option.ShortDisplayName ?? option.Value ?? string.Empty,
                        ShortDisplayName = option.ShortDisplayName ?? option.DisplayValue ?? option.Value ?? string.Empty
                    })
                    .ToList()
            };
        }

        private static AthleteStatsCategory MapCategory(AthleteStatsCategoryDto dto)
        {
            IReadOnlyList<AthleteStatsColumn> columns = MapColumns(dto);

            return new AthleteStatsCategory
            {
                Name = dto.Name ?? string.Empty,

                DisplayName = FormatCategoryName(dto.DisplayName ?? dto.Name),

                Columns = columns,

                Rows = dto.Statistics.Select(row => MapRow(row, columns.Count)).ToList(),

                Totals = NormalizeStats(dto.Totals, columns.Count),
                Averages = NormalizeStats(dto.Averages, columns.Count)
            };
        }

        private static IReadOnlyList<AthleteStatsColumn> MapColumns(AthleteStatsCategoryDto dto)
        {
            int count = new int[]
            {
                dto.Labels.Count,
                dto.Names.Count,
                dto.DisplayNames.Count,
                dto.Descriptions.Count
            }.Max();

            List<AthleteStatsColumn> columns = [];

            for (int i = 0; i < count; i++)
            {
                string name = i < dto.Names.Count ? dto.Names[i] : string.Empty;
                string label = i < dto.Labels.Count ? dto.Labels[i] : name;
                string displayName = i < dto.DisplayNames.Count ? dto.DisplayNames[i] : label;
                
                string? description = i < dto.Descriptions.Count ? dto.Descriptions[i] : null;
                
                columns.Add(new AthleteStatsColumn
                {
                    Name = name,
                    Label = label,
                    DisplayName = displayName,
                    Description = description
                });
            }
            
            return columns;
        }

        private static AthleteStatsRow MapRow(AthleteStatsRowDto dto, int columnCount)
        {
            return new AthleteStatsRow
            {
                TeamId = dto.TeamId ?? string.Empty,

                TeamSlug = dto.TeamSlug ?? string.Empty,

                SeasonYear = dto.Season?.Year,
                Season = dto.Season?.DisplayName ?? dto.Season?.Year?.ToString() ?? string.Empty,

                Position = dto.Position,

                Stats = NormalizeStats(dto.Stats, columnCount)
            };
        }

        private static AthleteStatsTeam MapTeam(AthleteStatsTeamDto dto)
        {
            return new AthleteStatsTeam
            {
                Id = dto.Id ?? string.Empty,

                DisplayName = dto.DisplayName ?? dto.Name ?? string.Empty,
                Abbreviation = dto.Abbreviation ?? string.Empty,

                Logo = SelectLogo(dto.Logos, dark: false),
                DarkLogo = SelectLogo(dto.Logos, dark: true),

                Color = dto.Color,
                AlternateColor = dto.AlternateColor
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

        private static string? SelectLogo(IReadOnlyList<EspnLogoDto>? logos, bool dark)
        {
            if (logos is null || logos.Count == 0)
            {
                return null;
            }
            
            EspnLogoDto? logo = logos
                .FirstOrDefault(item => HasRel(item, "full") && HasRel(item, dark ? "dark" : "default"));
            
            logo ??= logos.FirstOrDefault(item => dark ? HasRel(item, "dark") : !HasRel(item, "dark"));
            
            logo ??= logos.FirstOrDefault();
            
            return logo?.Href;
        }

        private static bool HasRel(EspnLogoDto logo, string rel)
        {
            return logo.Rel?.Contains(rel, StringComparer.OrdinalIgnoreCase) == true;
        }

        private static string FormatCategoryName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Statistics";
            }
            
            return char.ToUpperInvariant(name[0]) + name[1..];
        }
    }
}