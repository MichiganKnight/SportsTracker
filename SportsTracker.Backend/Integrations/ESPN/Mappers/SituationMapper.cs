using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class SituationMapper
    {
        public static GameSituation? Map(CompetitionDto competition, League league)
        {
            StatusDto status = competition.Status;
            
            return league switch
            {
                League.NFL => MapFootball(status),
                League.CFB => MapFootball(status),

                League.NBA => MapBasketball(status),
                League.CBB => MapBasketball(status),

                League.MLB => MapBaseball(competition),

                League.NHL => MapHockey(status),

                League.PGA => null,

                _ => null
            };
        }

        private static GameSituation? MapFootball(StatusDto status)
        {
            return new GameSituation
            {
                Primary = $"Q{status.Period}",
                Secondary = status.DisplayClock,
                Detail = status.Type.ShortDetail
            };
        }

        private static GameSituation? MapBasketball(StatusDto status)
        {
            return new GameSituation
            {
                Primary = $"Q{status.Period}",
                Secondary = status.DisplayClock,
                Detail = status.Type.ShortDetail
            };
        }

        private static GameSituation? MapHockey(StatusDto status)
        {
            return new GameSituation
            {
                Primary = $"Q{status.Period}",
                Secondary = status.DisplayClock,
                Detail = status.Type.ShortDetail
            };
        }

        private static GameSituation MapBaseball(CompetitionDto competition)
        {
            StatusDto status = competition.Status;

            return new GameSituation
            {
                Primary = status.Type.ShortDetail,
                Secondary = competition.Situation?.Outs is int outs ? $"{outs} Out{(outs == 1 ? "" : "s")}" : null
            };
        }
    }
}