using System.ComponentModel.DataAnnotations;

namespace AgriHub.Models
{
    //---------------------------------------------------------------------------------------------------
    // Passes the farmer data to the view
    public class FarmerViewModel
    {
        [Required(ErrorMessage = "Please enter the farmer's name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
//-----------------------------------------<<< End of File >>>----------------------------------------------------------