using SportsTracker.Frontend.ViewModels.PlayByPlay;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Mapping
{
    public interface IPlayByPlayMapper
    {
        PlayByPlayViewModel Map(GamePlayByPlay playByPlay);
    }
}