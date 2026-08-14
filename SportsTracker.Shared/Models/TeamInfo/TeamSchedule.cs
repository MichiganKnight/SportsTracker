using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Shared.Models.TeamInfo
{
    public sealed class TeamSchedule
    {
        public string TeamId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<Game> Games { get; init; } = [];
    }
}