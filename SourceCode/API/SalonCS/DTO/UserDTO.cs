using SalonCS.Model;
using System.ComponentModel.DataAnnotations;

namespace SalonCS.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.DefaultUser;
    }
}
