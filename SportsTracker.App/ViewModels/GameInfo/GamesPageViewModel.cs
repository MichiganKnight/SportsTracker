using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.ViewModels.GameInfo
{
    public sealed class GamesPageViewModel
    {
        public DateOnly Date { get; init; }
        
        public IReadOnlyList<LeagueSectionViewModel> Leagues { get; init; } = [];
        
        public DateOnly PreviousDate => Date.AddDays(-1);
        public DateOnly NextDate => Date.AddDays(1);
        
        public bool IsToday => Date == DateOnly.FromDateTime(DateTime.Today);
        
        public bool HasEvents => Leagues.Any(league => league.HasEvents);
        
        public int TotalEvents => Leagues.Sum(league => league.TotalEvents);
    }
}