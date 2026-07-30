namespace SportsTracker.Frontend.ViewModels
{
    public sealed class DashboardViewModel
    {
        public IReadOnlyList<LeagueSectionViewModel> Leagues { get; init; } = [];
    }
}