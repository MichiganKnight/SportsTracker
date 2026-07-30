namespace SportsTracker.Frontend.Config
{
    public sealed class SportsApiOptions
    {
        public const string SectionName = "SportsApi";
        
        public string BaseUrl { get; init; } = string.Empty;
    }
}