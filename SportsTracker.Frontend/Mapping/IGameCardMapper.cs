using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Frontend.Mapping
{
    public interface IGameCardMapper
    {
        GameCardViewModel Map(Game game);
    }
}