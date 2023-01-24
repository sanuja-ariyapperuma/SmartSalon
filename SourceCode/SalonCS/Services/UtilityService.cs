using SalonCS.IServices;

namespace SalonCS.Services
{
    public class UtilityService : IUtilityService
    {
        private IConfiguration _configuration;

        public UtilityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public byte[] GetHashingSalt()
        {
            var hashingSalt = _configuration.GetSection("LoginSecret:HashingSalt").Value;
            if (hashingSalt == null) throw new Exception("Hashing salt couldn't found");

            return Convert.FromBase64String(hashingSalt);
        }
    }
}
