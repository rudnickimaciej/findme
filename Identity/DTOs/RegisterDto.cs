using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,30}$", ErrorMessage = "Password must have at least one digit, one lowercase letter, and one uppercase letter. Minimum length is 8, maximum length is 30.")]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }

    }
}

