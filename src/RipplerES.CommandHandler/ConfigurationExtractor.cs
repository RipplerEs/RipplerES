using System;
using Microsoft.Extensions.Configuration;

namespace RipplerES.CommandHandler
{
    public static class ConfigurationExtensions
    {
        private static int ParseInt(string literal, int defaultValue)
        {
            int result;
            return int.TryParse(literal, out result) 
                ? result 
                : defaultValue;
        }

        private static bool ParseBool(string literal, bool defaultValue)
        {
            bool result;
            return bool.TryParse(literal, out result)
                ? result
                : defaultValue;
        }

        private static TResult GetAggregateSetting<TResult>(IConfiguration configurationRoot,
                                                            string section, 
                                                            string settingKey, 
                                                            TResult defaultValue, 
                                                            Func<string, TResult, TResult> parser)
        {
            var setting = configurationRoot.GetSection(section)[settingKey];

            return string.IsNullOrWhiteSpace(setting) 
                ? defaultValue 
                : parser(setting, defaultValue);
        }

        public  static int GetInt(this IConfiguration config,
                                       string section,
                                       string setting,
                                       int defaultValue)
        {
            return GetAggregateSetting(config, section, setting, defaultValue, ParseInt);
        }

        public static bool GetBool(this IConfiguration config,
                                         string section,
                                         string setting,
                                         bool defaultValue)
        {
            return GetAggregateSetting(config, section, setting, defaultValue, ParseBool);
        }
    }
}