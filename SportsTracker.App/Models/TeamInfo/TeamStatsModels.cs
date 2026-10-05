namespace SportsTracker.App.Models.TeamInfo
{
    public sealed class TeamStats
    {
        public int Season { get; init; }
        public int SeasonType { get; init; }
        
        public IReadOnlyList<TeamStatCategory> Categories { get; init; } = [];
    }

    public sealed class TeamStatCategory
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        
        public IReadOnlyList<TeamStatistic> Stats { get; init; } = [];
    }

    public sealed class TeamStatistic
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public double Value { get; init; }
        public string DisplayValue { get; init; } = string.Empty;
        
        public int? Rank { get; init; }
        
        public string? RankDisplayValue { get; init; }
    }
}