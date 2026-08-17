namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class RosterGroupViewModel
    {
        public string Name { get; init; } = string.Empty;
        
        public IReadOnlyList<RosterPlayerViewModel> Players { get; init; } = [];
    }
}