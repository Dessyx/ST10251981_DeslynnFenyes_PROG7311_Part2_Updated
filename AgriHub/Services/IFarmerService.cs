using AgriHub.Models;
using AgriHub.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    public interface IFarmerService
    {
        Task<IEnumerable<Farmer>> GetAllFarmersAsync();
        Task<Farmer> GetFarmerByUserIdAsync(string userId);
        Task AddFarmerAsync(Farmer farmer);
    }
}