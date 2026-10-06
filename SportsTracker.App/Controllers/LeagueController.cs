using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Models.LeagueNews;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    [Route("league")]
    public sealed class LeagueController(IScoreboardService scoreboardService, ILeagueViewModelMapper leagueViewModelMapper, ILeagueNewsService leagueNewsService, ILeagueNewsViewModelMapper leagueNewsViewModelMapper) : Controller
    {
        [HttpGet("{league}")]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            LeaguePageViewModel? viewModel = await GetLeaguePageViewModelAsync(league, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        [HttpGet("GameSections")]
        public async Task<IActionResult> GameSections(League league, CancellationToken cancellationToken)
        {
            LeaguePageViewModel? viewModel = await GetLeaguePageViewModelAsync(league, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Partials/_LeagueGameSections", viewModel);
        }

        [HttpGet("{league}/news")]
        public async Task<IActionResult> News(League league, CancellationToken cancellationToken)
        {
            LeagueNews? leagueNews = await leagueNewsService.GetNewsAsync(league, cancellationToken);

            if (leagueNews is null)
            {
                return NotFound();
            }
            
            LeagueNewsViewModel viewModel = leagueNewsViewModelMapper.Map(league, leagueNews);
            
            return View(viewModel);
        }

        [HttpGet("{league}/news/{articleId}")]
        public async Task<IActionResult> NewsArticle(League league, string articleId, CancellationToken cancellationToken)
        {
            LeagueNewsArticle? article = await leagueNewsService.GetArticleAsync(league, articleId, cancellationToken);

            if (article is null)
            {
                return NotFound();
            }

            LeagueNewsArticlePageViewModel viewModel = leagueNewsViewModelMapper.MapArticle(league, article);
            
            return View(viewModel);
        }

        private async Task<LeaguePageViewModel?> GetLeaguePageViewModelAsync(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return leagueViewModelMapper.Map(scoreboard);
        }
    }
}