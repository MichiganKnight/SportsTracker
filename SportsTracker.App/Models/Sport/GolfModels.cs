namespace SportsTracker.App.Models.Sport
{
    public sealed class GolfTournament
    {
        public string Name { get; init; } = string.Empty;
        
        public DateTime? EndTime { get; init; }
        
        public IReadOnlyList<GolfLeaderboardEntry> Leaderboard { get; init; } = [];
    }

    public sealed class GolfLeaderboardEntry
    {
        public string AthleteId { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? CountryFlag { get; init; }
        public string? Country { get; init; }
        
        public int? Position { get; init; }
        
        public string ScoreToPar { get; init; } = string.Empty;
        
        public IReadOnlyList<GolfRound> Rounds { get; init; } = [];
    }

    public sealed class GolfRound
    {
        public int Round { get; init; }
        
        public int? Strokes { get; init; }
        
        public string ScoreToPar { get; init; } = string.Empty;
        
        public IReadOnlyList<GolfHole> Holes { get; init; } = [];
    }

    public sealed class GolfHole
    {
        public int Hole { get; init; }
        
        public int? Strokes { get; init; }
        
        public string ScoreToPar { get; init; } = string.Empty;
    }
}