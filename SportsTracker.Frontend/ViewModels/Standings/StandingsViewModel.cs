using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.Standings
{
    public sealed class StandingsViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        
        public int Season { get; init; }
        
        public DateTime? LastUpdatedUtc { get; init; }
        
        public StandingsView SelectedView { get; init; }
        
        public IReadOnlyList<StandingsView> AvailableViews { get; init; } = [];
        public IReadOnlyList<StandingsGroupViewModel> Groups { get; init; } = [];
    }
}