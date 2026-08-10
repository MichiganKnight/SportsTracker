using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IStandingsService
    {
        Task<LeagueStandings?> GetStandingsAsync(League league, CancellationToken cancellationToken = default);
    }
}