namespace SportsTracker.App.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamStatsResponseDto
    {
        public TeamStatsSplitsDto? Splits { get; init; }
    }

    public sealed class TeamStatsSplitsDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<TeamStatCategoryDto> Categories  { get; init; } = [];
    }

    public sealed class TeamStatCategoryDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        public string? Summary { get; init; }
        
        public List<TeamStatisticDto> Stats { get; init; } = [];
    }
    
    public sealed class TeamStatisticDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Description { get; init; }
        public string? Abbreviation { get; init; }
        
        public double? Value { get; init; }
        public string? DisplayValue { get; init; }
        
        public int? Rank { get; init; }
        
        public string? RankDisplayValue { get; init; }
    }
}