using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonCS.IServices;
using SalonCS.Model;
using SalonCS.Services;

namespace SalonCS.Data
{
    public class DataContext : DbContext
    {
        private IPasswordService _passwordService;
        private IConfiguration _configuration;

        public DataContext(IPasswordService passwordService,IConfiguration configuration, DbContextOptions<DataContext> options) : base(options) {
            _passwordService = passwordService;
            _configuration = configuration;
        }

        //entities
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User user = new User
            {
                Id = 1,
                FirstName = "Administrator",
                LastName = "Local",
                Username = "administrator",
                Password = _passwordService.GeneratePassword(_configuration.GetSection("LoginSecret:InnitialSuperAdminPassword").Value),
                IsActive = true,
                UserRole = UserRole.SuperAdmin
            };
            modelBuilder.Entity<User>().HasData(user);
        }

    }
}
