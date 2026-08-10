namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
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
}