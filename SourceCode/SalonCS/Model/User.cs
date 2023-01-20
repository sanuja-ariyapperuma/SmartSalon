using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalonCS.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        [Required,MinLength(8)]
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsInnitialLogin { get; set; } = false;
        public UserRole UserRole { get; set; } = UserRole.DefaultUser;
    }
}
