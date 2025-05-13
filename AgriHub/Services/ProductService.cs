using AgriHub.Data;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AgriHub.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            ApplicationDbContext context,
            ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId)
        {
            return await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(int? farmerId, string category, DateTime? from, DateTime? to)
        {
            var query = _context.Products.AsQueryable();
            if (farmerId.HasValue)
                query = query.Where(p => p.FarmerId == farmerId);
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);
            if (from.HasValue)
                query = query.Where(p => p.ProductionDate >= from);
            if (to.HasValue)
                query = query.Where(p => p.ProductionDate <= to);
            return await query.Include(p => p.Farmer).ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                _logger.LogInformation("Adding product {ProductName} for farmer {FarmerId}", product.Name, product.FarmerId);

                // Verify the farmer exists
                var farmer = await _context.Farmers.FindAsync(product.FarmerId);
                if (farmer == null)
                {
                    _logger.LogError("Farmer {FarmerId} not found when adding product", product.FarmerId);
                    throw new InvalidOperationException($"Farmer with ID {product.FarmerId} not found");
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully added product {ProductId} for farmer {FarmerId}", 
                    product.ProductId, product.FarmerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product {ProductName} for farmer {FarmerId}", 
                    product.Name, product.FarmerId);
                throw;
            }
        }
    }
}