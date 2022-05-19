using DiskShare.Data;
using DiskShare.Models.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiskShare.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;
        private readonly AppIdentityDbContext _idectx;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            ILogger<AppUser> logger, AppIdentityDbContext idectx)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _idectx = idectx;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            TempData["returnUrl"] = returnUrl;   //returnUrl to post method
            if (_signInManager.IsSignedIn(User))
            {
                TempData["Fail"] = "Jesteś już zalogowany.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
                if (res.Succeeded)
                {
                    TempData["Success"] = "Zostałeś zalogowany.";
                    if (!string.IsNullOrEmpty(TempData["returnUrl"] as string))
                    {
                        #pragma warning disable CS8604 // Możliwy argument odwołania o wartości null.
                        return Redirect(TempData["returnUrl"] as string);
                        #pragma warning restore CS8604 // Możliwy argument odwołania o wartości null.
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Fail"] = "Niepoprawne dane logowania.";
            return View(login);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                TempData["Fail"] = "Jesteś już zarejestrowany.";
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    SurName = model.SurName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account.");
                    TempData["Success"] = "Konto zostało stworzone!";
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }

        public IActionResult Logout(string? returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                _signInManager.SignOutAsync();
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
