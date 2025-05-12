using AgriHub.Data;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId)
        {
            return await _context.Products.Where(p => p.FarmerId == farmerId).ToListAsync();
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
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}