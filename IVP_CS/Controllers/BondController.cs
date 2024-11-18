using CS_Console.Model;
using BondConsoleApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BondConsoleApp.Models;

namespace CS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BondController : ControllerBase
    {
        private readonly IBond _bondOperations;

        public BondController(IBond bondOperations)
        {
            _bondOperations = bondOperations;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Optionally check file type if needed
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only CSV files are allowed.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var res = await _bondOperations.ImportDataFromCsv(stream);
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBond(int id)
        {
            try
            {
                await _bondOperations.DeleteBondData(id);
                return Ok(new { message = "Bond successfully marked as inactive." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateBond([FromBody] EditBondModel bond)
        {
            if (bond == null || bond.SecurityID <= 0)
            {
                return BadRequest(new { message = "Invalid bond data." });
            }

            try
            {
                string msg = await _bondOperations.UpdateBondData(bond);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetEditBondsData()
        {
            try
            {
                var bonds = await _bondOperations.GetBondsData(); // Call the method to get bond data
                if (bonds == null || !bonds.Any())
                {
                    return NotFound(new { message = "No bond data found." });
                }

                return Ok(bonds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }


    }
}
