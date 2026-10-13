using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.Season
{
    public sealed class SeasonContext
    {
        public int Season { get; init; }
        
        public SeasonPhase Phase { get; init; }
        
        public int SeasonType => (int)Phase;
    }
}