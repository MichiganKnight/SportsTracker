namespace SportsTracker.Shared.Models
{
    public sealed class Athlete
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        public string? Jersey { get; init; }
        public string? Position { get; init; }
        public string? TeamId { get; init; }
    }
}