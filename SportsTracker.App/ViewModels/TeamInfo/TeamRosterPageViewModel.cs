namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamRosterPageViewModel
    {
        public TeamDetailsViewModel Team { get; init; } = new();
        
        public int? Season { get; init; }
        
        public string? SeasonName { get; init; }
        
        public IReadOnlyList<RosterGroupViewModel> Groups { get; init; } = [];
    }
}