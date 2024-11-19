using CS_Console.Model;
using CS_Console.EquityRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using CsvHelper;

namespace IVP_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquityController : ControllerBase
    {
        IEquity _security;
        private readonly ILogger<EquityController> _logger;
        public EquityController(IEquity security, ILogger<EquityController> logger)
        {
            _security = security;
            _logger = logger;

        }

        private string ExtractFirstMissingHeader(string exceptionMessage)
        {
            var match = System.Text.RegularExpressions.Regex.Match(exceptionMessage, @"Header with name '([^']+)'");
            return match.Success ? match.Groups[1].Value : "Unknown Header";
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            _logger.LogInformation("GetData Method Called");
            try
            {
                List<EditEquityModel> result = await _security.GetSecurityData();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No equity data found");
                    return NotFound(new { message = "No equity data found." });
                }
                _logger.LogInformation("Equity Data Successfully Retrieved");
                return Ok(result);
            }
            catch(DbException ex)
            {
                _logger.LogError(ex, "Database error while retrieving equity data");
                return StatusCode(StatusCodes.Status500InternalServerError, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving equity data");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Retrieving Equities: {ex.Message}");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> PostData(IFormFile file)
        {
            _logger.LogInformation("PostData Method Called");
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Invalid File Uploaded");
                    return BadRequest("No file uploaded.");
                }

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid File Format");
                    return BadRequest("Only CSV files are allowed.");
                }

                // Set the directory where the file will be saved
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                    _logger.LogInformation("Directory created for uploading file: {directory}", uploadsFolderPath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var result = await _security.ImportDataFromCsv(filePath);
                _logger.LogInformation("Successfully imported data");
                return Ok(result);
            }
            catch (HeaderValidationException ex)
            {
                var missingHeader = ExtractFirstMissingHeader(ex.Message);
                var errorMessage = $"Cannot add file: Missing header '{missingHeader}'";
                _logger.LogError("Headers Mismatching: {missingHeader}", missingHeader);
                return BadRequest(errorMessage);
            }
            catch (DbException ex)
            {
                _logger.LogError("DB Error Occured While Uploading the CSV");
                return StatusCode(StatusCodes.Status409Conflict, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error Occured While Uploading the CSV");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Uploading Equities: {ex.Message}");
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> PutData([FromBody] EditEquityModel esm)
        {
            _logger.LogInformation("Put Method Called");
            if (esm == null || esm.SecurityID <= 0)
            {
                _logger.LogInformation("Invalid Equity Data Provided for Update");
                return BadRequest(new { message = "Invalid bond data." });
            }

            try
            {
                await _security.UpdateSecurityData(esm);
                _logger.LogInformation("Successfully Updated Equity with ID - {id}", esm.SecurityID);
                return Ok();
            }
            catch (DbException ex)
            {
                _logger.LogInformation("DB Error Occured while Updating Equity with ID - {id}", esm.SecurityID);
                return StatusCode(StatusCodes.Status500InternalServerError, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unexpected Error Occured while Updating Equity with ID - {id}", esm.SecurityID);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Updating Equities: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData([FromRoute] int id)
        {
            if(id <= 0)
            {
                return BadRequest(new { message = "Invalid Security ID." });
            }
            try
            {
                var result = await _security.DeleteSecurityData(id);
                return Ok(new { message = result });
            }
            catch (DbException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Retrieving Equities: {ex.Message}");
            }
        }

        [HttpGet("getSecurityByID/{securityID}")]
        public IActionResult GetSecurityDetailsByID( [FromRoute] int securityID)
        {
            try
            {
                var securityDetails = _security.GetSecurityDetailsByID(securityID); // Replace _yourService with your actual service/operations class

                if (securityDetails == null || !securityDetails.Any())
                {
                    return NotFound(new { message = "No data found for the provided Security ID." });
                }

                return Ok(securityDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

    }
}
