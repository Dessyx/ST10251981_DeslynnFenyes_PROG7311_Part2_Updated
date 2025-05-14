using AgriHub.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using AgriHub.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AgriHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Farmer> Farmers { get; set; } // Adds Farmers table
        public DbSet<Product> Products { get; set; }  // Adds Products table

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
