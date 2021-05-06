/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///   operations for DB SSO
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Media.SSO
{
    using System;
    using System.Linq;
    using DB = KCM.ServiciosInternet.Plugins.Data.BDSSO;
    internal class SSO
    {
        /// <summary>
        /// Get json config path for an specific Site app
        /// </summary>
        /// <param name="strApiKey">Unique ApiKey value</param>
        /// <returns>a path(string) for que requested file</returns>
        public static string getConfigPath(string strApiKey)
        {
            using (DB.SSOEntities objSSO = new DB.SSOEntities())
            {
                Common.Library.Log.clsEscribirLog.EscribeDebug(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), Common.Library.Log.clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Media.SSO.SSO.getConfigPath", string.Format("Site ApiValue '{0}'", strApiKey));
                return (from p in objSSO.JFMGigyaSites where p.ApiKeyVc == strApiKey select p.ConfigFileVc).First();
            }
        }




    }
}
