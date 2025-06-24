﻿using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models.Entities
{
    //---------------------------------------------------------------------------------------------------
    // Product model with attributes and checks
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int FarmerId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Production date is required")]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters")]
        public string? ImageUrl { get; set; }

        public Farmer Farmer { get; set; }
    }
}
//------------------------------------------<<< End of File >>>---------------------------------------------------------