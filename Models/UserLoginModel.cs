using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, ErrorMessage = "Password must be between 6 and 60 characters.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
