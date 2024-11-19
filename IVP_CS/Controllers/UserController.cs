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
        private readonly ILogger<UserController> _logger;

        public UserController(IUser user, ILogger<UserController> logger)
        {
            _user = user;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            _logger.LogInformation("SignUp Method Initiated");
            
            if (user == null)
            {
                _logger.LogWarning("User Credentials Not Provided");
                return BadRequest("User credentials are required.");
            }
            if (user.Password.Length < 8)
            {
                _logger.LogWarning("Password Requirement Not Satisfied");
                return BadRequest("Password Minimum Length is 8");
            }
            try
            {
                await _user.SignIn(user.UserID, user.Password);
                _logger.LogInformation("User with {userId} SignedUp Successfully", user.UserID);
                return Ok("User signed up successfully.");
            }
            catch(DbException ex)
            {
                _logger.LogError(ex, "Database Error: UserID - {userId} already in use", user.UserID);
                return StatusCode(StatusCodes.Status409Conflict, "DB ERROR: User_ID Already in Use");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during signup for UserID - {userId}", user.UserID);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            _logger.LogInformation("Login by User - {id} Initiated", user.UserID);
            if (user.Password.Length < 8)
            {
                _logger.LogWarning("Password Requirement Not Satisfied");
                return BadRequest("Password Minimum Length is 8");
            }
            if (user == null)
            {
                _logger.LogWarning("User Credentials Not Provided");
                return BadRequest("User credentials are required.");
            }
            try
            {
                bool isAuthenticated = await _user.Authenticate(user.UserID, user.Password);

                if (isAuthenticated)
                {
                    _logger.LogInformation("User with {id}, Successfully Authenticated", user.UserID);
                    return Ok("User authenticated successfully.");
                }
                else
                {
                    _logger.LogInformation("Invalid Credentials provided for User with {id}", user.UserID);
                    return Unauthorized("Invalid credentials.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to login {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
