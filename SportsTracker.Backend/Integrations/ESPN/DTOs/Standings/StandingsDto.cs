namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
{
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
}