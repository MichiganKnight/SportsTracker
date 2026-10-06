using SportsTracker.App.Integrations.ESPN.DTOs.Team;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class TeamInjuriesMapper
    {
        public static TeamInjury? Map(TeamInjuryDto teamInjury)
        {
            if (!IsInjury(teamInjury))
            {
                return null;
            }

            return new TeamInjury
            {
                Id = teamInjury.Id,
                
                AthleteRef = teamInjury.Athlete?.Ref ?? string.Empty,
                Status = teamInjury.Status ?? teamInjury.Type?.Description ?? string.Empty,
                StatusAbbreviation = teamInjury.Type?.Abbreviation ?? string.Empty,
                Date = teamInjury.Date,
                ShortComment = teamInjury.ShortComment ?? string.Empty,
                LongComment = teamInjury.LongComment ?? string.Empty,

                Details = MapDetails(teamInjury.Details)
            };
        }

        public static InjuryAthlete MapAthlete(RosterAthleteDto athlete)
        {
            return new InjuryAthlete
            {
                Id = athlete.Id ?? string.Empty,
                
                DisplayName = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName ?? string.Empty,
                Jersey = athlete.Jersey ?? string.Empty,
                Position = athlete.Position?.DisplayName ?? athlete.Position?.Name ?? string.Empty,
                PositionAbbreviation = athlete.Position?.Abbreviation ?? string.Empty,
                HeadshotUrl = athlete.Headshot?.Href ?? string.Empty
            };
        }

        private static bool IsInjury(TeamInjuryDto teamInjury)
        {
            if (string.Equals(teamInjury.Status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.Equals(teamInjury.Type?.Description, "active", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            
            return true;
        }

        private static InjuryDetails? MapDetails(InjuryDetailsDto? injuryDetails)
        {
            return new InjuryDetails
            {
                Type = injuryDetails.Type ?? string.Empty,
                Location = injuryDetails.Location ?? string.Empty,
                Detail = injuryDetails.Detail ?? string.Empty,
                Side = injuryDetails.Side ?? string.Empty,
                ReturnDate = injuryDetails.ReturnDate
            };
        }
    }
}