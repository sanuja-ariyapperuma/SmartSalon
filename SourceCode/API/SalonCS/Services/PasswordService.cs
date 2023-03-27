using Microsoft.AspNetCore.Identity;
using SalonCS.IServices;
using System.Security.Cryptography;
using System.Text;

namespace SalonCS.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IConfiguration _configuration;
        private readonly IRSAService _rsaService;
        private readonly IUtilityService _utilityService;

        public PasswordService(
            IConfiguration configuration,
            IRSAService rSAService,
            IUtilityService utilityService)
        {
            _configuration = configuration;
            _rsaService = rSAService;
            _utilityService = utilityService;
        }

        public string GeneratePassword(string? password)
        {
            if(password == null) throw new Exception("Unsupported Innitial Password Found");

            using (var hmac = new HMACSHA512())
            {
                hmac.Key = _utilityService.GetHashingSalt();
                var computedHash = hmac.ComputeHash(Encoding.Default.GetBytes(password));

                return Convert.ToBase64String(computedHash);
            }
        }

        public bool MatchPassword(string requestPassword, string actualPassword)
        {
            //requestPassword = _rsaService.Decrypt(requestPassword);
            //actualPassword = _rsaService.Decrypt(actualPassword);

            using (var hmac = new HMACSHA512(_utilityService.GetHashingSalt()))
            {
                var requestComputedHash = hmac.ComputeHash(Encoding.Default.GetBytes(requestPassword));
                var actualComputedHash = Convert.FromBase64String(actualPassword);

                return requestComputedHash.SequenceEqual(actualComputedHash);
            }
        }

        public bool VerifyPassword(string requestPassword, string actualPassword)
        {
            return _rsaService.Decrypt(requestPassword).Equals(_rsaService.Decrypt(actualPassword));
        }

        
    }
}
