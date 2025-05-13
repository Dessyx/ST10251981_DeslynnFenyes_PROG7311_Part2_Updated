using Microsoft.AspNetCore.Mvc;
using AgriHub.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AgriHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Login attempt for user: {Email}", model.Email);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: User not found for email: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                // Check if it's an employee login attempt
                if (role == "Employee")
                {
                    // Verify if this is the predefined employee account
                    if (model.Email != "employee1@agri.com")
                    {
                        _logger.LogWarning("Login failed: Invalid employee email: {Email}", model.Email);
                        ModelState.AddModelError(string.Empty, "Invalid employee login attempt.");
                        return View(model);
                    }
                }
                // For farmers, we don't need to check the email since they can only login with their assigned credentials

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Successful login for user: {Email} with role: {Role}", model.Email, role);

                    if (role == "Employee")
                    {
                        return RedirectToAction("Index", "Farmer");
                    }
                    else if (role == "Farmer")
                    {
                        return RedirectToAction("MyProducts", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    _logger.LogWarning("Login failed: Invalid password for user: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
