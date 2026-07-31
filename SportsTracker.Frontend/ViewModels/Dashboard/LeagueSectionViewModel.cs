using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.Dashboard
{
    public sealed class LeagueSectionViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public int GameCount => Games.Count;
        public bool HasGames => Games.Count > 0;
        
        public int TotalGames { get; init; }

        public int DisplayedGames => Games.Count;

        public bool HasMoreGames => TotalGames > DisplayedGames;
        
        public DateTime LastUpdatedUtc { get; init; }
    }
}