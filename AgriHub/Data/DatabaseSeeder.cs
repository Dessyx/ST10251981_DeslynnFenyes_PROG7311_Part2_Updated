using Microsoft.AspNetCore.Identity;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriHub.Data
{
    //---------------------------------------------------------------------------------------------------
    // Supplies the database with needed data
    public class DatabaseSeeder
    {
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();  // Initialise services
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Define roles
            string[] roles = { "Farmer", "Employee" };

            // Create roles if not already present
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create default Employee
            var employeeUser = new IdentityUser
            {
                UserName = "employee1@agri.com",  // use this username for email
                Email = "employee1@agri.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != employeeUser.UserName))
            {
                var result = await userManager.CreateAsync(employeeUser, "Password123!");   // use this password
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }

            // Seeded farmers and products for dummy data
            if (!context.Farmers.Any())
            {
                var farmers = new List<Farmer>
                {
                    new Farmer { Name = "Xavier Martin", Email = "xavier@farm.com", Phone = "123-456-7890", UserId = "xav1" },
                    new Farmer { Name = "Bob Marley", Email = "bobby@farm.com", Phone = "234-567-8901", UserId = "bob1" },
                    new Farmer { Name = "Christiano Ronaldo", Email = "christiano@farm.com", Phone = "345-678-9012", UserId = "chris1" },
                    new Farmer { Name = "Rodrygo Goes", Email = "rodry@farm.com", Phone = "456-789-0123", UserId = "rod1" }
                };
                context.Farmers.AddRange(farmers);
                context.SaveChanges();

                var products = new List<Product>
                {
                    new Product { Name = "Organic Apples", Category = "Fruit", ProductionDate = DateTime.Now.AddMonths(-2), Price = 28.50m, FarmerId = farmers[0].FarmerId },
                    new Product { Name = "Fresh Milk", Category = "Dairy", ProductionDate = DateTime.Now.AddMonths(-1), Price = 39.00m, FarmerId = farmers[0].FarmerId },
                    new Product { Name = "Free-range Eggs", Category = "Poultry", ProductionDate = DateTime.Now.AddMonths(-1), Price = 40.00m, FarmerId = farmers[1].FarmerId },
                    new Product { Name = "Sweet Corn", Category = "Vegetable", ProductionDate = DateTime.Now.AddMonths(-3), Price = 28.00m, FarmerId = farmers[1].FarmerId },
                    new Product { Name = "Raw Honey", Category = "Honey", ProductionDate = DateTime.Now.AddMonths(-4), Price = 32.00m, FarmerId = farmers[2].FarmerId },
                    new Product { Name = "Goat Cheese", Category = "Dairy", ProductionDate = DateTime.Now.AddMonths(-2), Price = 39.00m, FarmerId = farmers[3].FarmerId },
                    new Product { Name = "Sunflower Seeds", Category = "Seeds", ProductionDate = DateTime.Now.AddMonths(-5), Price = 15.00m, FarmerId = farmers[3].FarmerId },
                    new Product { Name = "Pumpkin", Category = "Vegetable", ProductionDate = DateTime.Now.AddMonths(-1), Price = 23.00m, FarmerId = farmers[3].FarmerId }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
//-------------------------------------------------------<<< End Of File >>>----------------------------------------------------