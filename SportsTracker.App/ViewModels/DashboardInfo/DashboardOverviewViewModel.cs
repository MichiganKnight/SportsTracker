using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.DashboardInfo
{
    public sealed class DashboardOverviewViewModel
    {
        public int LiveEvents { get; init; }
        
        public IReadOnlyList<DashboardLeagueSummaryViewModel> Leagues { get; init; } = [];
    }

    public sealed class DashboardLeagueSummaryViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string? Icon { get; init; } = string.Empty;
    }
}