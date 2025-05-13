using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AgriHub.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AgriHub.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<FarmerController> _logger;

        public FarmerController(
            IFarmerService farmerService, 
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<FarmerController> logger)
        {
            _farmerService = farmerService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
                _logger.LogInformation("Attempting to create farmer with email: {Email}", model.Email);
                
                var (succeeded, errors) = await _farmerService.CreateFarmerAsync(model);
                
                if (succeeded)
                {
                    _logger.LogInformation("Successfully created farmer with email: {Email}", model.Email);
                    return RedirectToAction("Index");
                }
                
                _logger.LogError("Failed to create farmer. Errors: {Errors}", string.Join(", ", errors));
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid for farmer creation");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation error: {Error}", error.ErrorMessage);
                }
            }
            return View(model);
        }
    }
}