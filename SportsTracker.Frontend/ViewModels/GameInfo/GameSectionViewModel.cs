namespace SportsTracker.Frontend.ViewModels.GameInfo
{
    public sealed class GameSectionViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public bool HasGames => Games.Count > 0;
        
        public int Count => Games.Count;
    }
}