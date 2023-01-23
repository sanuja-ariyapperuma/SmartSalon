using System.ComponentModel.DataAnnotations;

namespace SalonCS.DTO
{
    public class PasswordReset
    {
        [Required(ErrorMessage = "Username should not be empty")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password should not be empty")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Verify password should not be empty")]
        public string VerifyPassword { get; set; } = string.Empty;
    }
}
