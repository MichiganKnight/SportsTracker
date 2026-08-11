namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class GameDetailsStatDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayValue { get; init; }
        public string? RankDisplayValue { get; init; }
    }
}