using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepo;
        public DashboardController(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }
        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboardRepo.GetUserClubs();
            var userRaces = await _dashboardRepo.GetUserRaces();
            var dashboardVM = new DashboardViewModel
            {
                UserRaces = userRaces,
                UserClubs = userClubs
            };

            return View(dashboardVM);
        }
    }
}
