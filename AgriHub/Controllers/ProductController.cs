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
using Microsoft.EntityFrameworkCore;
using AgriHub.Data;
using Microsoft.Extensions.Logging;

namespace AgriHub.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFarmerService _farmerService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService, 
            UserManager<IdentityUser> userManager,
            IFarmerService farmerService,
            ApplicationDbContext context,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _userManager = userManager;
            _farmerService = farmerService;
            _context = context;
            _logger = logger;
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
                if (user == null)
                {
                    _logger.LogWarning("User not found when trying to add product");
                    return Unauthorized();
                }

                var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
                if (farmer == null)
                {
                    _logger.LogWarning("Farmer not found for user {UserId} when trying to add product", user.Id);
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    var product = new Product
                    {
                        Name = model.Name,
                        Category = model.Category,
                        ProductionDate = model.ProductionDate,
                        FarmerId = farmer.FarmerId
                    };

                    _logger.LogInformation("Adding product {ProductName} for farmer {FarmerId}", product.Name, farmer.FarmerId);
                    await _productService.AddProductAsync(product);
                    _logger.LogInformation("Successfully added product {ProductId} for farmer {FarmerId}", product.ProductId, farmer.FarmerId);
                    return RedirectToAction("MyProducts");
                }
                else
                {
                    _logger.LogWarning("Invalid model state when adding product");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning("Validation error: {Error}", error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product");
                ModelState.AddModelError(string.Empty, "An error occurred while adding the product. Please try again.");
            }
            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllProducts(string category, DateTime? from, DateTime? to, int? farmerId)
        {
            var products = await _productService.FilterProductsAsync(farmerId, category, from, to);
            return View(products);
        }

        public async Task<IActionResult> Filter(ProductFilterViewModel filter)
        {
            var query = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            if (filter.StartDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= filter.EndDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                query = query.Where(p => p.Category == filter.Category);
            }

            if (filter.FarmerId.HasValue)
            {
                query = query.Where(p => p.FarmerId == filter.FarmerId.Value);
            }

            // Get all farmers for the dropdown
            filter.Farmers = await _context.Farmers.ToListAsync();
            
            // Get unique categories from existing products
            filter.AvailableCategories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            
            filter.Products = await query.ToListAsync();
            return View(filter);
        }
    }
}