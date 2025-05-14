using AgriHub.Data;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace AgriHub.Services
{
    //---------------------------------------------------------------------------------------------------
    // Handles the core functionality
    public class FarmerService : IFarmerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FarmerService(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<FarmerService> logger)
        {
            _context = context;
            _userManager = userManager;
        }

        //---------------------------------------------------------------------------------------------------
        // Retrieves all farmers with propducts
        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            return await _context.Farmers.Include(f => f.Products).ToListAsync();
        }

        /*        //---------------------------------------------------------------------------------------------------
                // 
                public async Task<Farmer> GetFarmerByIdAsync(int id)
                {
                    return await _context.Farmers
                        .Include(f => f.Products)
                        .FirstOrDefaultAsync(f => f.FarmerId == id);
                }*/

        //---------------------------------------------------------------------------------------------------
        // Retrieves the farmers with their products by their id
        public async Task<Farmer> GetFarmerByUserIdAsync(string userId)
        {
            return await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.UserId == userId);
        }

        //---------------------------------------------------------------------------------------------------
        // Adds a farmer to the database
        public async Task<(bool succeeded, string[] errors)> CreateFarmerAsync(FarmerViewModel model)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return (false, new[] { "A user with this email already exists." });
                }

                // Create Identity user for the farmer
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Create and add the farmer record
                    var farmer = new Farmer
                    {
                        UserId = user.Id,
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone
                    };

                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    return (true, null);
                }

     
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }
            catch (Exception ex)
            {
                return (false, new[] { "An unexpected error occurred while creating the farmer." });
            }
        }
    }
}
//--------------------------------------------<<< End of File >>>-------------------------------------------------------