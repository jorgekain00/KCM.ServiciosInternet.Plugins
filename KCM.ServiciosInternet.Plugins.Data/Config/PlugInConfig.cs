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
    ///     Get config values for PlugIn
    /// </summary>
    public static class PlugInConfig
    {
        /// <summary>
        /// Get Secret ReCAPTCHA LogIn Flow Key
        /// </summary>
        public static string strReCAPTCHALogInFlowSecret
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::reCAPTCHALogInFlowSecret", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Secret ReCAPTCHA Register Flow Key
        /// </summary>
        public static string strReCAPTCHARegisterFlowSecret
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::reCAPTCHARegisterFlowSecret", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Service Url
        /// </summary>
        public static string strServiceUrl
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::ServiceUrl", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Reset PS Url
        /// </summary>
        public static string strResetPSUrl
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::ResetPSUrl", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Log File Name
        /// </summary>
        public static string strLogName
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::Logname", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get SMTP Server Name
        /// </summary>
        public static string strSMTPServer
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::SMTPServer", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get FromAddress to mail
        /// </summary>
        public static string strFromAddressEmail
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::FromAddressEmail", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get Client Site Url for Reset Password
        /// </summary>
        public static string strSiteUrl
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::SiteUrl", typeof(string)).ToString();
            }
        }
        /// <summary>
        /// Get Email Subject for Reset Password
        /// </summary>
        public static string strResetEmailSubject
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::ResetEmailSubject", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get Email Template for Reset Password
        /// </summary>
        public static string StrMailHTML
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::HTMLMailTemplate", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get Reset Template for Reset Password
        /// </summary>
        public static string strResetHTML
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::HTMLReset", typeof(string)).ToString();
                    
            }
        }
        /// <summary>
        /// Get Loading Image Name
        /// </summary>
        public static string strLoadingImage
        {
            get
            {
                return  new System.Configuration.AppSettingsReader().GetValue("PlugInConfig::LoadingImage", typeof(string)).ToString();
                    
            }
        }
    }
}
