using SportsTracker.Shared.Models.Sport;

namespace SportsTracker.Shared.Models.GameInfo
{
    public sealed class GameSituation
    {
        public string Headline { get; init; } = string.Empty;
        public string Subheadline { get; init; } = string.Empty;
        public string? Detail { get; init; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
        
        public BaseballSituation? Baseball { get; init; }
    }
}