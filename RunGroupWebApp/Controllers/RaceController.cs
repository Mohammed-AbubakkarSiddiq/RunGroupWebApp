using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepo;
        private readonly IPhotoService _photoService;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepo = raceRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepo.GetAllAsync();
            return View(races);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            Race race = await _raceRepo.GetByIdAsync(Id);
            return View(race);
        }

        /// <summary>
        /// Returns the view for creating a new entity.
        /// </summary>
        /// <returns>A view that displays the form for creating a new entity.</returns>
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel createRaceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.UploadImageAsync(createRaceVM.Image);

                var race = new Race
                {
                    Title = createRaceVM.Title,
                    Description = createRaceVM.Description,
                    Image = result.Url.ToString(),
                    RaceCategory = createRaceVM.RaceCategory,
                    Address = new Address
                    {
                        Street = createRaceVM.Address.Street,
                        City = createRaceVM.Address.City,
                        State = createRaceVM.Address.State
                    }
                };

                await _raceRepo.AddAsync(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error in adding image");
            }

            return View(createRaceVM);
        }
    }
}
