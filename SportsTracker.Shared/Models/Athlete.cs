namespace SportsTracker.Shared.Models
{
    public sealed class Athlete
    {
        public string Id { get; init; } = string.Empty;
        
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        
        public Logo? Headshot { get; init; }
        
        public int? Rank { get; init; }
    }
}