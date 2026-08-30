namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthleteSplitsViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteSplitCategoryViewModel> Categories { get; init; } = [];
    }

    public sealed class AthleteSplitCategoryViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteSplitColumnViewModel> Columns { get; init; } = [];
        public IReadOnlyList<AthleteSplitRowViewModel> Rows { get; init; } = [];
    }

    public sealed class AthleteSplitColumnViewModel
    {
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }

    public sealed class AthleteSplitRowViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
}