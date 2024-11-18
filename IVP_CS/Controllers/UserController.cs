using CS_Console.Model;
using CS_Console.UserRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace IVP_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            if (user.Password.Length < 8)
                return BadRequest("Password Minimum Length is 8");
            try
            {
                if (user == null)
                {
                    return BadRequest("User credentials are required.");
                }
                await _user.SignIn(user.UserID, user.Password);

                return Ok("User signed up successfully.");
            }
            catch(DbException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, "DB ERROR: User_ID Already in Use");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user.Password.Length < 8)
                return BadRequest("Password Minimum Length is 8");
            try
            {
                if (user == null)
                {
                    return BadRequest("User credentials are required.");
                }

                bool isAuthenticated = await _user.Authenticate(user.UserID, user.Password);

                if (isAuthenticated)
                {
                    return Ok("User authenticated successfully.");
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
