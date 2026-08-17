using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.BoxScore;
using SportsTracker.App.Models.GameDetails;
using SportsTracker.App.Models.PlayByPlay;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.BoxScore;
using SportsTracker.App.ViewModels.GameDetails;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.PlayByPlay;

namespace SportsTracker.App.Controllers
{
    [Route("game")]
    public sealed class GameController(IGameDetailsService gameDetailsService, IGameContentService gameContentService, IGameDetailsViewModelMapper gameDetailsViewModelMapper, IBoxScoreViewModelMapper boxScoreViewModelMapper, IPlayByPlayViewModelMapper playByPlayViewModelMapper) : Controller
    {
        [HttpGet("{league}/{gameId}")]
        public async Task<IActionResult> Index(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetailsViewModel? viewModel = await GetGameDetailsAsync(league, gameId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        [HttpGet("content/{league}/{gameId}")]
        public async Task<IActionResult> Content(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetailsViewModel? viewModel = await GetGameDetailsAsync(league, gameId, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Partials/_GameDetailsContent", viewModel);
        }

        [HttpGet("{league}/{gameId}/boxscore")]
        public async Task<IActionResult> BoxScore(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePageViewModel<BoxScoreViewModel>? viewModel = await GetBoxScorePageAsync(league, gameId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        [HttpGet("boxscore/content/{league}/{gameId}")]
        public async Task<IActionResult> BoxScoreContent(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePageViewModel<BoxScoreViewModel>? viewModel = await GetBoxScorePageAsync(league, gameId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Partials/BoxScore/_BoxScorePageContent", viewModel);
        }

        [HttpGet("{league}/{gameId}/playbyplay")]
        public async Task<IActionResult> PlayByPlay(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePageViewModel<PlayByPlayViewModel>? viewModel = await GetPlayByPlayPageAsync(league, gameId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        [HttpGet("playbyplay/content/{league}/{gameId}")]
        public async Task<IActionResult> PlayByPlayContent(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePageViewModel<PlayByPlayViewModel>? viewModel = await GetPlayByPlayPageAsync(league, gameId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Partials/PlayByPlay/_PlayByPlayPageContent", viewModel);
        }

        private async Task<GameDetailsViewModel?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetails? details = await gameDetailsService.GetGameDetailsAsync(league, gameId, cancellationToken);
            
            return details is null ? null : gameDetailsViewModelMapper.Map(details);
        }

        private async Task<GamePageViewModel<BoxScoreViewModel>?> GetBoxScorePageAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            Task<GameBoxScore?> boxScoreTask = gameContentService.GetBoxScoreAsync(league, gameId, cancellationToken);
            Task<GameDetailsViewModel?> gameTask = GetGameDetailsAsync(league, gameId, cancellationToken);
            
            await Task.WhenAll(boxScoreTask, gameTask);
            
            GameBoxScore? boxScore = await boxScoreTask;
            GameDetailsViewModel? game = await gameTask;

            if (boxScore is null || game is null)
            {
                return null;
            }

            return new GamePageViewModel<BoxScoreViewModel>
            {
                Game = game,
                Content = boxScoreViewModelMapper.Map(boxScore)
            };
        }

        private async Task<GamePageViewModel<PlayByPlayViewModel>?> GetPlayByPlayPageAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            Task<GamePlayByPlay?> playByPlayTask = gameContentService.GetPlayByPlayAsync(league, gameId, cancellationToken);
            Task<GameDetailsViewModel?> gameTask = GetGameDetailsAsync(league, gameId, cancellationToken);
            
            await Task.WhenAll(playByPlayTask, gameTask);
            
            GamePlayByPlay? playByPlay = await playByPlayTask;
            GameDetailsViewModel? game = await gameTask;

            if (game is null)
            {
                return null;
            }
            
            PlayByPlayViewModel content = playByPlay is not null ? playByPlayViewModelMapper.Map(playByPlay) : new PlayByPlayViewModel
            {
                GameId = gameId,
                League = league,
                Plays = []
            };

            return new GamePageViewModel<PlayByPlayViewModel>
            {
                Game = game,
                Content = content
            };
        }
    }
}