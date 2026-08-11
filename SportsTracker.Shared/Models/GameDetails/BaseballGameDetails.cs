namespace SportsTracker.Shared.Models.GameDetails
{
    public sealed class BaseballGameDetails
    {
        public Athlete? AwayProbablePitcher { get; init; }
        public Athlete? HomeProbablePitcher { get; init; }
        
        public string? AwayProbablePitcherRecord { get; init; }
        public string? HomeProbablePitcherRecord { get; init; }
    }
}