using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.SearchInfo;

namespace SportsTracker.App.Controllers
{
    [Route("search")]
    public class SearchController(ISearchService searchService, ISearchViewModelMapper searchViewModelMapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? q, CancellationToken cancellationToken)
        {
            string query = q?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new SearchPageViewModel());
            }

            SearchResults? results = await searchService.SearchAsync(query, cancellationToken);

            SearchPageViewModel viewModel = results is null
                ? new SearchPageViewModel
                {
                    Query = query
                }
                : searchViewModelMapper.Map(results);

            return View(viewModel);
        }
    }
}