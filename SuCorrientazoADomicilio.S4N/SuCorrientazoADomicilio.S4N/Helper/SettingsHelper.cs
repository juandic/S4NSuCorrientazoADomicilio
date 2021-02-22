using System.Configuration;

namespace S4N.SuCorrientazoADomicilio.Dto.Helper
{
    public static class SettingsHelper
    {
        public static string SettingsKeyToString(string appSetting)
        {
            if (!string.IsNullOrEmpty(appSetting) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[appSetting]))
            {
                return ConfigurationManager.AppSettings[appSetting];
            }

            return string.Empty;
        }

        public static int SettingsKeyToInt(string appSetting)
        {
            if (!string.IsNullOrEmpty(appSetting) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[appSetting]))
            {
                int.TryParse(ConfigurationManager.AppSettings[appSetting], out var value);
                return value;
            }

            return 0;
        }
    }
}
