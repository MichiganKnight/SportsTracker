namespace SportsTracker.App.Models.Sport
{
    public sealed class FootballPlaybackPlay
    {
        public string Id { get; init; } = string.Empty;
        
        public int Index { get; init; }
        public int DriveNumber { get; init; }
        
        public int? Quarter { get; init; }
        public string? Clock { get; init; }
        
        public string Description { get; init; } = string.Empty;
        public string? PlayType { get; init; }
        
        public int? AwayScore { get; init; }
        public int? HomeScore { get; init; }
        
        public bool IsScoringPlay { get; init; }
        
        public FootballSituation? Situation { get; init; }
    }
}