using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.Standings
{
    public sealed class StandingsViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        
        public int Season { get; init; }
        
        public bool ShowTies { get; init; }
        public bool ShowGamesBack { get; init; }
        public bool ShowDifferential { get; init; }
        public bool ShowStreak { get; init; } = true;
        
        public DateTime? LastUpdatedUtc { get; init; }
        
        public StandingsView SelectedView { get; init; }
        
        public IReadOnlyList<StandingsView> AvailableViews { get; init; } = [];
        public IReadOnlyList<StandingsGroupViewModel> Groups { get; init; } = [];
    }
}