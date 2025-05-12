using Microsoft.AspNetCore.Identity;
using AgriHub.Models;

namespace AgriHub.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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

            //----------------------------------------------------------------------------
            // Create default Farmer
            var farmerUser = new ApplicationUser
            {
                UserName = "farmer1@farm.com",   // use this username for email
                Email = "farmer1@farm.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != farmerUser.UserName))
            {
                var result = await userManager.CreateAsync(farmerUser, "Password123!");   // use this password
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser, "Farmer");
                }
            }
            //------------------------------------------------------------------------------
            // Create default Employee
            var employeeUser = new ApplicationUser
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
