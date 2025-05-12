namespace AgriHub.Models.Entities
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string UserId { get; set; }  // This will store the IdentityUser.Id
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
