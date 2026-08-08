namespace SportsTracker.Shared.Models.Sport
{
    public class BaseballSituation
    {
        public int? Inning { get; init; }
        public string? InningState { get; init; }
        
        public int? Balls { get; init; }
        public int? Strikes { get; init; }
        public int? Outs { get; init; }
        
        public bool RunnerOnFirst { get; init; }
        public bool RunnerOnSecond { get; init; }
        public bool RunnerOnThird { get; init; }
        
        public Athlete? Batter { get; init; }
        public Athlete? Pitcher { get; init; }
        
        public string? LastPlay { get; init; }
    }
}