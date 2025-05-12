using System.ComponentModel.DataAnnotations;
using AgriHub.Models.Entities;

namespace AgriHub.Models
{
    public class ProductFilterViewModel
    {
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Product Category")]
        public string? Category { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
} 