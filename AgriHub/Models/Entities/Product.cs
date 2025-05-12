using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int FarmerId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }
        public Farmer Farmer { get; set; }
    }
}
