namespace SportsTracker.App.ViewModels.PlayByPlay
{
    public sealed class GamePlayViewModel
    {
        public string Id { get; init; } = string.Empty;
        
        public string? Type { get; init; }
        public string? Text { get; init; }
        public string? Period { get; init; }
        public string? Clock { get; init; }

        public string Category { get; init; } = "other";
        
        public bool ScoringPlay { get; init; }
        
        public string? TeamId { get; init;}
        
        public int? AwayScore { get; init; }
        public int? HomeScore { get; init; }
        
        public string? GroupId { get; init; }
        
        public string? Situation { get; init; }
        
        public string? Context { get; init; }
    }
}