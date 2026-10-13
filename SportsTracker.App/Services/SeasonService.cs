using SportsTracker.App.Enums;
using SportsTracker.App.Models.Season;

namespace SportsTracker.App.Services
{
    public interface ISeasonService
    {
        SeasonContext Resolve(League league, int? season, int? seasonType);
        
        IReadOnlyList<int> GetAvailableSeasons(League league);
        IReadOnlyList<SeasonPhaseOption> GetAvailablePhases(League league);
    }

    public sealed class SeasonService : ISeasonService
    {
        public SeasonContext Resolve(League league, int? season, int? seasonType)
        {
            int resolvedSeason = season ?? GetCurrentSeason(league);
            
            IReadOnlyList<SeasonPhaseOption> phases = GetAvailablePhases(league);

            SeasonPhaseOption defaultPhase = phases.FirstOrDefault(option => option.Phase == Enums.SeasonPhase.RegularSeason) ?? phases[0];
            SeasonPhaseOption selectedPhase = phases.FirstOrDefault(option => option.SeasonType == seasonType) ?? defaultPhase;

            return new SeasonContext
            {
                Season = resolvedSeason,
                Phase = selectedPhase.Phase,
            };
        }

        public IReadOnlyList<int> GetAvailableSeasons(League league)
        {
            int currentSeason = GetCurrentSeason(league);

            return Enumerable.Range(currentSeason - 9, 10).Reverse().ToList();
        }

        public IReadOnlyList<SeasonPhaseOption> GetAvailablePhases(League league)
        {
            return league switch
            {
                League.NFL =>
                [
                    new SeasonPhaseOption
                    {
                        Phase = Enums.SeasonPhase.Preseason,
                        DisplayName = "Preseason"
                    },
                    new SeasonPhaseOption
                    {
                        Phase = Enums.SeasonPhase.RegularSeason,
                        DisplayName = "Regular Season"
                    },
                    new SeasonPhaseOption
                    {
                        Phase = Enums.SeasonPhase.Postseason,
                        DisplayName = "Postseason"
                    }
                ],
                
                _ =>
                [
                    new SeasonPhaseOption
                    {
                        Phase = Enums.SeasonPhase.RegularSeason,
                        DisplayName = "Regular Season"
                    }       
                ]
            };
        }

        private static int GetCurrentSeason(League league)
        {
            DateTime today = DateTime.Today;

            return league switch
            {
                League.NFL when today.Month <= 2 => today.Year - 1,
                
                _ => today.Year
            };
        }
    }
}