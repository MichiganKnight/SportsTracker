using SportsTracker.Frontend.ViewModels.GameInfo;

namespace SportsTracker.Frontend.ViewModels.TeamInfo
{
    public sealed class TeamSchedulePageViewModel
    {
        public TeamDetailsViewModel Team { get; init; } = new();
        
        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
    }
}