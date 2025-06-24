using System.ComponentModel.DataAnnotations;
using AgriHub.Models.Entities;

namespace AgriHub.Models
{
    //---------------------------------------------------------------------------------------------------
    // Passes the needed data for filtering to the view
    public class ProductFilterViewModel
    {
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Product Category")]
        public string? Category { get; set; }

        [Display(Name = "Farmer")]
        public int? FarmerId { get; set; }

        [Display(Name = "Min Price")]
        public decimal? MinPrice { get; set; }

        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Sort By")]
        public string? SortBy { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Farmer> Farmers { get; set; } = new List<Farmer>();
        public IEnumerable<string> AvailableCategories { get; set; } = new List<string>();
    }
}
//------------------------------------------<<< End of File >>>---------------------------------------------------------