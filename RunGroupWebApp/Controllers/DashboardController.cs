using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Extensions;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepo;
        private readonly IPhotoService _photoService;
        public DashboardController(IDashboardRepository dashboardRepo, IPhotoService photoService)
        {
            _dashboardRepo = dashboardRepo;
            _photoService = photoService;
        }

        //Manual mapper
        public void MapUserEdit(ImageUploadResult imageUploadResult, EditUserViewModel editUserVM, AppUser appUser)
        {
            appUser.Pace = editUserVM.Pace;
            appUser.Mileage = editUserVM.Mileage;
            appUser.ImageURL = imageUploadResult.Url.ToString();

            if (appUser.Address == null)
            {
                // No existing address → create new one
                appUser.Address = new Address
                {
                    Street = editUserVM.Address.Street,
                    City = editUserVM.Address.City,
                    State = editUserVM.Address.State
                };
            }
            else
            {
                // Existing address → just update fields
                appUser.Address.Street = editUserVM.Address.Street;
                appUser.Address.City = editUserVM.Address.City;
                appUser.Address.State = editUserVM.Address.State;
            }
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

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = User.GetUserId();
            var appUser = await _dashboardRepo.GetUserByIdAsync(currentUserId);
            var editUserVM = new EditUserViewModel
            {
                UserId = currentUserId,
                Pace = appUser.Pace,
                Mileage = appUser.Mileage,
                AddressId = appUser.AddressId,
                Address = appUser.Address
            };
            return View(editUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel editUserVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error in the model state");
                return View(editUserVM);
            }

            var currentUserId = User.GetUserId();

            var appUser = await _dashboardRepo.GetUserByIdAsync(currentUserId);

            if (appUser == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(editUserVM);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(appUser.ImageURL))
                    {
                        await _photoService.DeleteImageAsync(appUser.ImageURL);
                    }
                    var imageUploadResult = await _photoService.UploadImageAsync(editUserVM.Image);

                    MapUserEdit(imageUploadResult, editUserVM, appUser);
                    await _dashboardRepo.UpdateUserAsync(appUser);

                    return RedirectToAction("Index");
                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(editUserVM);
                }
            }
    }
}}
