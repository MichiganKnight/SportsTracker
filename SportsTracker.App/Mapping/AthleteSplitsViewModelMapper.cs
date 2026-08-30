using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Mapping
{
    public interface IAthleteSplitsViewModelMapper
    {
        AthleteSplitsViewModel Map(AthleteSplits splits);
    }

    public sealed class AthleteSplitsViewModelMapper : IAthleteSplitsViewModelMapper
    {
        public AthleteSplitsViewModel Map(AthleteSplits splits)
        {
            return new AthleteSplitsViewModel
            {
                DisplayName = splits.DisplayName,

                Categories = splits.Categories
                    .Select(category => new AthleteSplitCategoryViewModel
                    {
                        Name = category.Name,
                        DisplayName = category.DisplayName,

                        Columns = category.Columns
                            .Select(column => new AthleteSplitColumnViewModel
                            {
                                Label = column.Label,
                                DisplayName = column.DisplayName
                            })
                            .ToList(),

                        Rows = category.Rows
                            .Select(row => new AthleteSplitRowViewModel
                            {
                                DisplayName = row.DisplayName,

                                Stats = row.Stats
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}