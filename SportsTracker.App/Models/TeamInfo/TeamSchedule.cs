using SportsTracker.App.Enums;
using SportsTracker.App.Models.GameInfo;

namespace SportsTracker.App.Models.TeamInfo
{
    public sealed class TeamSchedule
    {
        public string TeamId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<Game> Games { get; init; } = [];
    }
}