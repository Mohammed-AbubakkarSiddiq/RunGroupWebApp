using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        public ClubController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllAsync();
            //sends the data to the respective view.
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            Club club = await _clubRepository.GetByIdAsync(Id);
            return View(club);
        }

        /// <summary>
        /// Returns the view for creating a new entity.
        /// </summary>
        /// <returns>A view that displays the form for creating a new entity.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles HTTP POST requests to create a new club entity.
        /// </summary>
        /// <remarks>If the provided club model is invalid, the method returns the view with the submitted
        /// data and validation messages. On successful creation, the user is redirected to the list of clubs.</remarks>
        /// <param name="club">The club entity to add. Must contain valid data as defined by the model's validation attributes.</param>
        /// <returns>An IActionResult that redirects to the index view if the club is created successfully; otherwise, returns
        /// the view with validation errors.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }

            await _clubRepository.AddAsync(club);
            return RedirectToAction("Index");
        }
    }
}
