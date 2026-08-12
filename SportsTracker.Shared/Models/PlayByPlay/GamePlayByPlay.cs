using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.PlayByPlay
{
    public sealed class GamePlayByPlay
    {
        public string GameId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<GamePlay> Plays { get; init; } = [];
    }
}