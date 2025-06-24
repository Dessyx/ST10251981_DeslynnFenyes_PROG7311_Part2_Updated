﻿using AgriHub.Data;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    //---------------------------------------------------------------------------------------------------
    // Handles product functionality
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------------------------------------------------------------------
        // Fetches products including their assigned farmer 
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();
        }

        //---------------------------------------------------------------------------------------------------
        // Fetches the products using the farmers id
        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId)
        {
            return await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }

        //---------------------------------------------------------------------------------------------------
        // Fetches the products by the product id
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        //---------------------------------------------------------------------------------------------------
        // Adds the product to the database 
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        //---------------------------------------------------------------------------------------------------
        // Updates product changes 
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        //---------------------------------------------------------------------------------------------------
        // Deletes the product from the database
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        //---------------------------------------------------------------------------------------------------
        // Filter commands for product filtering 
        public async Task<IEnumerable<Product>> FilterProductsAsync(int? farmerId, string category, DateTime? from, DateTime? to, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var query = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            if (farmerId.HasValue)
            {
                query = query.Where(p => p.FarmerId == farmerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (from.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= to.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return await query.ToListAsync();
        }

        //---------------------------------------------------------------------------------------------------
        // Filter commands for product filtering 
        public async Task<ProductFilterViewModel> GetFilteredProductsAsync(ProductFilterViewModel filter)
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

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

        
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "price_low_to_high":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "price_high_to_low":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    case "name_a_to_z":
                        query = query.OrderBy(p => p.Name);
                        break;
                    case "name_z_to_a":
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case "popularity":
                       
                        query = query.OrderByDescending(p => p.ProductionDate);
                        break;
                    default:
                       
                        query = query.OrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                // Default 
                query = query.OrderBy(p => p.Name);
            }

            // Get all farmers for the dropdown
            filter.Farmers = await _context.Farmers.ToListAsync();
            
            // Get categories from existing products
            filter.AvailableCategories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            
            filter.Products = await query.ToListAsync();
            return filter;
        }
    }
}
//----------------------------------------------<<< End of File >>>-----------------------------------------------------