namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class StandingStatDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        
        public string? Description { get; init; }
        public string? Abbreviation { get; init; }
        public string? Type { get; init; }
        
        public double? Value { get; init; }
        
        public string? Summary { get; init; }
        public string? DisplayValue { get; init; }
    }
    
    public sealed class StandingTeamDto
    {
        public string? Id { get; init; }
        public string? Uid { get; init; }
        
        public string? Location { get; init; }
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        
        public bool? IsActive { get; init; }
        
        public List<StandingTeamLogoDto>? Logos { get; init; } = [];
    }
    
    public sealed class StandingTeamLogoDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }
    
    public class StandingsDto
    {
        public string? Id { get; init; }
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        
        public int? Season { get; init; }
        public int? SeasonType { get; init; }
        public string? SeasonDisplayName { get; init; }
        
        public List<StandingsEntryDto>? Entries { get; init; } = [];
    }
    
    public sealed class StandingsEntryDto
    {
        public StandingTeamDto? Team { get; init; }
        
        public List<StandingStatDto>? Stats { get; init; } = [];
    }
    
    public sealed class StandingsGroupDto
    {
        public string? Uid { get; init; }
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? ShortName { get; init; }
        
        public StandingsDto? Standings { get; init; }
    }
    
    public sealed class StandingsResponseDto
    {
        public string? Uid { get; init; }
        public string? Id { get; init; }
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? ShortName { get; init; }
        
        public List<StandingsGroupDto>? Children { get; init; } = [];
    }
}