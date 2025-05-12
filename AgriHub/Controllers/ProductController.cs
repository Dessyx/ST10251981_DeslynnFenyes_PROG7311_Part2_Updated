using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AgriEnergy_Hub.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductController(IProductService productService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.FarmerId == null) return Unauthorized();
            var products = await _productService.GetProductsByFarmerAsync(user.FarmerId.Value);
            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.FarmerId == null) return Unauthorized();
            if (ModelState.IsValid)
            {
                product.FarmerId = user.FarmerId.Value;
                await _productService.AddProductAsync(product);
                return RedirectToAction("MyProducts");
            }
            return View(product);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllProducts(string category, DateTime? from, DateTime? to, int? farmerId)
        {
            var products = await _productService.FilterProductsAsync(farmerId, category, from, to);
            return View(products);
        }
    }
}