using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.ViewModels;

namespace SportsTracker.Frontend.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            DashboardViewModel model = new()
            {
                Leagues =
                [
                    new LeagueSectionViewModel
                    {
                        LeagueName = "NFL",
                        Icon = "🏈",
                        Games =
                        [
                            new GameCardViewModel
                            {
                                AwayTeam = "Packers",
                                AwayScore = 3,
                                HomeTeam = "Bears",
                                HomeScore = 3,
                                Status = "3rd • 9:24",
                                IsLive = true
                            },
                            new GameCardViewModel
                            {
                                AwayTeam = "Chiefs",
                                AwayScore = 24,
                                HomeTeam = "Bills",
                                HomeScore = 21,
                                Status = "Final",
                                IsLive = false
                            }
                        ]
                    },
                    new LeagueSectionViewModel()
                    {
                        LeagueName = "MLB",
                        Icon = "⚾",
                        Games =
                        [
                            new GameCardViewModel
                            {
                                AwayTeam = "Cubs",
                                AwayScore = 6,
                                HomeTeam = "Cardinals",
                                HomeScore = 4,
                                Status = "Top 8",
                                IsLive = true
                            }
                        ]
                    }
                ]
            };

            return View(model);
        }
    }
}