using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonCS.DTO;
using SalonCS.IServices;
using System.Text.RegularExpressions;

namespace SalonCS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _userservice { get; set; }
        public AuthController(IAuthenticationService userservice)
        {
            _userservice = userservice;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<bool>>> Login([FromBody] UserCredentials usercredentials)
        {
            var response = ValidatePasswordOnLogin(usercredentials);

            if (!response.Success) return Ok(response);

            var jwt = await _userservice.Authenticate(usercredentials);

            if (String.IsNullOrEmpty(jwt)) 
            {
                response = new ServiceResponse<string> { Data = null, Message = "Username or password error", Success = false };
                return Ok(response);
            }
            
            response.Success = true;
            response.Data = jwt;

            return Ok(response);
        }

        private ServiceResponse<string> ValidatePasswordOnRegister(UserCredentials usercredentials) 
        {
            var response = ValidatePasswordOnLogin(usercredentials);

            response.Success = false;

            if (usercredentials.Password.Length < 8)
            {
                response.Message = "Password should contain more than 8 characters";
            }
            else if (!(Regex.Match(usercredentials.Password,@"(?=.*\d)")).Success)
            {
                response.Message = "Password should contain atleast 1 digit";
            }
            else if (!(Regex.Match(usercredentials.Password, @"(?=.*[a-z])")).Success)
            {
                response.Message = "Password should contain atleast 1 lowercase character";
            }
            else if (!(Regex.Match(usercredentials.Password, @"(?=.*[A-Z])")).Success)
            {
                response.Message = "Password should contain atleast 1 uppercase character";
            }
            else 
            {
                response.Success = true;
            }

            return response;
        }
        private ServiceResponse<string> ValidatePasswordOnLogin(UserCredentials usercredentials)
        {
            var response = new ServiceResponse<String>
            {
                Success = false,
                Data = null
            };

            if (String.IsNullOrEmpty(usercredentials.Username))
            {
                response.Message = "Username cannot be empty";
            }
            else if (String.IsNullOrEmpty(usercredentials.Password))
            {
                response.Message = "Password cannot be empty";
            }
            else
            {
                response.Success = true;
            }           

            return response;
        }

    }
}
