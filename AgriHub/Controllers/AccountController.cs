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
 

        public AccountController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
         
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
          

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    
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
                     
                        ModelState.AddModelError(string.Empty, "Invalid employee login attempt.");
                        return View(model);
                    }
                }
                // For farmers, we don't need to check the email since they can only login with their assigned credentials

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                 

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
