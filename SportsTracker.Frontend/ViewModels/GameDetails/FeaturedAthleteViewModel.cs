namespace SportsTracker.Frontend.ViewModels.GameDetails
{
    public sealed class FeaturedAthleteViewModel
    {
        public string Type { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        
        public string? ShortName { get; init; }
        public string? Headshot { get; init; }
        public string? TeamId { get; init; }
    }
}