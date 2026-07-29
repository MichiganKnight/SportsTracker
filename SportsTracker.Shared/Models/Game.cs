using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models
{
    public class Game
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public DateTime StartTime { get; init; }
        
        public GameStatus Status { get; init; }
        public string StatusText { get; init; } = string.Empty;

        public Team HomeTeam { get; init; } = null!;
        public Team AwayTeam { get; init; } = null!;
        
        public int HomeScore { get; init; }
        public int AwayScore { get; init; }
        
        public Venue? Venue { get; init; }
        
        public bool IsNeutralSite { get; init; }
    }
}