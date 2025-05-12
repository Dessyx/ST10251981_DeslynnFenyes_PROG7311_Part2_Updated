using AgriHub.Data;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriHub.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly ApplicationDbContext _context;
        public FarmerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            return await _context.Farmers.Include(f => f.Products).ToListAsync();
        }

        public async Task<Farmer> GetFarmerByIdAsync(int id)
        {
            return await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.FarmerId == id); 
        }


        public async Task AddFarmerAsync(Farmer farmer)
        {
            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
        }
    }
}