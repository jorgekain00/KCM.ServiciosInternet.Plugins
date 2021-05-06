/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///     Global Config values for the app
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Data.SSO.Config
{
    using System;
    public static class GlobalConfig
    {
        /// <summary>
        /// Is app in Debug Mode
        /// </summary>
        public static bool IsAppInDebugMode
        {
            get
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("GlobalConfig::isDebug"));
            }
        }
        /// <summary>
        /// Get Service Url
        /// </summary>
        public static string strServiceUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("GlobalConfig::ServiceUrl");
            }
        }
        /// <summary>
        /// Get Reset PS Url
        /// </summary>
        public static string strResetPSUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("GlobalConfig::ResetPSUrl");
            }
        } 
        /// <summary>
        /// Get Reset PS Url
        /// </summary>
        public static string strScriptsLibPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("GlobalConfig::ScriptsLibPath");
            }
        }/// <summary>
        /// Get Data folder
        /// </summary>
        public static string strDataPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("GlobalConfig::DataPath");
            }
        }
    }
}
