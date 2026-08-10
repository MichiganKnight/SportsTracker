using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Baseball
{
    public sealed class BaseballSituationDto
    {
        public int? Balls { get; init; }
        public int? Outs { get; init; }
        public int? Strikes { get; init; }
        
        public bool OnFirst { get; init; }
        public bool OnSecond { get; init; }
        public bool OnThird { get; init; }
        
        public LastPlayDto? LastPlay { get; init; }
        
        public AthleteDto? Batter { get; init; }
        public AthleteDto? Pitcher { get; init; }
        
        public List<DueUpDto>? DueUp { get; init; } = [];
    }

    public sealed class LastPlayDto
    {
        public string Text { get; init; } = string.Empty;
    }

    public sealed class DueUpDto
    {
        public AthleteDto? Athlete { get; init; }
        
        public string? Summary { get; init; }
        
        public int? BatOrder { get; init; }
    }
}