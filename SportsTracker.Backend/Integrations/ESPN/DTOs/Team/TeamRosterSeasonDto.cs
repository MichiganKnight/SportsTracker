namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamRosterSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayName { get; init; }
        
        public int? Type { get; init; }
        
        public string? Name { get; init; }
    }
}