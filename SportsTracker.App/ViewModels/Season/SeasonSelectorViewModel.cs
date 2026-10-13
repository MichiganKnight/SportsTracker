using SportsTracker.App.Enums;
using SportsTracker.App.Models.Season;

namespace SportsTracker.App.ViewModels.Season
{
    public sealed class SeasonSelectorViewModel
    {
        public League League { get; init; }

        public int SelectedSeason { get; init; }
        public int SelectedSeasonType { get; init; }

        public IReadOnlyList<int> Seasons { get; init; } = [];

        public IReadOnlyList<SeasonPhaseOption> Phases { get; init; } = [];

        public string Controller { get; init; } = string.Empty;
        public string Action { get; init; } = string.Empty;

        public Dictionary<string, string> RouteValues { get; init; } = [];
    }
}