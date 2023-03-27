using Microsoft.EntityFrameworkCore;
using SalonCS.Data;
using SalonCS.IServices;
using SalonCS.Model;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace SalonCS.Services
{
    public class UserService : IUserService
    {
        private DataContext _data;
        private IRSAService _rsaService;
        private IUtilityService _utilityService;

        public UserService(
            DataContext data,
            IRSAService rSAService, 
            IUtilityService utilityService)
        {
            _data = data;
            _rsaService = rSAService;
            _utilityService = utilityService;
        }
        public bool EditUser(int id, User user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _data.Users.ToListAsync();
        }
        public async Task<User> GetUser(int id)
        {
            return await _data.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async void InactiveUser(User user)
        {
            user.IsActive= false;
            await _data.SaveChangesAsync();
        }
        public async void RegisterUser(User newUser)
        {
            newUser.Username = _rsaService.Decrypt(newUser.Username);
            newUser.IsInnitialLogin = true;
            newUser.IsActive = true;
            newUser.Password = null;

            await _data.Users.AddAsync(newUser);
            _data.SaveChanges();
        }
        public async Task<bool> IsUsernameAlreadyExists(string username) 
        {
            var decryptedUsername = _rsaService.Decrypt(username);
            var isUsernameExists = await _data.Users.Where(user => decryptedUsername == user.Username).CountAsync();

            return isUsernameExists > 0;
        }

        public async Task<bool> ResetPassword(int id, string password)
        {
            var dbUser = await _data.Users.Where(x => x.Id == id).FirstAsync();

            if (dbUser == null) { return false; }

            using (var hmac = new HMACSHA512(_utilityService.GetHashingSalt()))
            {
                var requestComputedHash = hmac.ComputeHash(Encoding.Default.GetBytes(password));
                dbUser.Password = Convert.ToBase64String(requestComputedHash);
            }
            
            _data.SaveChanges();
            return true;
        }
    }
}
