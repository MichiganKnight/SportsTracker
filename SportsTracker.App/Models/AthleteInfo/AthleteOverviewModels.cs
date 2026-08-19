namespace SportsTracker.App.Models.AthleteInfo
{
    public sealed class AthleteOverview
    {
        public string StatisticsTitle { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteOverviewStatColumn> StatColumns { get; init; } = [];
        public IReadOnlyList<AthleteOverviewStatRow> StatRows { get; init; } = [];
        
        public AthleteNews? LatestNews { get; init; }
        public AthleteAnalysis? Analysis { get; init; }
        
        public IReadOnlyList<AthleteAward> Awards { get; init; } = [];
        
        public AthleteFantasy? Fantasy { get; init; }
        
        public string? GolfSeasonRankingsTitle { get; init; }
        
        public IReadOnlyList<GolfSeasonRanking> GolfSeasonRankings { get; init; } = [];
        public IReadOnlyList<GolfRecentTournament> GolfRecentTournaments { get; init; } = [];
    }

    public sealed class AthleteOverviewStatColumn
    {
        public string Name { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }

    public sealed class AthleteOverviewStatRow
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }

    public sealed class AthleteNews
    {
        public string Headline { get; init; } = string.Empty;
        
        public string? Description { get; init; }
        
        public DateTime? LastModified { get; init; }
        
        public string? Image { get; init; }
    }
    
    public sealed class AthleteAnalysis
    {
        public string Headline { get; init; } = string.Empty;
        
        public string? Story { get; init; }
        public string? Description { get; init; }
        
        public DateTime? Published { get; init; }
    }

    public sealed class AthleteAward
    {
        public string Name { get; init; } = string.Empty;
        
        public string? DisplayCount { get; init; }
        
        public IReadOnlyList<string> Seasons { get; init; } = [];
    }

    public sealed class AthleteFantasy
    {
        public string? DraftRank { get; init; }
        public string? PositionRank { get; init; }
        public string? PercentOwned { get; init; }
        public string? Projection { get; init; }
    }

    public sealed class GolfSeasonRanking
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string DisplayValue { get; init; } = string.Empty;
        
        public int? Rank { get; init; }
        
        public string? RankDisplayValue { get; init; }
    }
    
    public sealed class GolfRecentTournament
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        
        public string? Position { get; init; }
        public string? ScoreToPar { get; init; }
        
        public IReadOnlyList<int?> RoundScores { get; init; } = [];
    }
}