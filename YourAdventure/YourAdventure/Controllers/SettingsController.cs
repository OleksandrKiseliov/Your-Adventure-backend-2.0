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

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class  SettingsController : ControllerBase
    {

        private readonly ISettingsGenerator _settingsGenerator;

        public SettingsController(ISettingsGenerator settingsGenerator)
        {
            _settingsGenerator = settingsGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            var settings = await _settingsGenerator.GetAllSettings();
            return Ok(settings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewSettings(Settings settings)
        {
            var newSettings = await _settingsGenerator.CreateNewSettings(settings);
            return Ok(newSettings);
        }

        [HttpGet("language")]
        public async Task<IActionResult> GetInterfaceLanguage()
        {
            var language = await _settingsGenerator.GetInterfaceLanguage();
            return Ok(language);
        }

        [HttpGet("notification")]
        public async Task<IActionResult> GetNotificationSetting()
        {
            var isEnabled = await _settingsGenerator.GetNotificationSetting();
            return Ok(isEnabled);
        }

        [HttpPost("language")]
        public async Task<IActionResult> UpdateInterfaceLanguage([FromBody] string language)
        {
            await _settingsGenerator.UpdateInterfaceLanguage(language);
            return Ok();
        }

        [HttpPost("notification")]
        public async Task<IActionResult> UpdateNotificationSetting([FromBody] bool isEnabled)
        {
            await _settingsGenerator.UpdateNotificationSetting(isEnabled);
            return Ok();
        }

    }
}
