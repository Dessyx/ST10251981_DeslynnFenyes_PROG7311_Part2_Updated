using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AgriHub.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFarmerService _farmerService;

        public ProductController(
            IProductService productService, 
            UserManager<IdentityUser> userManager,
            IFarmerService farmerService)
        {
            _productService = productService;
            _userManager = userManager;
            _farmerService = farmerService;
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
            if (farmer == null) return Unauthorized();

            var products = await _productService.GetProductsByFarmerAsync(farmer.FarmerId);
            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
                if (farmer == null) return Unauthorized();

                if (ModelState.IsValid)
                {
                    var product = new Product
                    {
                        Name = model.Name,
                        Category = model.Category,
                        ProductionDate = model.ProductionDate,
                        Price = model.Price,
                        FarmerId = farmer.FarmerId
                    };

                    await _productService.AddProductAsync(product);
                    return RedirectToAction("MyProducts");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the product. Please try again.");
            }
            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllProducts(string category, DateTime? from, DateTime? to, int? farmerId, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productService.FilterProductsAsync(farmerId, category, from, to, minPrice, maxPrice);
            return View(products);
        }

        public async Task<IActionResult> Filter(ProductFilterViewModel filter)
        {
            var result = await _productService.GetFilteredProductsAsync(filter);
            return View(result);
        }
    }
}