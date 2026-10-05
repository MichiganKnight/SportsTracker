namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamStatsViewModel
    {
        public TeamDetailsViewModel Team { get; init; } = null!;
        
        public int Season { get; init; }
        public int SeasonType { get; init; }

        public string SelectedSection { get; init; } = "Overview";
        
        public IReadOnlyList<string> AvailableSections { get; init; } = [];
        
        public IReadOnlyList<TeamStatSectionViewModel> Sections { get; init; } = [];
    }

    public sealed class TeamStatSectionViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        
        public IReadOnlyList<TeamStatisticViewModel> Stats { get; init; } = [];
    }

    public sealed class TeamStatisticViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        
        public string? Rank { get; init; }
    }
}