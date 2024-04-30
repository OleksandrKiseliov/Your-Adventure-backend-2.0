
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Generators.Interfaces;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services
{
    public class InterfaceLanguageGenerator : IInterfaceLanguageGenerator
    {
        private readonly IConfiguration _config;

        public InterfaceLanguageGenerator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<InterfaceLanguage>> GetAllInterfaceLanguages()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var interfaceLanguage = await connection.QueryAsync<InterfaceLanguage>("select * from InterfaceLanguage");
            return interfaceLanguage.AsList();
        }

        public async Task<InterfaceLanguage> NewInterfaceLanguage(InterfaceLanguage interfaceLanguage)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into InterfaceLanguage (InterfaceLanguage)" +
                " values ( @InterfaceLanguageName)", interfaceLanguage);
            return interfaceLanguage;
        }
    }
}
