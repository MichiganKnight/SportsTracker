using System.Globalization;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Mapping
{
    public interface IAthleteDetailsViewModelMapper
    {
        AthleteDetailsViewModel Map(AthleteDetails athlete);
    }
    
    public class AthleteDetailsViewModelMapper : IAthleteDetailsViewModelMapper
    {
        public AthleteDetailsViewModel Map(AthleteDetails athlete)
        {
            return new AthleteDetailsViewModel
            {
                Id = athlete.Id,
                
                League = athlete.League,
                
                DisplayName = athlete.DisplayName,
                
                Headshot = athlete.Headshot,
                
                IsActive = athlete.IsActive,
                
                Status = athlete.Status,
                
                TeamId = athlete.Team?.Id,
                TeamName = athlete.Team?.DisplayName,
                
                TeamLogo = athlete.Team?.Logo,
                TeamDarkLogo = athlete.Team?.DarkLogo,
                
                Position = athlete.Position?.Name,
                
                Jersey = athlete.Jersey,
                
                Age = athlete.Age,
                
                DateOfBirth = FormatDateOfBirth(athlete.DateOfBirth),
                BirthPlace = athlete.BirthPlace,
                
                Height = athlete.Height,
                Weight = athlete.Weight,
                
                College = athlete.College,
                Experience = athlete.Experience,
                Draft = athlete.Draft,
                BatsThrows = athlete.BatsThrows,
                
                TurnedPro = athlete.TurnedPro,
                
                Hand = athlete.Hand,
                
                Citizenship = athlete.Citizenship,
                CountryFlag = athlete.CountryFlag,
                
                StatsSummaryTitle = athlete.StatsSummaryTitle,
                
                StatsSummary = athlete.StatsSummary
                    .Select(stat => new AthleteStatSummaryViewModel
                    {
                        Name = stat.Name,
                        Label = stat.ShortDisplayName,
                        Value = stat.DisplayValue,
                        
                        Rank = stat.RankDisplayValue
                    })
                    .ToList()
            };
        }

        private static string? FormatDateOfBirth(string? dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return null;
            }
            
            string[] formats =
            [
                "M/d/yyyy",
                "MM/dd/yyyy",
                "d/M/yyyy",
                "dd/MM/yyyy"
            ];

            return DateTime.TryParseExact(dateOfBirth, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date.ToString("d") : dateOfBirth;
        }
    }
}