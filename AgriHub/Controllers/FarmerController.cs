using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AgriHub.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AgriHub.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FarmerController(
            IFarmerService farmerService, 
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _farmerService = farmerService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var farmers = await _farmerService.GetAllFarmersAsync();
            ViewBag.TotalFarmers = _context.Farmers.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalCategories = _context.Products.Select(p => p.Category).Distinct().Count();
            return View(farmers);
        }

        public IActionResult AddFarmer()
        {
            return View(new FarmerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(FarmerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (succeeded, errors) = await _farmerService.CreateFarmerAsync(model);
                
                if (succeeded)
                {
                    return RedirectToAction("Index");
                }
                
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);
        }
    }
}