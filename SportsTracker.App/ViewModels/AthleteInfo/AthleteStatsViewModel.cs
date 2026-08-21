namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthleteStatsViewModel
    {
        public IReadOnlyList<AthleteStatsCategoryViewModel> Categories { get; init; } = [];
        
        public bool HasStats => Categories.Count > 0;
    }

    public sealed class AthleteStatsCategoryViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteStatsColumnViewModel> Columns { get; init; } = [];
        public IReadOnlyList<AthleteStatsRowViewModel> Rows { get; init; } = [];
        
        public IReadOnlyList<string> Totals { get; init; } = [];
    }

    public sealed class AthleteStatsColumnViewModel
    {
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Description { get; init; }
    }

    public sealed class AthleteStatsRowViewModel
    {
        public string Season { get; init; } = string.Empty;
        
        public string TeamId { get; init; } = string.Empty;
        
        public string TeamName { get; init; } = string.Empty;
        public string TeamAbbreviation { get; init; } = string.Empty;
        
        public string? TeamLogo { get; init; }
        public string? TeamDarkLogo { get; init; }
        
        public string? Position { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
}