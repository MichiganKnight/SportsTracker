using System.Text.Json;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs
{
    public class ScoreboardResponseDto
    {
        public JsonElement Events { get; set; }
    }
}