using CS_Console.LogRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVP_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        ILog _log;
        public LogController(ILog log)
        {
            _log = log;
        }
        [HttpGet("")]
        public IActionResult GetLogs()
        {
            try
            {
                var result = _log.GetAllSecurityUpdateLogs();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Retrieving Logs: {ex.Message}");
            }
            
        }
    }
}
