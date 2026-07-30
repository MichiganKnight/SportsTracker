namespace SportsTracker.Frontend.ViewModels
{
    public sealed class GameCardViewModel
    {
        public string GameId { get; init; } = string.Empty;
        
        public TeamViewModel AwayTeam { get; init; } = new();
        public TeamViewModel HomeTeam { get; init; } = new();
        
        public string Status { get; init; } = string.Empty;
        public bool IsLive { get; init; }
    }
}