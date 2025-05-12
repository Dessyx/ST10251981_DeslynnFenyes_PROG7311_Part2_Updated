using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgriHub.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        public FarmerController(IFarmerService farmerService)
        {
            _farmerService = farmerService;
        }

        public async Task<IActionResult> Index()
        {
            var farmers = await _farmerService.GetAllFarmersAsync();
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