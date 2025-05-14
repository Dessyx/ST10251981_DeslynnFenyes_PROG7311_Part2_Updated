using AgriHub.Models;
using AgriHub.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId);
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> FilterProductsAsync(int? farmerId, string category, DateTime? from, DateTime? to, decimal? minPrice = null, decimal? maxPrice = null);
        Task<ProductFilterViewModel> GetFilteredProductsAsync(ProductFilterViewModel filter);
    }
}