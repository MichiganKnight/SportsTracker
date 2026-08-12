namespace SportsTracker.Backend.Config
{
    public sealed class CacheOptions
    {
        public const string SectionName = "Cache";
        
        public int WorkerRefreshSeconds { get; init; } = 15;
        public int LiveScoreboardSeconds { get; init; } = 15;
        public int ScheduledScoreboardMinutes { get; init; } = 2;
        public int FinalScoreboardMinutes { get; init; } = 30;
        public int TeamMinutes { get; init; } = 60;
        public int StandingsMinutes { get; init; } = 60;
        public int GroupsMinutes { get; init; } = 1440;
        public int GameDetailsLiveSeconds { get; init; } = 15;
        public int GameDetailsFinalMinutes { get; init; } = 60;
        public int BoxScoreLiveSeconds { get; init; } = 15;
        public int BoxScoreFinalMinutes { get; init; } = 60;
        public int PlayByPlayLiveSeconds { get; init; } = 15;
        public int PlayByPlayFinalMinutes { get; init; } = 60;
    }
}