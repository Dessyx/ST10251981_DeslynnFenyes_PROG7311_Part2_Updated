using AgriHub.Models;
using AgriHub.Services;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;               // Imports
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AgriHub.Controllers
{
    //--------------------------------------------------------------------------------------------------
    // Controller that handles functionality behind products
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

        //-----------------------------------------------------------------------------------------------
        // Displays the products that are assigned to the specific farmer
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

        //---------------------------------------------------------------------------------------------------
        // Displays the add product form view to the farmer
        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View(new ProductViewModel());
        }

        //---------------------------------------------------------------------------------------------------
        // Adds the product to the database
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
                        ImageUrl = model.ImageUrl,
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

        // -----------------------------------------------------------------------------------------------
        // Edits the product infromation 
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
            
            if (product.FarmerId != farmer.FarmerId)
            {
                return Unauthorized();
            }

            var model = new ProductViewModel
            {
                Name = product.Name,
                Category = product.Category,
                ProductionDate = product.ProductionDate,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };

            return View(model);
        }

        // -----------------------------------------------------------------------------------------------
        // Edits the product infromation through ProductViewModel
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
                
                if (product.FarmerId != farmer.FarmerId)
                {
                    return Unauthorized();
                }

                product.Name = model.Name;
                product.Category = model.Category;
                product.ProductionDate = model.ProductionDate;
                product.Price = model.Price;
                product.ImageUrl = model.ImageUrl;

                await _productService.UpdateProductAsync(product);
                return RedirectToAction("MyProducts");
            }

            return View(model);
        }

        //---------------------------------------------------------------------------------------------------
        // Deletes the product from the database
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var farmer = await _farmerService.GetFarmerByUserIdAsync(user.Id);
            
            if (product.FarmerId != farmer.FarmerId)
            {
                return Unauthorized();
            }

            await _productService.DeleteProductAsync(id);
            return RedirectToAction("MyProducts");
        }

        //---------------------------------------------------------------------------------------------------
        // Passes through the product information
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllProducts(string category, DateTime? from, DateTime? to, int? farmerId, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productService.FilterProductsAsync(farmerId, category, from, to, minPrice, maxPrice);
            return View(products);
        }

        //---------------------------------------------------------------------------------------------------
        // Filters products 
        public async Task<IActionResult> Filter(ProductFilterViewModel filter)
        {
            var result = await _productService.GetFilteredProductsAsync(filter);
            return View(result);
        }
    }
}
// --------------------------------------<<< Enf of File >>>-------------------------------------------------