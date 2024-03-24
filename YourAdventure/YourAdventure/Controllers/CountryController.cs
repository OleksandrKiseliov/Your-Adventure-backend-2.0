using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using YourAdventure.Models;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CountryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetAllCountries()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var countries = await connection.QueryAsync<Country>("SELECT * FROM Country");
            return Ok(countries);
        }

        [HttpGet("{CountryId}")]
        public async Task<ActionResult<Country>> GetCountry(int CountryId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var country = await connection.QueryFirstOrDefaultAsync<Country>("SELECT * FROM Country WHERE CountryId = @CountryId", new { CountryId });
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult<Country>> NewCountry(Country country)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO Country (CountryName) VALUES (@CountryName)", country);
            return CreatedAtAction(nameof(GetCountry), new { CountryId = country.CountryId }, country);
        }

        [HttpPut("{CountryId}")]
        public async Task<IActionResult> UpdateCountry(int CountryId, Country country)
        {
            if (CountryId != country.CountryId)
            {
                return BadRequest();
            }

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var affectedRows = await connection.ExecuteAsync("UPDATE Country SET CountryName = @CountryName WHERE CountryId = @CountryId", country);
            if (affectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{CountryId}")]
        public async Task<IActionResult> DeleteCountry(int CountryId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var affectedRows = await connection.ExecuteAsync("DELETE FROM Country WHERE CountryId = @CountryId", new { CountryId });
            if (affectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
