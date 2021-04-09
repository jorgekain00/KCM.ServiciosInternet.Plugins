/***********************************************************************************************
 *  Date    : March 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Plugins.Data.Config
{
    using System;
    /// <author>
    ///     Ing. Jorge Flores Miguel  C84818
    /// </author>
    /// <creationDate>
    ///     March 2021
    /// </creationDate>
    /// <summary>
    ///     Global Config values for the service
    /// </summary>
    class GlobalConfig
    {
        /// <summary>
        /// Url Google Validator from ReCAPTCHA
        /// </summary>
        public static string UrlGoogleValidator
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("GlobalConfig::UrlGoogleValidator", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Flag -- disable recaptcha
        /// </summary>
        public static bool iSReCAPTCHADisable
        {
            get
            {
                return Convert.ToBoolean(new System.Configuration.AppSettingsReader().GetValue("GlobalConfig::DisableReCAPTCHA", typeof(string)).ToString());
            }
        }
    }
}
