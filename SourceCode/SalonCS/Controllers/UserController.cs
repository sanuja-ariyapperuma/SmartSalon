using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonCS.Data;
using SalonCS.DTO;
using SalonCS.Model;

namespace SalonCS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.SuperAdmin))]
    public class UserController : ControllerBase
    {
        private DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<bool>>> Register([FromBody] User? registerUser) 
        {
            
            var isUsernameExists = await _dataContext.Users.Where(user => registerUser.Username == user.Username).CountAsync();
            if (isUsernameExists > 0) return Ok(new ServiceResponse<string> { Data = null, Message="Username already exists", Success = false });

            registerUser.IsInnitialLogin = true;
            registerUser.IsActive = true;
            registerUser.Password = null;

            await _dataContext.Users.AddAsync(registerUser);
            await _dataContext.SaveChangesAsync();

            return Ok(true);
        }

    }
}
