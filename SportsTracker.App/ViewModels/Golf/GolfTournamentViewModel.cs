namespace SportsTracker.App.ViewModels.Golf
{
    public sealed class GolfTournamentViewModel
    {
        public string EventId { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        
        public DateTime StartTime { get; init; }
        public DateTime? EndTime { get; init; }
        
        public string Status { get; init; } = string.Empty;
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }
        public bool IsUpcoming { get; init; }
        
        public string? Venue { get; init; }
        public string? Location { get; init; }
        
        public IReadOnlyList<GolfLeaderboardRowViewModel> Leaderboard { get; init; } = [];
    }
}