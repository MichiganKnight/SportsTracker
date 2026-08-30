namespace SportsTracker.App.Integrations.ESPN.DTOs.Athlete
{
    public sealed class AthleteSplitsResponseDto
    {
        public string? DisplayName { get; init; }
        
        public List<string> Labels { get; init; } = [];
        public List<string> Names { get; init; } = [];
        public List<string> DisplayNames { get; init; } = [];
        
        public List<AthleteSplitCategoryDto> SplitCategories { get; init; } = [];
        
        public Dictionary<string, AthleteSplitColumnSetDto>? ExtraPlayerPageAthleteSplits { get; init; }
    }

    public sealed class AthleteSplitCategoryDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ExtraAthleteSplitsType { get; init; }
        
        public List<AthleteSplitDto> Splits { get; init; } = [];
    }

    public sealed class AthleteSplitDto
    {
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<string> Stats { get; init; } = [];
    }
    
    public sealed class AthleteSplitColumnSetDto
    {
        public List<string> Labels { get; init; } = [];
        public List<string> Names { get; init; } = [];
        public List<string> DisplayNames { get; init; } = [];
    }
}