using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                var passwordValid = await _userManager.CheckPasswordAsync(user, loginVM.Password);

                if (passwordValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }

                //Not a recommendended way. Using as this is a small app. 
                //Recommended way: User a property for ths validation in its viewmodel.
                TempData["Error"] = "Invalid credentials. Please enter valid credentials";
                return View(loginVM);
            }

            TempData["Error"] = "Invalid credentials. Please enter valid credentials";
            return View(loginVM);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "User already exist!";
                return View(registerVM);
            }

            var newUser = new AppUser
            {
                Email = registerVM.EmailAddress,
                //Not recomended but it is a simple app. So, just keeping the email and user name same.
                UserName = registerVM.EmailAddress
            };

            var userCreationResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            //This endpoint is meant for user
            if (userCreationResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                //Not a good practice. But, this is a simple app. Good practice: Redirect to login page.
                return RedirectToAction("Index", "Race");
            }

            TempData["Error"] = "Server Error!";
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        }
    }
}
