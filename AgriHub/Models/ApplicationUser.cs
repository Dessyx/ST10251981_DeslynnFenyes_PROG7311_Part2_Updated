namespace AgriHub.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } // "Farmer" or "Employee"
        public int? FarmerId { get; set; } // Only for Farmer users
    }
}
