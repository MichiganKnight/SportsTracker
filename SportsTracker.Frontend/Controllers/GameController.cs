using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.BoxScore;
using SportsTracker.Frontend.ViewModels.GameDetails;
using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.PlayByPlay;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.GameDetails;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Controllers
{
    [Route("game")]
    public sealed class GameController(IGameApiClient api, IGameDetailsMapper gameDetailsMapper, IBoxScoreMapper boxScoreMapper, IPlayByPlayMapper playByPlayMapper) : Controller
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
            
            return PartialView("Game/_GameDetailsContent", viewModel);
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
            
            return PartialView("Game/_BoxScorePageContent", viewModel);
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
            
            return PartialView("Game/_PlayByPlayPageContent", viewModel);
        }

        private async Task<GameDetailsViewModel?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameDetails>? response = await api.GetGameDetailsAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return null;
            }
            
            return gameDetailsMapper.Map(response.Data);
        }

        private async Task<GamePageViewModel<BoxScoreViewModel>?> GetBoxScorePageAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            Task<ApiResponse<GameBoxScore>?> boxScoreTask = api.GetBoxScoreAsync(league, gameId, cancellationToken);
            Task<GameDetailsViewModel?> gameTask = GetGameDetailsAsync(league, gameId, cancellationToken);
            
            await Task.WhenAll(boxScoreTask, gameTask);
            
            ApiResponse<GameBoxScore>? boxScoreResponse = await boxScoreTask;
            GameDetailsViewModel? game = await gameTask;

            if (boxScoreResponse?.Data is null || game is null)
            {
                return null;
            }

            return new GamePageViewModel<BoxScoreViewModel>
            {
                Game = game,
                Content = boxScoreMapper.Map(boxScoreResponse.Data)
            };
        }

        private async Task<GamePageViewModel<PlayByPlayViewModel>?> GetPlayByPlayPageAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            Task<ApiResponse<GamePlayByPlay>?> playByPlayTask = api.GetPlayByPlayAsync(league, gameId, cancellationToken);
            Task<GameDetailsViewModel?> gameTask = GetGameDetailsAsync(league, gameId, cancellationToken);
            
            await Task.WhenAll(playByPlayTask, gameTask);
            
            ApiResponse<GamePlayByPlay>? playByPlayResponse = await playByPlayTask;
            GameDetailsViewModel? game = await gameTask;

            if (game is null)
            {
                return null;
            }
            
            PlayByPlayViewModel playByPlay = playByPlayResponse?.Data is not null ? playByPlayMapper.Map(playByPlayResponse.Data) : new PlayByPlayViewModel
            {
                GameId = gameId,
                League = league,
                Plays = []
            };

            return new GamePageViewModel<PlayByPlayViewModel>
            {
                Game = game,
                Content = playByPlay
            };
        }
    }
}