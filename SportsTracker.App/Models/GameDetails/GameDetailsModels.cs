using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.GameDetails
{
    public sealed class GameDetails
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public DateTime StartTime { get; init; }
        
        public string Status { get; init; } = string.Empty;
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }
        public bool IsScheduled { get; init; }

        public GameDetailsTeam AwayTeam { get; init; } = null!;
        public GameDetailsTeam HomeTeam { get; init; } = null!;
        
        public string? Venue { get; init; }
        public string? VenueCity { get; init; }
        public string? VenueState { get; init; }
        
        public int? Attendance { get; init; }
        
        public IReadOnlyList<string> Broadcasts { get; init; } = [];
        public IReadOnlyList<FeaturedAthlete> FeaturedAthletes { get; init; } = [];
        
        public string? Headline { get; init; }
        public string? Recap { get; init; }
        
        public BaseballGameDetails? Baseball { get; init; }
    }
    
    public sealed class GameDetailsTeam
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public int Score { get; init; }
        
        public bool Winner { get; init; }
        
        public string? Record { get; init; }
        
        public int? Hits { get; init; }
        public int? Errors { get; init; }
        
        public IReadOnlyList<LineScore> LineScores { get; init; } = [];
    }
    
    public sealed class LineScore
    {
        public int Period { get; init; }
        
        public double Value { get; init; }
        
        public string DisplayValue { get; init; } = string.Empty;
    }
    
    public sealed class FeaturedAthlete
    {
        public string Type { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;

        public Athlete Athlete { get; init; } = null!;
        
        public string? TeamId { get; init; }
    }
    
    public sealed class BaseballGameDetails
    {
        public Athlete? AwayProbablePitcher { get; init; }
        public Athlete? HomeProbablePitcher { get; init; }
        
        public string? AwayProbablePitcherRecord { get; init; }
        public string? HomeProbablePitcherRecord { get; init; }
    }
}