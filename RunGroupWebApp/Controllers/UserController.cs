using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            List<AppUser> appUsers = await _userRepo.GetUserListAsync();
            List<GetAppUserViewModel> userVMList = new List<GetAppUserViewModel>();

            foreach (var appUser in appUsers)
            {
                GetAppUserViewModel userVM = new GetAppUserViewModel
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    Pace = appUser.Pace,
                    Mileage = appUser.Mileage,
                    Image = appUser.ImageURL
                };

                userVMList.Add(userVM);
            }

            return View(userVMList);
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var user = await _userRepo.GetByIdAsync(Id);
            GetAppUserViewModel userVM = new GetAppUserViewModel { 
            
                Id = user.Id,
                UserName = user.UserName, 
                Pace = user.Pace,
                Mileage =user.Mileage,
                Image = user.ImageURL
            };

            return View(userVM);
        }
    }
}
