
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services
{
    public class ColorGenerator : IColorGenerator
    {
        private readonly IConfiguration _config;

        public ColorGenerator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Color>> GetAllColors()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var colors = await connection.QueryAsync<Color>("SELECT * FROM Color");
            return colors.AsList();
        }

        public async Task<Color> NewColor(Color color)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO Color ( ColorName) VALUES ( @ColorName)", color);
            return color;
        }
    }
}
