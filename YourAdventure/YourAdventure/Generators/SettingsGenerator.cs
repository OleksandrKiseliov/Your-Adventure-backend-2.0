using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<List<Settings>> GetAllSettings()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var settings =  await connection.QueryAsync<Settings>("GetSettings", commandType: CommandType.StoredProcedure);
            return settings.AsList();

        }

        public async Task<Settings> CreateNewSettings(Settings settings)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("interfaceLanguageId", settings.InterfaceLanguageFId);
            parameters.Add("notification", settings.Notification);
            parameters.Add("colorId", settings.ColorFId);
            parameters.Add("personId", settings.PersonFId);
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("CreateSettings", parameters, commandType: CommandType.StoredProcedure);
            return settings;
        }

        public async Task<int> GetInterfaceLanguage(int personId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("personId", personId);
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<int>("GetLanguage", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetNotificationSetting(int personId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("personId", personId);
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryFirstOrDefaultAsync<int>("GetNotification", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateInterfaceLanguage(int language, int personId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("language", language);
            parameters.Add("personId", personId);
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UpdateLanguage", parameters, commandType: CommandType.StoredProcedure);

        }

        public async Task UpdateNotificationSetting(bool isEnabled, int personId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("isEnabled", isEnabled);
            parameters.Add("personId", personId);
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UpdateNotification", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
