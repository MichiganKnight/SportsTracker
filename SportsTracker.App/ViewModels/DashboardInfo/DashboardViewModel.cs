using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.ViewModels.DashboardInfo
{
    public sealed class DashboardViewModel
    {
        public IReadOnlyList<LeagueSectionViewModel> Leagues { get; init; } = [];
    }
}