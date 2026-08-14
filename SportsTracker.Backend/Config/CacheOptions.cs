namespace SportsTracker.Backend.Config
{
    public sealed class CacheOptions
    {
        public const string SectionName = "Cache";
        
        public int WorkerRefreshSeconds { get; init; } = 15;
        public int LiveScoreboardSeconds { get; init; } = 15;
        public int ScheduledScoreboardMinutes { get; init; } = 2;
        public int FinalScoreboardMinutes { get; init; } = 30;
        
        public int GameDetailsLiveSeconds { get; init; } = 15;
        public int GameDetailsFinalMinutes { get; init; } = 60;
        public int GameDetailsScheduledMinutes { get; init; } = 2;
        
        public int StandingsMinutes { get; init; } = 60;
        public int GroupsMinutes { get; init; } = 1440;
        
        public int GameSummaryLiveSeconds { get; init; } = 15;
        public int GameSummaryScheduledMinutes { get; init; } = 2;
        public int GameSummaryFinalMinutes { get; init; } = 60;
        
        public int TeamMinutes { get; init; } = 60;
        public int TeamScheduleMinutes { get; init; } = 2;
    }
}