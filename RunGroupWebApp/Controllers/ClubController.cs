using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
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
        /// <param name="createClubVM">The club entity to add. Must contain valid data as defined by the model's validation attributes.</param>
        /// <returns>An IActionResult that redirects to the index view if the club is created successfully; otherwise, returns
        /// the view with validation errors.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel createClubVM)
        {
            if (ModelState.IsValid)
            {
                //Upload image in cloud
                var result = await _photoService.UploadImageAsync(createClubVM.Image);

                var club = new Club
                {
                    Title = createClubVM.Title,
                    Description = createClubVM.Description,
                    //Once uploaded add the URL of the uploaded image is about to be added in the database.
                    Image = result.Url.ToString(),
                    ClubCategory = createClubVM.ClubCategory,
                    Address = new Address
                    {
                        Street = createClubVM.Address.Street,
                        City = createClubVM.Address.City,
                        State = createClubVM.Address.State
                    }
                };
                await _clubRepository.AddAsync(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error in image upload");
            }

            return View(createClubVM);
        }
    }
}
