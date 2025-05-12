using AgriHub.Models;
using AgriHub.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId);
        Task<IEnumerable<Product>> FilterProductsAsync(int? farmerId, string category, DateTime? from, DateTime? to);
        Task AddProductAsync(Product product);
    }
}