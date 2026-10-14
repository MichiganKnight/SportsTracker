namespace SportsTracker.App.Models.Sport
{
    public sealed class HockeyShot
    {
        public string? Id { get; init; } = string.Empty;
        
        public string? Description { get; init; }
        public string? TeamId { get; init; }
        public string? Clock { get; init; }
        public string? Strength { get; init; }

        public int Period { get; init; }

        public double X { get; init; }
        public double Y { get; init; }

        public bool IsGoal { get; init; }
        public bool IsSaved { get; init; }
        public bool IsMissed { get; init; }
    }
}