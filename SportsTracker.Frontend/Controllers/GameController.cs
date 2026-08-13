using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.BoxScore;
using SportsTracker.Frontend.ViewModels.GameDetails;
using SportsTracker.Frontend.ViewModels.PlayByPlay;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.GameDetails;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Controllers
{
    [Route("game")]
    public sealed class GameController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly IGameDetailsMapper _mapper;
        private readonly IBoxScoreMapper _boxScoreMapper;
        private readonly IPlayByPlayMapper _playByPlayMapper;
        
        public GameController(ISportsApiClient api, IGameDetailsMapper mapper, IBoxScoreMapper boxScoreMapper, IPlayByPlayMapper playByPlayMapper)
        {
            _api = api;
            _mapper = mapper;
            _boxScoreMapper = boxScoreMapper;
            _playByPlayMapper = playByPlayMapper;
        }

        [HttpGet("{league}/{gameId}")]
        public async Task<IActionResult> Index(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameDetails>? response = await _api.GetGameDetailsAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel viewModel = _mapper.Map(response.Data);
            
            return View(viewModel);
        }

        [HttpGet("content/{league}/{gameId}")]
        public async Task<IActionResult> Content(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetailsViewModel? viewModel = await GetGameDetailsViewModelAsync(league, gameId, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Game/_GameDetailsContent", viewModel);
        }

        [HttpGet("{league}/{gameId}/boxscore")]
        public async Task<IActionResult> BoxScore(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameBoxScore>? response = await _api.GetBoxScoreAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel? game = await GetGameDetailsViewModelAsync(league, gameId, cancellationToken);

            if (game is null)
            {
                return NotFound();
            }

            BoxScorePageViewModel viewModel = new()
            {
                Game = game,
                BoxScore = _boxScoreMapper.Map(response.Data)
            };
            
            return View(viewModel);
        }

        [HttpGet("boxscore/content/{league}/{gameId}")]
        public async Task<IActionResult> BoxScoreContent(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameBoxScore>? response = await _api.GetBoxScoreAsync(league, gameId, cancellationToken);
            
            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel? game = await GetGameDetailsViewModelAsync(league, gameId, cancellationToken);
            
            if (game is null)
            {
                return NotFound();
            }
            
            BoxScorePageViewModel viewModel = new()
            {
                Game = game,
                BoxScore = _boxScoreMapper.Map(response.Data)
            };
            
            return PartialView("Game/_BoxScorePageContent", viewModel);
        }

        [HttpGet("{league}/{gameId}/playbyplay")]
        public async Task<IActionResult> PlayByPlay(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GamePlayByPlay>? response = await _api.GetPlayByPlayAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel game = await GetGameDetailsViewModelAsync(league, gameId, cancellationToken) ?? new GameDetailsViewModel();

            if (game is null)
            {
                return NotFound();
            }
            
            PlayByPlayPageViewModel viewModel = new()
            {
                Game = game,
                PlayByPlay = _playByPlayMapper.Map(response.Data)
            };
            
            return View(viewModel);
        }

        [HttpGet("playbyplay/content/{league}/{gameId}")]
        public async Task<IActionResult> PlayByPlayContent(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GamePlayByPlay>? response = await _api.GetPlayByPlayAsync(league, gameId, cancellationToken);
            
            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel? game = await GetGameDetailsViewModelAsync(league, gameId, cancellationToken);

            if (game is null)
            {
                return NotFound();
            }

            PlayByPlayPageViewModel viewModel = new()
            {
                Game = game,
                PlayByPlay = _playByPlayMapper.Map(response.Data)
            };
            
            return PartialView("Game/_PlayByPlayPageContent", viewModel);
        }

        private async Task<GameDetailsViewModel?> GetGameDetailsViewModelAsync(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameDetails>? response = await _api.GetGameDetailsAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return null;
            }
            
            return _mapper.Map(response.Data);
        }
    }
}