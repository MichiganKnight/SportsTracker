using SportsTracker.App.Models.Sport;

namespace SportsTracker.App.Models.GameInfo
{
    public sealed class GameSituation
    {
        public string Headline { get; init; } = string.Empty;
        public string Subheadline { get; init; } = string.Empty;
        public string? Detail { get; init; }
        public string Badge { get; set; } = string.Empty;
        
        public BaseballSituation? Baseball { get; init; }
        public FootballSituation? Football { get; init; }
    }
}