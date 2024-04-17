﻿namespace YourAdventure.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourAdventure.Models;

using System.Threading.Tasks;

public interface ISettingsGenerator
{
    Task<Settings> GetAllSettings();
    Task<Settings> CreateNewSettings(Settings settings);
    Task<string> GetInterfaceLanguage();
    Task<bool> GetNotificationSetting();
    Task UpdateInterfaceLanguage(string language);
    Task UpdateNotificationSetting(bool isEnabled);
}

