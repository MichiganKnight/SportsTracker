namespace SportsTracker.Frontend.ViewModels
{
    public sealed class LeagueSectionViewModel
    {
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
    }
}