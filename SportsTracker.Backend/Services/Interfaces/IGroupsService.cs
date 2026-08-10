using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Services.Interfaces
{
    public interface IGroupsService
    {
        Task<IReadOnlyList<SportsGroup>?> GetGroupsAsync(League league, CancellationToken cancellationToken = default);
    }
}