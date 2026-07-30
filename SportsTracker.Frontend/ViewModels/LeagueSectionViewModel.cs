using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels
{
    public sealed class LeagueSectionViewModel
    {
        public League League { get; init; }
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public bool HasGames => Games.Count > 0;
    }
}