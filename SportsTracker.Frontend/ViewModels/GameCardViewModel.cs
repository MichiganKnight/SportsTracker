namespace SportsTracker.Frontend.ViewModels
{
    public sealed class GameCardViewModel
    {
        public string AwayTeam { get; init; } = string.Empty;
        public int AwayScore { get; init; }
        public string HomeTeam { get; init; } = string.Empty;
        public int HomeScore { get; init; }
        public string Status { get; init; } = string.Empty;
        public bool IsLive { get; init; }
    }
}