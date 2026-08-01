using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public interface IDashboardMapper
    {
        DashboardViewModel Map(IReadOnlyDictionary<League, IReadOnlyList<Game>?> scoreboards);
    }
}