using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services
{
    public class SettingsGenerator : ISettingsGenerator
    {
        private readonly IConfiguration _config;

        public SettingsGenerator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetInterfaceLanguage()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<string>("SELECT InterfaceLanguage FROM Settings ");
        }

        public async Task<bool> GetNotificationSetting()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<bool>("SELECT Notification FROM Settings ");
        }

        public async Task UpdateInterfaceLanguage(string language)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE Settings SET InterfaceLanguage = @Language ", new { Language = language });
        }

        public async Task UpdateNotificationSetting(bool isEnabled)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE Settings SET Notification = @IsEnabled ", new { IsEnabled = isEnabled });
        }
    }
}
