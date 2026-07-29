namespace SportsTracker.Backend.Config
{
    public sealed class CacheOptions
    {
        public const string SectionName = "Cache";
        
        public int LiveScoreboardSeconds { get; init; } = 15;
        public int ScheduledScoreboardMinutes { get; init; } = 2;
        public int FinalScoreboardMinutes { get; init; } = 30;
        public int TeamMinutes { get; init; } = 60;
        public int StandingsMinutes { get; init; } = 60;
    }
}