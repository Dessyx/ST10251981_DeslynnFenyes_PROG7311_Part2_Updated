using Microsoft.AspNetCore.Identity;
using AgriHub.Models;
using AgriHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriHub.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
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
        }
    }
}
