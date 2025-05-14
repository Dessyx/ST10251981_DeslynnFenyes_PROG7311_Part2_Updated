using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models
{
    //---------------------------------------------------------------------------------------------------
    // Obtains the attributes for login
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }  // User email

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }  // User password

        public bool RememberMe { get; set; } // Remeber the user
    }
}
//----------------------------------------<<< End of File >>>-----------------------------------------------------------