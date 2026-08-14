using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.PlayByPlay
{
    public sealed class PlayByPlayViewModel
    {
        public string GameId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<PlayFilterViewModel> Filters { get; init; } = [];
        public IReadOnlyList<GamePlayViewModel> Plays { get; init; } = [];
    }
}