namespace SportsTracker.App.Config
{
    public sealed class SportsApiOptions
    {
        public const string SectionName = "Espn";
        
        public string BaseUrl { get; init; } = string.Empty;
    }
}