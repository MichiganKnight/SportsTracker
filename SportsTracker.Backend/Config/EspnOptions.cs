namespace SportsTracker.Backend.Config
{
    public sealed class EspnOptions
    {
        public const string SectionName = "Espn";
        
        public string BaseUrl { get; init; } = string.Empty;
    }
}