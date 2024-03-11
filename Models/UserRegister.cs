using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, ErrorMessage = "Password must be between 6 and 60 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }
    }
}
