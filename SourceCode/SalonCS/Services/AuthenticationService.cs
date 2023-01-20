using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SalonCS.Data;
using SalonCS.DTO;
using SalonCS.IServices;
using SalonCS.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SalonCS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public DataContext data { get; set; }

        private IPasswordService _passwordService;
        private IConfiguration _configuration;

        public AuthenticationService(DataContext dataContext,IPasswordService passwordService,IConfiguration configuration)
        {
            data = dataContext;
            _passwordService = passwordService;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(UserCredentials usercred)
        {
            var user = await data.Users.Where(user => user.Username == usercred.Username && user.IsActive).FirstOrDefaultAsync();

            if (user == null) return string.Empty;
            if (String.IsNullOrEmpty(user.Password)) throw new Exception("No Password Found");
            if (!_passwordService.VerifyPassword(usercred.Password, user.Password)) return string.Empty;
            
            return CreateToken(user);
        }

        public bool Register(UserCredentials usercred)
        {
            throw new NotImplementedException();
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name,String.Concat(user.FirstName," ",user.LastName)),
                new Claim(ClaimTypes.Role,user.UserRole.ToString()),
                new Claim(ClaimTypes.UserData,user.Id.ToString())
            };

            var tokenSecret = _configuration.GetSection("LoginSecret:Token").Value;

            if (tokenSecret == null) throw new Exception("Invalid token found");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSecret));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
