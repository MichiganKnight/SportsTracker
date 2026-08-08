using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class AthleteMapper
    {
        public static Athlete? Map(AthleteDto? athlete)
        {
            if (athlete is null || string.IsNullOrWhiteSpace(athlete.Id))
            {
                return null;
            }

            return new Athlete
            {
                Id = athlete.Id,
                Name = !string.IsNullOrWhiteSpace(athlete.DisplayName) ? athlete.DisplayName : athlete.FullName,
                ShortName = athlete.ShortName,
                Headshot = athlete.Headshot,
                Jersey = athlete.Jersey,
                Position = athlete.Position?.Abbreviation,
                TeamId = athlete.Team?.Id
            };
        }
    }
}