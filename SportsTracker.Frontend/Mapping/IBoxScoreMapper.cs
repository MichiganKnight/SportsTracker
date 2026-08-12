using SportsTracker.Frontend.ViewModels.BoxScore;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Frontend.Mapping
{
    public interface IBoxScoreMapper
    {
        BoxScoreViewModel Map(GameBoxScore boxScore);
    }
}