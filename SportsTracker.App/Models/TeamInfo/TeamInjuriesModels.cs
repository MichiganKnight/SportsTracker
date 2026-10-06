namespace SportsTracker.App.Models.TeamInfo
{
    public class TeamInjuries
    {
        public IReadOnlyList<TeamInjury> Injuries { get; init; } = [];
    }

    public sealed class TeamInjury
    {
        public string Id { get; init; } = string.Empty;
        
        public string AthleteRef { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string StatusAbbreviation { get; init; } = string.Empty;
        
        public DateTime? Date { get; init; }
        
        public string ShortComment { get; init; } = string.Empty;
        public string LongComment { get; init; } = string.Empty;
        
        public InjuryDetails? Details { get; init; }
        public InjuryAthlete? Athlete { get; init; }
    }
    
    public sealed class InjuryDetails
    {
        public string Type { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;
        public string Side { get; init; } = string.Empty;
        
        public DateOnly? ReturnDate { get; init; }
    }
    
    public sealed class InjuryAthlete
    {
        public string Id { get; init; } = string.Empty;

        public string DisplayName { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Jersey { get; init; } = string.Empty;
        public string Position { get; init; } = string.Empty;
        public string PositionAbbreviation { get; init; } = string.Empty;
        public string HeadshotUrl { get; init; } = string.Empty;
    }
}