using SportsTracker.App.ViewModels.GameInfo;

namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamSchedulePageViewModel
    {
        public TeamDetailsViewModel Team { get; init; } = new();
        
        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
    }
}