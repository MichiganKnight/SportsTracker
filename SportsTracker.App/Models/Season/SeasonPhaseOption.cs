using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.Season
{
    public sealed class SeasonPhaseOption
    {
        public SeasonPhase Phase { get; init; }
        
        public string DisplayName { get; init; } = string.Empty;
        
        public int SeasonType => (int)Phase;
    }
}