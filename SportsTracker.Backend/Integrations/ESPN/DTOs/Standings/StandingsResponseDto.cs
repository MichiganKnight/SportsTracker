namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
{
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