using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonCS.DTO;
using SalonCS.IServices;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SalonCS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authservice { get; set; }
        public AuthController(IAuthenticationService authservice)
        {
            _authservice = authservice;
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
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] PasswordReset passwordreset) 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string? jwtUsername =  identity.FindFirst("UserData") != null ? identity.FindFirst("UserData").Value : null ;
                string? jwtId = identity.FindFirst("Sid") != null ? identity.FindFirst("Sid").Value : null;

                if (jwtUsername == null || jwtId == null) throw new Exception("Invalid token found");
                if (!passwordreset.Password.Equals(passwordreset.VerifyPassword)) return BadRequest("Password and Verify Passwords does not match");

                //Should implement password reset db save service 

            }
            throw new Exception("Empty userclaims found");
        }

    }
}
