namespace SportsTracker.App.Models.Sport
{
    public class BaseballSituation
    {
        public int? Balls { get; init; }
        public int? Strikes { get; init; }
        public int? Outs { get; init; }
        
        public bool OnFirst { get; init; }
        public bool OnSecond { get; init; }
        public bool OnThird { get; init; }
        
        public Athlete? Batter { get; init; }
        public Athlete? Pitcher { get; init; }
        
        public IReadOnlyList<Athlete> DueUp { get; init; } = [];
        
        public string? LastPlay { get; init; }
    }
}