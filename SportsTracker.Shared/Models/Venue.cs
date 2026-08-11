namespace SportsTracker.Shared.Models
{
    public class Venue
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        
        public string? City { get; init; }
        public string? State { get; init; }
        
        public bool? IsIndoor { get; init; }
    }
}