using System;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public static class clsConfigPlugIn
    {
        public static string strAPIKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::APIKey", typeof(string)).ToString();
            }
        }

        public static string strAPISecretKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::APISecretKey", typeof(string)).ToString();
            }
        }

        public static string strUserKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::UserKey", typeof(string)).ToString();
            }
        }

        public static string strUserSecretKey
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::UserSecretKey", typeof(string)).ToString();
            }
        }

        public static string strReCAPTCHALogInFlowSecret
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::reCAPTCHALogInFlowSecret", typeof(string)).ToString();
            }
        }
        public static string strReCAPTCHARegisterFlowSecret
        {
            get
            {
                return new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::reCAPTCHARegisterFlowSecret", typeof(string)).ToString();
            }
        }

        public static int intExpirationKeyMins
        {
            get
            {
                return Convert.ToInt32(
                    new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::ExpirationKeyMins", typeof(string)).ToString()
                    );
            }
        }

        public static int intExpirationSessionDays
        {
            get
            {
                return Convert.ToInt32(
                    new System.Configuration.AppSettingsReader().GetValue("ConfigPlugIn::ExpirationSessionDays", typeof(string)).ToString()
                    );
            }
        }
    }
}
