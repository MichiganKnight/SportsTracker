namespace SportsTracker.Shared.Models
{
    public class BaseballSituation
    {
        public int? Balls { get; init; }
        public int? Strikes { get; init; }
        public int? Outs { get; init; }
        
        public bool RunnerOnFirst { get; init; }
        public bool RunnerOnSecond { get; init; }
        public bool RunnerOnThird { get; init; }
    }
}