using Microsoft.AspNetCore.Identity;
using SalonCS.IServices;
using System.Security.Cryptography;
using System.Text;

namespace SalonCS.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IConfiguration _configuration;
        public PasswordService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratePassword(string? password)
        {
            if(password == null) throw new Exception("Unsupported Innitial Password Found");

            using (var hmac = new HMACSHA512())
            {
                hmac.Key = GetHashingSalt();
                var computedHash = hmac.ComputeHash(Encoding.Default.GetBytes(password));

                return Convert.ToBase64String(computedHash);
            }
        }

        public bool VerifyPassword(string requestPassword, string actualPassword)
        {
            using (var hmac = new HMACSHA512(GetHashingSalt()))
            {
                var requestComputedHash = hmac.ComputeHash(Encoding.Default.GetBytes(requestPassword));
                var actualComputedHash = Convert.FromBase64String(actualPassword);

                return requestComputedHash.SequenceEqual(actualComputedHash);
            }
        }

        private byte[] GetHashingSalt() 
        {
            var hashingSalt = _configuration.GetSection("LoginSecret:HashingSalt").Value;
            if (hashingSalt == null) throw new Exception("Hashing salt couldn't found");

            return Convert.FromBase64String(hashingSalt);
        }
    }
}
