using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AgriHub.Data;
using System.Linq;

namespace AgriHub.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly ApplicationDbContext _context;
        public FarmerController(IFarmerService farmerService, ApplicationDbContext context)
        {
            _farmerService = farmerService;
            _context = context;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                await _farmerService.AddFarmerAsync(farmer);
                return RedirectToAction("Index");
            }
            return View(farmer);
        }
    }
}