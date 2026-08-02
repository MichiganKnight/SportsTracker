using SportsTracker.Frontend.ViewModels.TeamInfo;

namespace SportsTracker.Frontend.ViewModels.GameInfo
{
    public sealed class GameCardViewModel
    {
        public string GameId { get; init; } = string.Empty;
        
        public TeamViewModel HomeTeam { get; init; } = null!;
        public TeamViewModel AwayTeam { get; init; } = null!;
        
        public string Status { get; init; } = string.Empty;
        
        public string? SituationPrimary { get; init; }
        public string? SituationSecondary { get; init; }
        public string? SituationDetail { get; init; }
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }
        public bool IsUpcoming { get; init; }
        
        public string? Venue { get; init; }
        
        public DateTime StartTime { get; init; }
        
        public bool IsNeutralSite { get; init; }
    }
}