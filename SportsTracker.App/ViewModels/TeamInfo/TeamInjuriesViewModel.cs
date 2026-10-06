using SportsTracker.App.Enums;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class TeamInjuriesViewModel
    {
        public League League { get; init; }
        
        public string TeamId { get; init; } = string.Empty;
        public string TeamName { get; init; } = string.Empty;
        public string? TeamLogo { get; init; } = string.Empty;
        
        public IReadOnlyList<TeamInjury> Injuries { get; init; } = [];
    }
}