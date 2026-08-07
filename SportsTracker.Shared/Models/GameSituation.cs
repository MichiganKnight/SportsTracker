namespace SportsTracker.Shared.Models
{
    public sealed class GameSituation
    {
        public string Headline { get; init; } = string.Empty;
        public string Subheadline { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;
        
        public BaseballSituation? Baseball { get; init; }
    }
}