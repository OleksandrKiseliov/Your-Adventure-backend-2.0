namespace YourAdventure.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourAdventure.Models;

using System.Threading.Tasks;

public interface ISettingsGenerator
{
    Task<List<Settings>> GetAllSettings();
    Task<Settings> CreateNewSettings(Settings settings);
    Task<int> GetInterfaceLanguage(int personId);
    Task<int> GetNotificationSetting(int personId);
    Task UpdateInterfaceLanguage(int language, int personId);
    Task UpdateNotificationSetting(bool isEnabled, int personId);
}


