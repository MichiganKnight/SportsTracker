namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public class HockeyCoordinateDto
    {
        public double? X { get; init; }
        public double? Y { get; init; }
    }

    public class HockeyStrengthDto
    {
        public string? Id { get; init; }
        
        public string? Text { get; init; }
        public string? Abbreviation { get; init; }
    }
}