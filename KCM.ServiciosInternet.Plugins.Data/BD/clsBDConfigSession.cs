using System;
using System.Configuration;


namespace KCM.ServiciosInternet.Plugins.Entities.BD
{
    public static class clsBDConfigSession
    {
        public static string strConnBD
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn::BD"].ConnectionString;
            }
        }
    }
}
