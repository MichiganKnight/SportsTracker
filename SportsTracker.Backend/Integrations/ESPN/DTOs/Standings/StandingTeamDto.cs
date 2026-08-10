namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
{
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
}