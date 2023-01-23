using SalonCS.CustomValidations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalonCS.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required"),
            MinLength(3,ErrorMessage ="First name should be more than 3 characters")]
        public string FirstName { get; set; } = string.Empty;
        [MinLength(3, ErrorMessage = "Last name should be more than 3 characters")]
        public string? LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Username is required"), 
            MinLength(5, ErrorMessage = "Username should be more than 5 characters")]
        public string Username { get; set; } = string.Empty;
        [CustomValidationPassword]
        public string? Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsInnitialLogin { get; set; } = false;
        public UserRole UserRole { get; set; } = UserRole.DefaultUser;
    }
}
