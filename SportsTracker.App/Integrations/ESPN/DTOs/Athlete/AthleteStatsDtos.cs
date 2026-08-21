namespace SportsTracker.App.Integrations.ESPN.DTOs.Athlete
{
    public sealed class AthleteStatsResponseDto
    {
        public List<AthleteStatsFilterDto> Filters { get; init; } = [];

        public Dictionary<string, AthleteStatsTeamDto> Teams { get; init; } = [];
        
        public List<AthleteStatsCategoryDto> Categories { get; init; } = [];
        public List<AthleteStatsGlossaryDto> Glossary { get; init; } = [];
    }

    public sealed class AthleteStatsFilterDto
    {
        public string? DisplayName { get; init; }
        public string? Name { get; init; }
        public string? Value { get; init; }
        
        public List<AthleteStatsFilterOptionDto> Options { get; init; } = [];
    }

    public sealed class AthleteStatsFilterOptionDto
    {
        public string? Value { get; init; }
        public string? DisplayValue { get; init; }
        public string? ShortDisplayName { get; init; }
    }
    
    public sealed class AthleteStatsCategoryDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        
        public List<string> Labels { get; init; } = [];
        public List<string> Names { get; init; } = [];
        public List<string> DisplayNames { get; init; } = [];
        public List<string> Descriptions { get; init; } = [];
        
        public List<AthleteStatsRowDto> Statistics { get; init; } = [];
        
        public List<string> Totals { get; init; } = [];
        public List<string> Averages { get; init; } = [];
        
        public string? SortKey { get; init; }
    }

    public sealed class AthleteStatsRowDto
    {
        public string? TeamId { get; init; }
        
        public string? TeamSlug { get; init; }
        
        public AthleteStatsSeasonDto? Season { get; init; }
        
        public List<string> Stats { get; init; } = [];
        
        public string? Position { get; init; }
    }

    public sealed class AthleteStatsSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayName { get; init; }
    }

    public sealed class AthleteStatsTeamDto
    {
        public string? Id { get; init; }
        
        public string? Slug { get; init; }
        public string? Location { get; init; }
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public List<EspnLogoDto> Logos { get; init; } = [];
    }

    public sealed class AthleteStatsGlossaryDto
    {
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
    }
}