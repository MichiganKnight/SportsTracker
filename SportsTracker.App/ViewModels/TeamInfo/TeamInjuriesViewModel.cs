using SportsTracker.App.Enums;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamInjuriesViewModel
    {
        public League League { get; init; }
        
        public TeamDetailsViewModel Team { get; init; } = null!;
        
        public IReadOnlyList<TeamInjury> Injuries { get; init; } = [];
    }
}