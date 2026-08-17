using SportsTracker.App.Enums;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.Golf;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueSectionViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string? Icon { get; init; }

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        public IReadOnlyList<GolfEventCardViewModel> GolfEvents { get; init; } = [];
        
        public int LiveEvents { get; init; }
        public int TotalEvents { get; init; }
        
        public bool IsGolf => League == League.PGA;
        
        public bool HasGames => Games.Count > 0;
        public bool HasGolfEvents => GolfEvents.Count > 0;
        public bool HasEvents => HasGames || HasGolfEvents;
        
        public int GameCount => Games.Count;

        public int DisplayedEvents => IsGolf ? GolfEvents.Count : GameCount;

        public bool HasMoreEvents => TotalEvents > DisplayedEvents;
        
        public DateTime LastUpdatedUtc { get; init; }
    }
}