using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Controllers
{
    [Route("athlete")]
    public sealed class AthleteController(
        IAthleteService athleteService,
        IAthleteDetailsViewModelMapper athleteDetailsViewModelMapper,
        IAthleteOverviewViewModelMapper athleteOverviewViewModelMapper,
        IAthleteStatsViewModelMapper athleteStatsViewModelMapper,
        IAthleteGameLogViewModelMapper gameLogViewModelMapper) : Controller
    {
        [HttpGet("{league}/{athleteId}")]
        public async Task<IActionResult> Index(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthletePageViewModel<AthleteOverviewViewModel>? viewModel = await GetOverviewPageAsync(league, athleteId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet("overview/content/{league}/{athleteId}")]
        public async Task<IActionResult> OverviewContent(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthletePageViewModel<AthleteOverviewViewModel>? viewModel = await GetOverviewPageAsync(league, athleteId, cancellationToken);

            if (viewModel is null)
            {
                return NotFound();
            }

            return PartialView("Partials/_AthleteOverviewPageContent", viewModel);
        }

        [HttpGet("{league}/{athleteId}/stats")]
        public async Task<IActionResult> Stats(League league, string athleteId, CancellationToken cancellationToken)
        {
            Task<AthleteDetails?> detailsTask = athleteService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            Task<AthleteStats?> statsTask = athleteService.GetAthleteStatsAsync(league, athleteId, cancellationToken);

            await Task.WhenAll(detailsTask, statsTask);

            AthleteDetails? details = await detailsTask;

            if (details is null)
            {
                return NotFound();
            }

            AthleteStats? stats = await statsTask;

            AthletePageViewModel<AthleteStatsViewModel> viewModel = new()
            {
                Athlete = athleteDetailsViewModelMapper.Map(details),

                Content = stats is null ? new AthleteStatsViewModel() : athleteStatsViewModelMapper.Map(stats)
            };

            return View(viewModel);
        }

        [HttpGet("{league}/{athleteId}/gamelog")]
        public async Task<IActionResult> GameLog(League league, string athleteId, CancellationToken cancellationToken)
        {
            Task<AthleteDetails?> detailsTask = athleteService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            Task<AthleteGameLog?> gameLogTask = athleteService.GetAthleteGameLogAsync(league, athleteId, cancellationToken);

            await Task.WhenAll(detailsTask, gameLogTask);

            AthleteDetails? details = await detailsTask;

            if (details is null)
            {
                return NotFound();
            }

            AthleteGameLog? gameLog = await gameLogTask;

            AthletePageViewModel<AthleteGameLogViewModel> viewModel = new()
            {
                Athlete = athleteDetailsViewModelMapper.Map(details),

                Content = gameLog is null
                    ? new AthleteGameLogViewModel
                    {
                        League = league
                    }
                    : gameLogViewModelMapper.Map(gameLog, league)
            };

            return View(viewModel);
        }

        [HttpGet("{league}/{athleteId}/splits")]
        public async Task<IActionResult> Splits(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetailsViewModel? athlete = await GetAthleteDetailsAsync(league, athleteId, cancellationToken);

            if (athlete is null)
            {
                return NotFound();
            }

            return View(athlete);
        }

        private async Task<AthleteDetailsViewModel?> GetAthleteDetailsAsync(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetails? details = await athleteService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);

            return details is null ? null : athleteDetailsViewModelMapper.Map(details);
        }

        private async Task<AthletePageViewModel<AthleteOverviewViewModel>?> GetOverviewPageAsync(League league, string athleteId, CancellationToken cancellationToken)
        {
            Task<AthleteDetails?> detailsTask = athleteService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            Task<AthleteOverview?> overviewTask = athleteService.GetAthleteOverviewAsync(league, athleteId, cancellationToken);

            await Task.WhenAll(detailsTask, overviewTask);

            AthleteDetails? details = await detailsTask;

            if (details is null)
            {
                return null;
            }

            AthleteOverview? overview = await overviewTask;

            return new AthletePageViewModel<AthleteOverviewViewModel>
            {
                Athlete = athleteDetailsViewModelMapper.Map(details),

                Content = overview is null ? new AthleteOverviewViewModel() : athleteOverviewViewModelMapper.Map(overview)
            };
        }
    }
}