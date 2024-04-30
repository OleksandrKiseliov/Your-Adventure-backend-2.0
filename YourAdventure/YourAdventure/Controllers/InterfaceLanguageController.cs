using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YourAdventure.Models;
using System;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Generators.Interfaces;
using YourAdventure.BusinessLogic.Services;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterfaceLanguageController : ControllerBase
    {

        private readonly IInterfaceLanguageGenerator _interfaceLanguageGenerator;

        public InterfaceLanguageController(IInterfaceLanguageGenerator interfaceLanguageGenerator)
        {
            _interfaceLanguageGenerator = interfaceLanguageGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInterfaceLanguages()
        {
            var interfaceLanguage = await _interfaceLanguageGenerator.GetAllInterfaceLanguages();
            return Ok(interfaceLanguage);
        }

        [HttpPost]
        public async Task<IActionResult> NewInterfaceLanguage(InterfaceLanguage interfaceLanguage)
        {
            var interfaceLanguageController = await _interfaceLanguageGenerator.NewInterfaceLanguage(interfaceLanguage);
            return Ok(interfaceLanguageController);
        }

    }
}