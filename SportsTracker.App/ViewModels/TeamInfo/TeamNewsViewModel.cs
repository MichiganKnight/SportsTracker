using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamNewsViewModel
    {
        public TeamDetailsViewModel Team { get; init; } = null!;
        
        public IReadOnlyList<LeagueNewsArticleViewModel> Articles { get; init; } = [];
    }
}