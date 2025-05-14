using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models.Entities
{
    //---------------------------------------------------------------------------------------------------
    // Farmer model with attributes
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        public string UserId { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
//---------------------------------------------<<< End of File >>>------------------------------------------------------