using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class AthleteDetailsMapper
    {
        public static AthleteDetails? Map(AthleteProfileResponseDto response, League league)
        {
            AthleteProfileDto? dto = response.Athlete;

            if (dto is null)
            {
                return null;
            }

            return new AthleteDetails
            {
                Id = dto.Id ?? string.Empty,

                League = league,

                FirstName = dto.FirstName ?? string.Empty,
                LastName = dto.LastName ?? string.Empty,
                DisplayName = dto.DisplayName ?? dto.FullName ?? BuildDisplayName(dto),

                Headshot = dto.Headshot?.Href,

                IsActive = dto.Active == true,

                Status = dto.Status?.Name ?? dto.Status?.Abbreviation,

                Age = dto.Age,
                DateOfBirth = dto.DisplayDOB,
                BirthPlace = dto.DisplayBirthPlace,

                Height = dto.DisplayHeight,
                Weight = dto.DisplayWeight,

                DebutYear = dto.DebutYear,

                Team = MapTeam(dto.Team),

                Position = MapPosition(dto.Position),

                Jersey = dto.DisplayJersey ?? dto.Jersey,

                College = dto.College?.Name ?? dto.College?.ShortName,

                Experience = dto.DisplayExperience,

                Draft = dto.DisplayDraft,

                BatsThrows = dto.DisplayBatsThrows,

                TurnedPro = dto.TurnedPro,

                Hand = dto.Hand?.DisplayValue ?? dto.Hand?.Abbreviation ?? dto.Hand?.Type,

                Citizenship = dto.Citizenship,
                CountryFlag = dto.Flag?.Href,

                StatsSummaryTitle = dto.StatsSummary?.DisplayName,

                StatsSummary = MapStatsSummary(dto.StatsSummary)
            };
        }

        private static AthleteTeam? MapTeam(AthleteProfileTeamDto? dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new AthleteTeam
            {
                Id = dto.Id ?? string.Empty,

                DisplayName = dto.DisplayName ?? string.Empty,
                Abbreviation = dto.Abbreviation ?? string.Empty,

                Logo = SelectLogo(dto.Logos, dark: false),
                DarkLogo = SelectLogo(dto.Logos, dark: true),

                Color = dto.Color,
                AlternateColor = dto.AlternateColor
            };
        }

        private static AthletePosition? MapPosition(AthletePositionDto? dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new AthletePosition
            {
                Id = dto.Id ?? string.Empty,
                Name = dto.DisplayName ?? dto.Name ?? string.Empty,
                Abbreviation = dto.Abbreviation ?? string.Empty,
            };
        }

        private static IReadOnlyList<AthleteStatSummary> MapStatsSummary(AthleteStatsSummaryDto dto)
        {
            if (dto?.Statistics is null || dto.Statistics.Count == 0)
            {
                return [];
            }

            return dto.Statistics
                .Where(stat => !string.IsNullOrWhiteSpace(stat.DisplayValue))
                .Select(stat => new AthleteStatSummary
                {
                    Name = stat.Name ?? string.Empty,
                    DisplayName = stat.DisplayName ?? stat.ShortDisplayName ?? stat.Abbreviation ?? stat.Name ?? string.Empty,
                    ShortDisplayName = stat.ShortDisplayName ?? stat.DisplayName ?? string.Empty,
                    Abbreviation = stat.Abbreviation ?? string.Empty,

                    Value = stat.Value,
                    DisplayValue = stat.DisplayValue,

                    Rank = stat.Rank,
                    RankDisplayValue = stat.RankDisplayValue
                })
                .ToList();
        }

        private static string? SelectLogo(IReadOnlyList<EspnLogoDto>? logos, bool dark)
        {
            if (logos is null || logos.Count == 0)
            {
                return null;
            }
            
            EspnLogoDto? logo = logos.FirstOrDefault(x => HasRel(x, "full") && HasRel(x, dark ? "dark" : "default"));
            
            logo ??= logos.FirstOrDefault(x => dark ? HasRel(x, "dark") : !HasRel(x, "dark"));
            
            logo ??= logos.FirstOrDefault();
            
            return logo?.Href;
        }

        private static bool HasRel(EspnLogoDto logo, string rel)
        {
            return logo.Rel?.Contains(rel, StringComparer.OrdinalIgnoreCase) == true;
        }

        private static string BuildDisplayName(AthleteProfileDto dto)
        {
            return string.Join(" ", new[]
            {
                dto.FirstName,
                dto.LastName
            }
            .Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}