using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.GameDetails
{
    public sealed class GameDetails
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public DateTime StartTime { get; init; }
        
        public string Status { get; init; } = string.Empty;
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }

        public GameDetailsTeam AwayTeam { get; init; } = null!;
        public GameDetailsTeam HomeTeam { get; init; } = null!;
        
        public string? Venue { get; init; }
        public string? VenueCity { get; init; }
        public string? VenuState { get; init; }
        
        public int? Attendance { get; init; }
        
        public IReadOnlyList<string> Broadcasts { get; init; } = [];
        public IReadOnlyList<FeaturedAthlete> FeaturedAthletes { get; init; } = [];
        
        public string? Headline { get; init; }
        public string? Recap { get; init; }
        
        public BaseballGameDetails? Baseball { get; init; }
    }
}