using SportsTracker.Frontend.ViewModels.DashboardInfo;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public interface IDashboardMapper
    {
        DashboardViewModel Map(IReadOnlyDictionary<League, IReadOnlyList<Game>?> scoreboards);
    }
}