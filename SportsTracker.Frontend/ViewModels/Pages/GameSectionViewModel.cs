using SportsTracker.Frontend.ViewModels.Dashboard;

namespace SportsTracker.Frontend.ViewModels.Pages
{
    public sealed class GameSectionViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public bool HasGames => Games.Count > 0;
    }
}