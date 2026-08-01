using SportsTracker.Frontend.ViewModels.LeagueInfo;

namespace SportsTracker.Frontend.ViewModels.DashboardInfo
{
    public sealed class DashboardViewModel
    {
        public IReadOnlyList<LeagueSectionViewModel> Leagues { get; init; } = [];
        
        public DateTime LastUpdatedUtc { get; init; }
        
        public int TotalGames => Leagues.Sum(l => l.GameCount);
        public int LeagueCount => Leagues.Count;
    }
}