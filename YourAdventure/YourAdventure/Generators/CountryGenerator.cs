using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services
{
    public class CountryGenerator : ICountryGenerator
    {
        private readonly IConfiguration _config;

        public CountryGenerator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var countries = await connection.QueryAsync<Country>("SELECT * FROM Country");
            return countries.AsList();
        }

        public async Task<Country> GetCountry(int countryId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var country = await connection.QueryFirstOrDefaultAsync<Country>("SELECT * FROM Country WHERE CountryId = @CountryId", new { CountryId = countryId });
            return country;
        }

        public async Task<Country> NewCountry(Country country)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO Country (CountryName) VALUES (@CountryName)", country);
            return country;
        }

        public async Task<int> UpdateCountry(int countryId, Country country)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var affectedRows = await connection.ExecuteAsync("UPDATE Country SET CountryName = @CountryName WHERE CountryId = @CountryId", new { CountryId = countryId, country.CountryName });
            return affectedRows;
        }

        public async Task<int> DeleteCountry(int countryId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var affectedRows = await connection.ExecuteAsync("DELETE FROM Country WHERE CountryId = @CountryId", new { CountryId = countryId });
            return affectedRows;
        }
    }
}
