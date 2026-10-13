namespace SportsTracker.App.Models.Season
{
    public sealed class SeasonCalendar
    {
        public int Season { get; init; }

        public IReadOnlyList<SeasonPeriod> Periods { get; init; } = [];
    }
    
    public sealed class SeasonPeriod
    {
        public int SeasonType { get; init; }

        public string Name { get; init; } = string.Empty;

        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }

        public IReadOnlyList<SeasonWeek> Weeks { get; init; } = [];
    }
    
    public sealed class SeasonWeek
    {
        public int Number { get; init; }

        public string Name { get; init; } = string.Empty;
        public string AlternateName { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;

        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}