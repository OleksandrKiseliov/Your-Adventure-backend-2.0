using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using YourAdventure.BusinessLogic.Services.Interfaces; 
using YourAdventure.Models;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitedCountriesController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IVisitedCountriesGenerator _visitedCountriesGenerator;

        public VisitedCountriesController(IConfiguration config, IVisitedCountriesGenerator visitedCountriesGenerator) 
        {
            _config = config;
            _visitedCountriesGenerator = visitedCountriesGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<VisitedCountries>>> GetAllVisitedCountries()
        {
            var visitedCountries = await _visitedCountriesGenerator.GetAllVisitedCountries(); 
            return Ok(visitedCountries);
        }

        [HttpPost]
        public async Task<ActionResult<VisitedCountries>> AddVisitedCountry(VisitedCountries visitedCountry)
        {
            var addedVisitedCountry = await _visitedCountriesGenerator.AddVisitedCountry(visitedCountry);
            return CreatedAtAction(nameof(GetVisitedCountry), new { PersonFId = addedVisitedCountry.PersonFId, CountryFId = addedVisitedCountry.CountryFId }, addedVisitedCountry);
        }

        [HttpGet("{PersonFId}/{CountryFId}")]
        public async Task<ActionResult<VisitedCountries>> GetVisitedCountry(int PersonFId, int CountryFId)
        {
            var visitedCountry = await _visitedCountriesGenerator.GetVisitedCountry(PersonFId, CountryFId);
            if (visitedCountry == null)
            {
                return NotFound();
            }
            return Ok(visitedCountry);
        }

        [HttpDelete("{PersonFId}/{CountryFId}")]
        public async Task<IActionResult> DeleteVisitedCountry(int PersonFId, int CountryFId)
        {
            var affectedRows = await _visitedCountriesGenerator.DeleteVisitedCountry(PersonFId, CountryFId);
            if (affectedRows == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{personId}/visitedcountries")]
        public async Task<ActionResult<List<Country>>> GetVisitedCountries(int personId)
        {
            var visitedCountries = await _visitedCountriesGenerator.GetVisitedCountries(personId); 
            return Ok(visitedCountries);
        }
    }
}
