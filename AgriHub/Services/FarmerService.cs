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
    public class FarmerService : IFarmerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<FarmerService> _logger;

        public FarmerService(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<FarmerService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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

        public async Task<Farmer> GetFarmerByUserIdAsync(string userId)
        {
            return await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<(bool succeeded, string[] errors)> CreateFarmerAsync(FarmerViewModel model)
        {
            try
            {
                _logger.LogInformation("Starting farmer creation process for email: {Email}", model.Email);

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("User with email {Email} already exists", model.Email);
                    return (false, new[] { "A user with this email already exists." });
                }

                // Create Identity user for the farmer
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                _logger.LogInformation("Creating Identity user for email: {Email}", model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Successfully created Identity user, adding to Farmer role");
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Create and add the farmer record
                    var farmer = new Farmer
                    {
                        UserId = user.Id,
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone
                    };

                    _logger.LogInformation("Adding farmer record to database");
                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Successfully created farmer with ID: {FarmerId}", farmer.FarmerId);
                    return (true, null);
                }

                _logger.LogError("Failed to create Identity user. Errors: {Errors}", 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating farmer");
                return (false, new[] { "An unexpected error occurred while creating the farmer." });
            }
        }
    }
}