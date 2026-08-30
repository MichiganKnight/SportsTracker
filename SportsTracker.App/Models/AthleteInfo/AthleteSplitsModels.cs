namespace SportsTracker.App.Models.AthleteInfo
{
    public sealed class AthleteSplits
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteSplitCategory> Categories { get; init; } = [];
    }

    public sealed class AthleteSplitCategory
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteSplitColumn> Columns { get; init; } = [];
        public IReadOnlyList<AthleteSplitRow> Rows { get; init; } = [];
    }

    public sealed class AthleteSplitColumn
    {
        public string Name { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }

    public sealed class AthleteSplitRow
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Abbreviation { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
}