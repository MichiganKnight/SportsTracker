namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthletePageViewModel<TContent>
    {
        public AthleteDetailsViewModel Athlete { get; init; } = new();

        public TContent Content { get; init; } = default!;
    }
}