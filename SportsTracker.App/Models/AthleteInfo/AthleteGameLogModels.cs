namespace SportsTracker.App.Models.AthleteInfo
{
    public sealed class AthleteGameLog
    {
        public IReadOnlyList<AthleteGameLogColumn> Columns { get; init; } = [];
        public IReadOnlyList<AthleteGameLogSeason> Seasons { get; init; } = [];
    }

    public sealed class AthleteGameLogColumn
    {
        public string Name { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }
    
    public sealed class AthleteGameLogSeason
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public string? TeamAbbreviation { get; init; }
        
        public IReadOnlyList<AthleteGameLogCategory> Categories { get; init; } = [];
    }

    public sealed class AthleteGameLogCategory
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public string? SplitType { get; init; }
        
        public IReadOnlyList<AthleteGameLogGame> Games { get; init; } = [];
        public IReadOnlyList<string> Totals { get; init; } = [];
    }

    public sealed class AthleteGameLogGame
    {
        public string EventId { get; init; } = string.Empty;
        
        public DateTimeOffset? GameDate { get; init; }
        
        public string Result { get; init; } = string.Empty;
        public string Score { get; init; } = string.Empty;
        public string AtVs { get; init; } = string.Empty;
        
        public string? EventNote { get; init; }
        
        public string OpponentId { get; init; } = string.Empty;
        public string OpponentName { get; init; } = string.Empty;
        public string OpponentAbbreviation { get; init; } = string.Empty;
        
        public string? OpponentLogo { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
}