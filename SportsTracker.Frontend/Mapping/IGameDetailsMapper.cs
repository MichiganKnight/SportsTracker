using SportsTracker.Frontend.ViewModels.GameDetails;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Frontend.Mapping
{
    public interface IGameDetailsMapper
    {
        GameDetailsViewModel Map(GameDetails details);
    }
}