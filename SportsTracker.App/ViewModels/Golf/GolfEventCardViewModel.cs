namespace SportsTracker.App.ViewModels.Golf
{
    public sealed class GolfEventCardViewModel
    {
        public string EventId { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        
        public DateTime StartTime { get; init; }
        public DateTime? EndTime { get; init; }
        
        public string Status { get; init; } = string.Empty;
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }
        public bool IsUpcoming { get; init; }
        
        public IReadOnlyList<GolfLeaderboardRowViewModel> Leaders { get; init; } = [];
    }
    
    public sealed class GolfLeaderboardRowViewModel
    {
        public string AthleteId { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        
        public string? CountryFlag { get; init; }
        public string? Country { get; init; }
        
        public int? Position { get; init; }
        
        public string Score { get; init; } = string.Empty;
    }
}