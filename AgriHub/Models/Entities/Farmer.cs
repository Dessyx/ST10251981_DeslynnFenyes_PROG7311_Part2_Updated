namespace AgriHub.Models.Entities
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
