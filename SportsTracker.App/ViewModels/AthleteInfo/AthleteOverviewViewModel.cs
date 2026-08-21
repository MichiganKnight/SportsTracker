namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthleteOverviewViewModel
    {
        public string StatisticsTitle { get; init; } = string.Empty;

        public IReadOnlyList<AthleteOverviewStatColumnViewModel> StatColumns { get; init; } = [];
        public IReadOnlyList<AthleteOverviewStatRowViewModel> StatRows { get; init; } = [];
        
        public AthleteNewsViewModel? LatestNews { get; init; }
        public AthleteAnalysisViewModel? Analysis { get; init; }
        
        public IReadOnlyList<AthleteAwardViewModel> Awards { get; init; } = [];
        
        public AthleteFantasyViewModel? Fantasy { get; init; }
        
        public string? GolfSeasonRankingsTitle { get; init; }
        
        public IReadOnlyList<GolfSeasonRankingViewModel> GolfSeasonRankings { get; init; } = [];
        public IReadOnlyList<GolfRecentTournamentViewModel> GolfRecentTournaments { get; init; } = [];
        
        public bool HasStatistics => StatColumns.Count > 0 && StatRows.Count > 0;
        public bool HasNews => LatestNews is not null;
        public bool HasAnalysis => Analysis is not null;
        public bool HasAwards => Awards.Count > 0;
        public bool HasFantasy => Fantasy is not null;
        
        public bool HasGolfRankings => GolfSeasonRankings.Count > 0;
        public bool HasGolfRecentTournaments => GolfRecentTournaments.Count > 0;
    }

    public sealed class AthleteOverviewStatColumnViewModel
    {
        public string Label  { get; init; } = string.Empty;
        public string DisplayName { get; init; }  = string.Empty;
    }

    public sealed class AthleteOverviewStatRowViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<string> Stats  { get; init; } = [];
    }

    public sealed class AthleteNewsViewModel
    {
        public string Headline { get; init; } = string.Empty;
        
        public string? Description { get; init; }
        public string? Image { get; init; }
        
        public DateTime? LastModified { get; init; }
    }
    
    public sealed class AthleteAnalysisViewModel
    {
        public string Headline { get; init; } = string.Empty;

        public string? Story { get; init; }
        public string? Description { get; init; }

        public DateTime? Published { get; init; }
    }
    
    public sealed class AthleteAwardViewModel
    {
        public string Name { get; init; } = string.Empty;

        public string? DisplayCount { get; init; }

        public IReadOnlyList<string> Seasons { get; init; } = [];
    }
    
    public sealed class AthleteFantasyViewModel
    {
        public string? DraftRank { get; init; }
        public string? PositionRank { get; init; }
        public string? PercentOwned { get; init; }
        public string? Projection { get; init; }
    }
    
    public sealed class GolfSeasonRankingViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        public string DisplayValue { get; init; } = string.Empty;

        public string? Rank { get; init; }
    }
    
    public sealed class GolfRecentTournamentViewModel
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