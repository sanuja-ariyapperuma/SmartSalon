using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonCS.Data;
using SalonCS.DTO;
using SalonCS.IServices;
using SalonCS.Model;

namespace SalonCS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.SuperAdmin))]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<bool>>> Register([FromBody] User registerUser) 
        {
            var isUsernameExists = await _userService.IsUsernameAlreadyExists(registerUser.Username);

            if (isUsernameExists) return Ok(new ServiceResponse<string> { Data = null, Message="Username already exists", Success = false });

            _userService.RegisterUser(registerUser);

            return Ok(true);
        }

    }
}
