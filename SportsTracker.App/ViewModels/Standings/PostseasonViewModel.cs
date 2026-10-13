using SportsTracker.App.Enums;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.Season;

namespace SportsTracker.App.ViewModels.Standings
{
    public sealed class PostseasonViewModel
    {
        public League League { get; init; }

        public string LeagueName { get; init; } = string.Empty;

        public SeasonSelectorViewModel SeasonSelector { get; init; } = new();

        public int SelectedRound { get; init; }

        public IReadOnlyList<PostseasonRoundViewModel> Rounds { get; init; } = [];
        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];

        public PostseasonRoundViewModel? CurrentRound => Rounds.FirstOrDefault(round => round.Number == SelectedRound);
    }
    
    public sealed class PostseasonRoundViewModel
    {
        public int Number { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;

        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}