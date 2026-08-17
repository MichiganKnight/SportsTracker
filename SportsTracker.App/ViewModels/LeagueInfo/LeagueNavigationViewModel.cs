using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueNavigationViewModel
    {
        public League League { get; init; }
        public LeaguePage ActivePage { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
    }
}