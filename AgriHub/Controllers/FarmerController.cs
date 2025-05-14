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
            var products = _context.Products.ToList();
            ViewBag.TotalFarmers = _context.Farmers.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalCategories = _context.Products.Select(p => p.Category).Distinct().Count();

            // Farmer trend: Farmer names and their product counts
            var farmerNames = _context.Farmers.Select(f => f.Name).ToList();
            var farmerProductCounts = _context.Farmers
                .Select(f => f.Products.Count)
                .ToList();

            // Product trend: Products added per month (last 6 months)
            var now = DateTime.Now;
            var months = Enumerable.Range(0, 6)
                .Select(i => new DateTime(now.Year, now.Month, 1).AddMonths(-i))
                .OrderBy(d => d)
                .ToList();
            var monthLabels = months.Select(m => m.ToString("MMM yyyy")).ToList();
            var productCounts = months.Select(m =>
                _context.Products.Count(p => p.ProductionDate.Month == m.Month && p.ProductionDate.Year == m.Year)
            ).ToList();

            // Category pie chart: Category names and product counts
            var categoryData = _context.Products
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();
            var categoryLabels = categoryData.Select(c => c.Category).ToList();
            var categoryCounts = categoryData.Select(c => c.Count).ToList();

            // Pass all products and a mapping of farmerId to their products
            ViewBag.AllProducts = products;
            ViewBag.FarmerIdToProducts = products.GroupBy(p => p.FarmerId)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.FarmerNames = farmerNames;
            ViewBag.FarmerProductCounts = farmerProductCounts;
            ViewBag.MonthLabels = monthLabels;
            ViewBag.ProductCounts = productCounts;
            ViewBag.CategoryLabels = categoryLabels;
            ViewBag.CategoryCounts = categoryCounts;

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