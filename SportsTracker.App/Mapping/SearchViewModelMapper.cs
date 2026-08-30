using SportsTracker.App.Models;
using SportsTracker.App.ViewModels.SearchInfo;

namespace SportsTracker.App.Mapping
{
    public interface ISearchViewModelMapper
    {
        SearchPageViewModel Map(SearchResults results);
        
        SearchSuggestionsViewModel MapSuggestions(SearchResults results);
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

        public SearchSuggestionsViewModel MapSuggestions(SearchResults results)
        {
            List<SearchSuggestionViewModel> suggestions = [];
            
            suggestions.AddRange(results.Players.Take(4).Select(MapSuggestion));
            suggestions.AddRange(results.Teams.Take(3).Select(MapSuggestion));
            suggestions.AddRange(results.Games.Take(3).Select(MapSuggestion));

            return new SearchSuggestionsViewModel
            {
                Query = results.Query,
                Results = suggestions
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

        private static SearchSuggestionViewModel MapSuggestion(SearchResult result)
        {
            return new SearchSuggestionViewModel
            {
                Type = result.Type.ToString(),

                Id = result.Id,

                League = result.League.ToString(),
                DisplayName = result.DisplayName,

                Subtitle = result.Subtitle,
                Image = result.Image,
                DarkImage = result.DarkImage,

                Url = GetResultUrl(result)
            };
        }

        private static string GetResultUrl(SearchResult result)
        {
            return result.Type switch
            {
                SearchResultType.Player => $"/athlete/{result.League}/{result.Id}",
                SearchResultType.Team => $"/team/{result.League}/{result.Id}",
                SearchResultType.Game => $"/game/{result.League}/{result.Id}",
                
                _ => string.Empty
            };
        }
    }
}