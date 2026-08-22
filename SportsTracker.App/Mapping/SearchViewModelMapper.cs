using SportsTracker.App.Models;
using SportsTracker.App.ViewModels.SearchInfo;

namespace SportsTracker.App.Mapping
{
    public interface ISearchViewModelMapper
    {
        SearchPageViewModel Map(SearchResults results);
    }

    public sealed class SearchViewModelMapper : ISearchViewModelMapper
    {
        public SearchPageViewModel Map(SearchResults results)
        {
            return new SearchPageViewModel
            {
                Query = results.Query,

                DidYouMean = results.DidYouMean,

                Players = results.Players.Select(MapResult).ToList(),
                Teams = results.Teams.Select(MapResult).ToList(),
                Games = results.Games.Select(MapResult).ToList()
            };
        }

        private static SearchResultViewModel MapResult(SearchResult result)
        {
            return new SearchResultViewModel
            {
                Type = result.Type,

                Id = result.Id,

                League = result.League,

                DisplayName = result.DisplayName,
                Subtitle = result.Subtitle,
                Description = result.Description,

                Image = result.Image,
                DarkImage = result.DarkImage,

                Date = result.Date
            };
        }
    }
}