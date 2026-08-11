namespace SportsTracker.Shared.Models.GameDetails
{
    public sealed class FeaturedAthlete
    {
        public string Type { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;

        public Athlete Athlete { get; init; } = null!;
        
        public string? TeamId { get; init; }
    }
}