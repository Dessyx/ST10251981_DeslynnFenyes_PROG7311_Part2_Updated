using Microsoft.AspNetCore.Mvc;
using AgriHub.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;            // Imports
using Microsoft.Extensions.Logging;

namespace AgriHub.Controllers
{
    // -------------------------------------------------------------------------------------------------------
    // Controller which handles the Login and Page navigation for users
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(  // Passes the authentication managers through the constructor
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
         
        }

        // ---------------------------------------------------------------------------------------------------
        // Displays the Login page
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View();
        }

        //-----------------------------------------------------------------------------------------------------
        // Post method to authenticate users and direct them to their respective pages
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

                // Checks if it's an employee attempting to log in
                if (role == "Employee")
                {
                    // Verifies if the entered email matches the employee email
                    if (model.Email != "employee1@agri.com")
                    {                    
                        ModelState.AddModelError(string.Empty, "Invalid employee login attempt.");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Checks the roles
                    if (role == "Employee")
                    {
                        return RedirectToAction("Index", "Farmer"); // Employees are directed to add farmers, view producst and filters
                    }
                    else if (role == "Farmer")
                    {
                        return RedirectToAction("MyProducts", "Product"); // Farmers are directed to view and add products
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

        //------------------------------------------------------------------------------------------------------------
        // Logs the user out of their account
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
// -----------------------------------------------------<<< End Of File >>>---------------------------------------------