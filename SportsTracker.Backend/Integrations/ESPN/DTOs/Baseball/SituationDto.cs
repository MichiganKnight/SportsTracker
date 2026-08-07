namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Baseball
{
    public sealed class SituationDto
    {
        public int? Outs { get; init; }
        public int? Balls { get; init; }
        public int? Strikes { get; init; }
        
        public bool OnFirst { get; init; }
        public bool OnSecond { get; init; }
        public bool OnThird { get; init; }
        
        public LastPlayDto? LastPlay { get; init; }
        
        public List<DueUpDto> DueUp { get; init; } = [];
    }

    public sealed class LastPlayDto
    {
        public string Text { get; init; } = string.Empty;
    }

    public sealed class DueUpDto
    {
        //public AthleteDto Athlete { get; init; } = new();
        
        public string Summary { get; init; } = string.Empty;
    }
}