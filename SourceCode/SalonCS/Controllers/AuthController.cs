using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonCS.DTO;
using SalonCS.IServices;
using SalonCS.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SalonCS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authservice { get; set; }

        private IPasswordService _passwordService;
        private IUserService _userService;
        private IAsymmetricEncryption _asymmetricEncryption;

        public AuthController(
            IAuthenticationService authservice, 
            IPasswordService passwordService,
            IUserService userService,
            IAsymmetricEncryption asymmetricEncryption
            )
        {
            _authservice = authservice;
            _passwordService = passwordService;
            _userService = userService;
            _asymmetricEncryption = asymmetricEncryption;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] UserCredentials usercredentials)
        {

            var response = new ServiceResponse<string>();

            var jwt = await _authservice.Authenticate(usercredentials);

            if (String.IsNullOrEmpty(jwt))
            {
                response = new ServiceResponse<string> { Data = null, Message = "Username or password error", Success = false };
                return Ok(response);
            }

            response.Success = true;
            response.Data = jwt;

            return Ok(response);
        }

        [HttpPost("resetpassword")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword([FromBody] PasswordReset passwordreset)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string jwtUsername = identity.FindFirst("UserData") != null ? identity.FindFirst("UserData").Value : null;
                string jwtId = identity.FindFirst("Sid") != null ? identity.FindFirst("Sid").Value: null;

                if (jwtUsername == null || jwtId == null) throw new Exception("Invalid token found");
                if (_passwordService.VerifyPassword(passwordreset.Password,passwordreset.VerifyPassword)) return BadRequest("Password and Verify Passwords does not match");

                return Ok(_userService.ResetPassword(Convert.ToInt32(jwtId), passwordreset.Password));
            }
            throw new Exception("Empty userclaims found");
        }

        [HttpGet("publicKey")]
        public ActionResult<ServiceResponse<string>> GetPublicKey() 
        {
            return Ok(Convert.ToBase64String(Encoding.UTF8.GetBytes(_asymmetricEncryption.GetPublicKey())));
        }

        [HttpGet("TestEncryptPassword")]
        public ActionResult<ServiceResponse<string>> TestGetEncrypted([FromQuery] string plainText) 
        {
            var publicKey = Environment.GetEnvironmentVariable("PublicKey");
            var data = Encoding.UTF8.GetBytes(plainText);

            using (var rsa = new RSACryptoServiceProvider(512))
            {
                // client encrypting data with public key issued by server                    
                rsa.FromXmlString(publicKey);
                var encryptedData = rsa.Encrypt(data, true);
                var base64Encrypted = Convert.ToBase64String(encryptedData);

                return Ok(base64Encrypted);
            }
        }

    }
}
