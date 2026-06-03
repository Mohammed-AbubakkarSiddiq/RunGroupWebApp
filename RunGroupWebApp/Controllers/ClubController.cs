using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Extensions;
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
            var currentUserId = User.GetUserId();
            var createClubVM = new CreateClubViewModel
            {
                //This id will be included as a hidded input in the view.
                AppUserId = currentUserId
            };
            return View(createClubVM);
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
                    //Value taken from the hidden input in the view
                    AppUserId = createClubVM.AppUserId,
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

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null) return View("Error");

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                ClubCategory = club.ClubCategory
            };

            //For edit, we need to populate the view with the entity selected for edit. So, user can easily edit it.
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditClubViewModel editedClubVM, int id)
        {
            //Model validation
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error in editing the club");
                return View("Edit",editedClubVM);
            }

            //Checking if the id is valid and club exist
            var existingClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            var photoUploadResult = new ImageUploadResult();

            if (existingClub != null)
            {
                try
                {
                    //Deleting the previously uploaded photo with the URL (public Id got from the database respective record)
                    await _photoService.DeleteImageAsync(existingClub.Image);
                    //Uploading the new image (IFormFile)
                    photoUploadResult = await _photoService.UploadImageAsync(editedClubVM.Image);
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", "Error in editing the photo");
                    return View();
                }

                var editedClub = new Club
                {
                    Id = id,
                    Title = editedClubVM.Title,
                    Description = editedClubVM.Description,
                    Image = photoUploadResult.Url.ToString(),
                    AddressId = editedClubVM.AddressId,
                    Address = new Address
                    {
                        Id = (int)editedClubVM.AddressId,
                        Street = editedClubVM.Address.Street,
                        City = editedClubVM.Address.City,
                        State = editedClubVM.Address.State
                    },
                    ClubCategory = editedClubVM.ClubCategory
                };

                await _clubRepository.UpdateAsync(editedClub);

                return RedirectToAction("Index");
            }
            else
            {
                return View(editedClubVM);
            }
        }
    }
}
