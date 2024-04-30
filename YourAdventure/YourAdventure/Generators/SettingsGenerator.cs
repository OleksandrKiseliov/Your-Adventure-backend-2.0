using Dapper;
using Microsoft.Extensions.Configuration;
using System;
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

        public async Task<Settings> GetAllSettings()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<Settings>("SELECT * FROM Settings ");
        }

        public async Task<Settings> CreateNewSettings(Settings settings)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.ExecuteAsync("insert into Settings (InterfaceLanguageFId, Notification, ColorFId, PersonFId)" +
                " values (@InterfaceLanguageFId, @Notification, @ColorFId, @PersonFId)", settings);
            return settings;
        }

        public async Task<string> GetInterfaceLanguage()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<string>("SELECT InterfaceLanguageFID FROM Settings ");
        }

        public async Task<bool> GetNotificationSetting()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<bool>("SELECT Notification FROM Settings ");
        }

        public async Task UpdateInterfaceLanguage(string language)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE Settings SET InterfaceLanguageFID = @Language ", new { Language = language });
        }

        public async Task UpdateNotificationSetting(bool isEnabled)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE Settings SET Notification = @IsEnabled ", new { IsEnabled = isEnabled });
        }
    }
}
