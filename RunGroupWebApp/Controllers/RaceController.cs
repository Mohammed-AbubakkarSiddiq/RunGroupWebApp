using CloudinaryDotNet.Actions;
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

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepo.GetByIdAsync(id);

            if (race == null) return View("Error");

            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                RaceCategory = race.RaceCategory
            };

            //For edit, we need to populate the view with the entity selected for edit. So, user can easily edit it.
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRaceViewModel editedRaceVM, int id)
        {
            //Model validation
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error in editing the race");
                return View("Edit", editedRaceVM);
            }

            //Checking if the id is valid and club exist
            var existingRace = await _raceRepo.GetByIdAsyncNoTracking(id);
            var photoUploadResult = new ImageUploadResult();

            if (existingRace != null)
            {
                try
                {
                    //Deleting the previously uploaded photo with the URL (public Id got from the database respective record)
                    await _photoService.DeleteImageAsync(existingRace.Image);
                    //Uploading the new image (IFormFile)
                    photoUploadResult = await _photoService.UploadImageAsync(editedRaceVM.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error in editing the photo");
                    return View();
                }

                var editedRace = new Race
                {
                    Id = id,
                    Title = editedRaceVM.Title,
                    Description = editedRaceVM.Description,
                    Image = photoUploadResult.Url.ToString(),
                    AddressId = (int)editedRaceVM.AddressId,
                    Address = new Address
                    {
                        Id = (int)editedRaceVM.AddressId,
                        Street = editedRaceVM.Address.Street,
                        City = editedRaceVM.Address.City,
                        State = editedRaceVM.Address.State
                    },
                    RaceCategory = editedRaceVM.RaceCategory
                };

                await _raceRepo.UpdateAsync(editedRace);

                return RedirectToAction("Index");
            }
            else
            {
                return View(editedRaceVM);
            }
        }
    }
}
