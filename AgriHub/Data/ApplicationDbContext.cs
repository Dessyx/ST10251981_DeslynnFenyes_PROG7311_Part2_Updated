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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Product>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.ProductId);


                entity.HasOne(e => e.Farmer)
                      .WithMany(f => f.Products)
                      .HasForeignKey(e => e.FarmerId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasIndex(e => e.FarmerId); 
                entity.HasIndex(e => e.Category); 
                entity.HasIndex(e => e.ProductionDate); 
                entity.HasIndex(e => e.Price); 
                entity.HasIndex(e => e.Name); 
                entity.HasIndex(e => new { e.Category, e.Price }); 
                entity.HasIndex(e => new { e.FarmerId, e.ProductionDate }); 

                // Constraints
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });


            modelBuilder.Entity<Farmer>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.FarmerId);

                // Indexes for better performance
                entity.HasIndex(e => e.UserId); 
                entity.HasIndex(e => e.Name); 
                entity.HasIndex(e => e.Email); 

                // Constraints
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });
        }
    }
}
