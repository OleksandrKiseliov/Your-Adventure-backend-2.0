using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services
{
    public class VisitedCountriesGenerator : IVisitedCountriesGenerator
    {
        private readonly IConfiguration _config;

        public VisitedCountriesGenerator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<VisitedCountries>> GetAllVisitedCountries()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return (await connection.QueryAsync<VisitedCountries>("SELECT * FROM VisitedCountries")).AsList();
        }

        public async Task<VisitedCountries> GetVisitedCountry(int personFId, int countryFId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<VisitedCountries>(
                "SELECT * FROM VisitedCountries WHERE PersonFId = @personFId AND CountryFId = @countryFId",
                new { personFId, countryFId });
        }

        public async Task<VisitedCountries> AddVisitedCountry(VisitedCountries visitedCountry)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            // Виклик збереженої процедури через Dapper
            await connection.ExecuteAsync(
                "visitCountry", // Ім'я збереженої процедури
                new { PersonFId = visitedCountry.PersonFId, CountryFId = visitedCountry.CountryFId },
                commandType: CommandType.StoredProcedure);

            return visitedCountry;
        }

        public async Task<int> DeleteVisitedCountry(int personFId, int countryFId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync(
                "DELETE FROM VisitedCountries WHERE PersonFId = @personFId AND CountryFId = @countryFId",
                new { personFId, countryFId });
        }

        public async Task<List<Country>> GetVisitedCountries(int personId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return (await connection.QueryAsync<Country>(
                @"SELECT c.Geojson, c.CountryName
                  FROM Country c
                  INNER JOIN VisitedCountries vc ON c.CountryId = vc.CountryFId
                  WHERE vc.PersonFId = @personId "
,
                new { personId })).AsList();
        }
    }
}
