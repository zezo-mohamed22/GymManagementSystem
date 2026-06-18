using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementSystem.Controllers;
using GymManagementSystemDAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementSystemPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly SignInManager<ApplicationUser> _SignInManager;

        public AccountController(UserManager<ApplicationUser> userManger,
                                SignInManager<ApplicationUser> signInManager,
                                ILogger<ApplicationUser> logger)
        {
            _userManger = userManger;
            _logger = logger;
            _SignInManager = signInManager;
        }
        public IActionResult Index() => View();
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManger.FindByEmailAsync(model.Email);
            if(user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);
            }
            var result = await _SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Id} signed in");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (result.IsLockedOut)
            {
                _logger.LogInformation($"User {user.Id} is locked out");
                ModelState.AddModelError("InvalidLogin", "This Account is temporarily locked, try again later");
            }
            else if (result.IsNotAllowed) {
                ModelState.AddModelError("InvalidLogin", "Sign in is not allowed for this account");
            }
            else
            {
               ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
           await _SignInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

    }
}
