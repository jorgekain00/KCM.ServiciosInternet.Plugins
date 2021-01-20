using System;

namespace KCM.ServiciosInternet.Plugins.Entities.Config
{
    /// <author>
    ///     Ing. Jorge Flores Miguel  C84818
    /// </author>
    /// <creationDate>
    ///     January 2021
    /// </creationDate>
    /// <summary>
    ///     Get config values for Gigya
    /// </summary>
    public static class GigyaConfig
    {
        /// <summary>
        /// Get Gigya Kotex's API Key
        /// </summary>
        public static string strAPIKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("GigyaConfig::APIKey", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Gigya KCC Partner's SecretKey Key
        /// </summary>
        public static string strAPISecretKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("GigyaConfig::APISecretKey", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get User key Gigya App
        /// </summary>
        public static string strUserKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("GigyaConfig::UserKey", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Secret key Gigya App
        /// </summary>
        public static string strUserSecretKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("GigyaConfig::UserSecretKey", typeof(string)).ToString();
            }
        }
    }
}
