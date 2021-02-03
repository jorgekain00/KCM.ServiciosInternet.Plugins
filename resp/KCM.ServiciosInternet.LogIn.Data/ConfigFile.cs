using System;

namespace KCM.ServiciosInternet.LogIn.Data
{
    public class ConfigFile
    {
        public string strAPIKey { get
            {
                return new appsettingsreader
            }
        }

        public string strAPISecretKey { get; set; }

        public string strUserKey { get; set; }

        public string strUserSecretKey { get; set; }
    }
}
