using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models
{
    //---------------------------------------------------------------------------------------------------
    // Passes the product data to the view 
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Please enter a product name")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please select a production date")]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "Please enter a price")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters")]
        public string? ImageUrl { get; set; }
    }
}
//----------------------------------------------<<< End of File >>>-----------------------------------------------------