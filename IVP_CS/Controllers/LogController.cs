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
        private readonly ILogger<LogController> _logger;   
        public LogController(ILog log, ILogger<LogController> logger)
        {
            _log = log;
            _logger = logger;

        }
        [HttpGet("")]
        public IActionResult GetLogs()
        {
            _logger.LogInformation("GetLogs Method Called");
            try
            {
                var result = _log.GetAllSecurityUpdateLogs();
                _logger.LogInformation("{logCount} Logs retreived Successfully", result.Count());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While Fetching the Logs");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Retrieving Logs: {ex.Message}");
            }
            
        }
    }
}
