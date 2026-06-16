using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroupWebApp.Helpers;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunGroupWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace RunGroupWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepo;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepo = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeVM = new HomeViewModel();

            try
            {
                //End point of the service along with the API key.
                var locationService = "https://ipinfo.io/?token=79c84fd780b8ff";
                var userLocationInfo = new WebClient().DownloadString(locationService);

                //Deserialize JSON into the object
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(userLocationInfo);

                // Create a RegionInfo object using the 2-letter country code (e.g. "IN")
                RegionInfo regionInfo = new RegionInfo(ipInfo.Country);

                // Get the full English name of that country (e.g. "India") and store it back
                ipInfo.Country = regionInfo.EnglishName;
                homeVM.City = ipInfo.City;
                homeVM.State = ipInfo.Region;
                if (homeVM.City != null)
                {
                    homeVM.Clubs = await _clubRepo.GetByCityAsync(homeVM.City);

                    if (homeVM.Clubs.Count() == 0)
                    {
                        homeVM.Clubs = null;
                    }
                }
                else
                {
                    homeVM.Clubs = null;
                }
                return View(homeVM);
            }
            catch (Exception)
            {
                homeVM.Clubs = null;
                return View(homeVM);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
