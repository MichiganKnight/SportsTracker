using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueNewsViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Header { get; init; } = string.Empty;
        
        public IReadOnlyList<LeagueNewsArticleViewModel> Articles { get; init; } = [];
    }
}