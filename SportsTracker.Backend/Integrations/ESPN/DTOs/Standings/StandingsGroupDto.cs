namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
{
    public sealed class StandingsGroupDto
    {
        public string? Uid { get; init; }
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? ShortName { get; init; }
        
        public StandingsDto? Standings { get; init; }
    }
}