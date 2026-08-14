namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameSummary
{
    public sealed class DrivesDto
    {
        public List<DriveDto> Previous { get; init; }
        
        public DriveDto? Current { get; init; }
    }
}