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
        public EquityController(IEquity security)
        {
            _security = security;
        }

        private string ExtractFirstMissingHeader(string exceptionMessage)
        {
            var match = System.Text.RegularExpressions.Regex.Match(exceptionMessage, @"Header with name '([^']+)'");
            return match.Success ? match.Groups[1].Value : "Unknown Header";
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                List<EditEquityModel> result = await _security.GetSecurityData();
                if (result == null || !result.Any())
                {
                    return NotFound(new { message = "No equity data found." });
                }
                return Ok(result);
            }
            catch(DbException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Retrieving Equities: {ex.Message}");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> PostData(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Only CSV files are allowed.");
                }

                // Set the directory where the file will be saved
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var result = await _security.ImportDataFromCsv(filePath);
                return Ok(result);
            }
            catch (HeaderValidationException ex)
            {
                var missingHeader = ExtractFirstMissingHeader(ex.Message);
                var errorMessage = $"Cannot add file: Missing header '{missingHeader}'";

                return BadRequest(errorMessage);
            }
            catch (DbException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Uploading Equities: {ex.Message}");
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> PutData([FromBody] EditEquityModel esm)
        {
            if (esm == null || esm.SecurityID <= 0)
            {
                return BadRequest(new { message = "Invalid bond data." });
            }

            try
            {
                await _security.UpdateSecurityData(esm);
                return Ok();
            }
            catch (DbException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"DB ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
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
