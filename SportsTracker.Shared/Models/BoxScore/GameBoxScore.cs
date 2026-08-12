using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.BoxScore
{
    public sealed class GameBoxScore
    {
        public string GameId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<TeamBoxScore> Teams { get; init; } = [];
    }
}