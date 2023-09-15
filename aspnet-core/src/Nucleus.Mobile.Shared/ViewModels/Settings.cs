using System;
using System.Collections.Generic;
using System.Text;

// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace  Nucleus.ViewModels.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters§
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string LastUsenameSettingsKey = "last_Username_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly bool SettingsBoolDefault = false;
        private static readonly DateTime SettingsDateTimeDefault = DateTime.Now;

        #endregion

        public static bool isEmployeeChecked
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(isEmployeeChecked), SettingsBoolDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(isEmployeeChecked), value);
            }
        }
        public static bool isEquipmentChecked
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(isEquipmentChecked), SettingsBoolDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(isEquipmentChecked), value);
            }
        }
        public static string LastUseUserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastUsenameSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastUsenameSettingsKey, value);
            }
        }
        public static string RememberPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(RememberPassword), SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(RememberPassword), value);
            }
        }

        public static string RememberUserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(RememberUserName), SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(RememberUserName), value);
            }
        }

        public static int Entertime
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Entertime), 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Entertime), value);
            }
        }


    }
}

