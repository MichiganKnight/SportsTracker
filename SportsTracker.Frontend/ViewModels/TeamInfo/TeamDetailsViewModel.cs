using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.TeamInfo
{
    public sealed class TeamDetailsViewModel
    {
        public League League { get; init; }
        
        public TeamViewModel Team { get; init; } = new();
        
        public string? DarkLogo { get; init; }
        
        public bool IsActive { get; init; }
        
        public string? GroupId { get; init; }
        
        public IReadOnlyList<TeamRecordViewModel> Records { get; init; } = [];
        
        public TeamVenueViewModel? Venue { get; init; }

        public string? OverallRecord => GetRecord("total");
        public string? HomeRecord => GetRecord("home");
        public string? AwayRecord => GetRecord("road");

        private string? GetRecord(string type)
        {
            return Records.FirstOrDefault(record => record.Type.Equals(type, StringComparison.OrdinalIgnoreCase))?.Summary;
        }
    }
}