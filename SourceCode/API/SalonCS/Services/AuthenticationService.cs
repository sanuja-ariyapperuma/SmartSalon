﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SalonCS.Data;
using SalonCS.DTO;
using SalonCS.IServices;
using SalonCS.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalonCS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public DataContext data { get; set; }

        private IPasswordService _passwordService;
        private IConfiguration _configuration;
        private IRSAService _rsaService;

        public AuthenticationService(
            DataContext dataContext,
            IPasswordService passwordService,
            IConfiguration configuration,
            IRSAService rSAService
            )
        {
            data = dataContext;
            _passwordService = passwordService;
            _configuration = configuration;
            _rsaService = rSAService;
        }
        public async Task<string> Authenticate(UserCredentials usercred)
        {
            //Decrypt using RSA
            //usercred.Username = _rsaService.Decrypt(usercred.Username);
            //usercred.Password = _rsaService.Decrypt(usercred.Password);

            //Get user
            var user = await data.Users.Where(user => user.Username == usercred.Username && user.IsActive).FirstOrDefaultAsync();

            //No user found
            if (user == null) return string.Empty;
            
            //Create token for fresh user (who doesnt have a password yet)
            if (String.IsNullOrEmpty(user.Password) && user.IsInnitialLogin) return CreateToken(user,true);

            //Validate username and password
            if (!_passwordService.MatchPassword(usercred.Password, user.Password)) return string.Empty;
            

            return CreateToken(user);
        }
        public bool Register(UserCredentials usercred)
        {
            throw new NotImplementedException();
        }
        private string CreateToken(User user,bool isInnitialLogin = false)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name,String.Concat(user.FirstName," ",user.LastName)),
                new Claim(ClaimTypes.Role,user.UserRole.ToString()),
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.UserData,user.Username.ToString())
            };


            var tokenSecret = _configuration.GetSection("LoginSecret:Token").Value;

            if (tokenSecret == null) throw new Exception("Invalid token found");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSecret));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: isInnitialLogin ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
    }
}