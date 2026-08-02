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
        public GameSituation? Situation { get; init; }

        #region Team Sports
        
        public Team? HomeTeam { get; init; } = null!;
        public Team? AwayTeam { get; init; } = null!;
        
        public int HomeScore { get; init; }
        public int AwayScore { get; init; }
        
        #endregion
        
        #region Athletes
        
        public List<Athlete> Athletes { get; init; } = [];
        
        #endregion
        
        public Venue? Venue { get; init; }
        
        public bool IsNeutralSite { get; init; }

        public bool IsUpcoming => Status is GameStatus.Scheduled or GameStatus.Pregame;
        public bool IsLive => Status is GameStatus.InProgress or GameStatus.Halftime;
        public bool IsFinal => Status is GameStatus.Final or GameStatus.FinalOvertime or GameStatus.FinalShootout;
        public bool IsDelayed => Status is GameStatus.Delayed or GameStatus.Postponed or GameStatus.Suspended;
        public bool IsCancelled => Status is GameStatus.Cancelled;

        public string StatusBadge => Status switch
        {
            GameStatus.InProgress => "LIVE",
            GameStatus.Halftime => "HALFTIME",

            GameStatus.Final => "FINAL",
            GameStatus.FinalOvertime => "FINAL / OT",
            GameStatus.FinalShootout => "FINAL / SO",

            GameStatus.Delayed => "DELAYED",
            GameStatus.Postponed => "POSTPONED",
            GameStatus.Suspended => "SUSPENDED",
            GameStatus.Cancelled => "CANCELLED",

            GameStatus.Pregame => "PREGAME",

            _ => StatusText
        };
    }
}