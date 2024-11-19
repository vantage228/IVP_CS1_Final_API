using CS_Console.Model;
using BondConsoleApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BondConsoleApp.Models;
using CsvHelper;

namespace CS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BondController : ControllerBase
    {
        private readonly IBond _bondOperations;
        private readonly ILogger<BondController> _logger;   

        public BondController(IBond bondOperations, ILogger<BondController> logger)
        {
            _bondOperations = bondOperations;
            _logger = logger;

        }

        //To avoid user from entering wrong file
        private string ExtractFirstMissingHeader(string exceptionMessage)
        {
            var match = System.Text.RegularExpressions.Regex.Match(exceptionMessage, @"Header with name '([^']+)'");
            return match.Success ? match.Groups[1].Value : "Unknown Header";
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv([FromForm] IFormFile file)
        {
            _logger.LogInformation("Upload CSV Initiated");
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No File Uploaded");
                return BadRequest("No file uploaded.");
            }

            // Check file type
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid File Format: {FileName}", file.FileName);
                return BadRequest("Only CSV files are allowed.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var res = await _bondOperations.ImportDataFromCsv(stream);
                    _logger.LogInformation("CSV File Processed Successfully");
                    return Ok(res);
                }
            }
            catch (HeaderValidationException ex)
            {
                var missingHeader = ExtractFirstMissingHeader(ex.Message);
                var errorMessage = $"Cannot add file: Missing header '{missingHeader}'";
                _logger.LogError("Headers Mismatching: {missingHeader}", missingHeader);
                return BadRequest(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Uploading the CSV");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBond(int id)
        {
            _logger.LogInformation("DeleteBond Method Initiated for Bond_Id: {id}", id);
            try
            {
                await _bondOperations.DeleteBondData(id);
                _logger.LogInformation("Bond with Id - {id}, marked as inactive", id);
                return Ok(new { message = "Bond successfully marked as inactive." });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error while Deleting the bond with ID: {id}", id);
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateBond([FromBody] EditBondModel bond)
        {
            _logger.LogInformation("UpdateBond Method Initiated for Bond_Id: {id}", bond.SecurityID);
            if (bond == null || bond.SecurityID <= 0)
            {
                _logger.LogWarning("Invalid Bond Data");
                return BadRequest(new { message = "Invalid bond data." });
            }

            try
            {
                string msg = await _bondOperations.UpdateBondData(bond);
                _logger.LogInformation("Successfully updated Bond with Id - {id}", bond.SecurityID);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error while Updating the bond with ID: {id}", bond.SecurityID);
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetEditBondsData()
        {
            _logger.LogInformation("GetBond Method Initiated");
            try
            {
                var bonds = await _bondOperations.GetBondsData(); // Call the method to get bond data
                if (bonds == null || !bonds.Any())
                {
                    _logger.LogWarning("No Bond Data Found");
                    return NotFound(new { message = "No bond data found." });
                }
                _logger.LogInformation("{count} Bonds Retreived Successfully", bonds.Count());
                return Ok(bonds);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error Occured while Fetching the Data");
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }


    }
}
