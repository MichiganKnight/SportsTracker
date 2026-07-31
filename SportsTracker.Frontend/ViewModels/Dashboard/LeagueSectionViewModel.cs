namespace SportsTracker.Frontend.ViewModels.Dashboard
{
    public sealed class LeagueSectionViewModel
    {
        public SportsTracker.Shared.Enums.League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public int GameCount => Games.Count;
        public bool HasGames => Games.Count > 0;
        
        public DateTime LastUpdatedUtc { get; init; }
    }
}