/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///   operations on Site config for SSO
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Business.Tools
{
    using KCM.ServiciosInternet.Plugins.Data.SSO.Config;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    internal class SiteConfig
    {
        /// <summary>
        /// Get Deserialized json config object into SSOConfig.Root object
        /// </summary>
        /// <param name="strPath">site config file Path</param>
        /// <returns>SSOConfig.Root with config settings for each site</returns>
        public static SSOConfig.Root getSiteConfig(string strPath)
        {
            Common.Library.Log.clsEscribirLog.EscribeDebug(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), Common.Library.Log.clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.Tools.SiteConfig.getSiteConfig", string.Format("Site config JSON file  Path '{0}'", strPath));
            return JsonConvert.DeserializeObject<SSOConfig.Root>(File.ReadAllText(strPath));
        }
    }
}
