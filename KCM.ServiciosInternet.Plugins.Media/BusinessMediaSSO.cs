/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///   operations for DB SSO
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Media
{
    public class BusinessMediaSSO
    {
        /// <summary>
        /// Get Deserialized json config object into SSOConfig.Root object
        /// </summary>
        /// <param name="strPath">site config file Path</param>
        /// <returns>SSOConfig.Root with config settings for each site</returns>
        public static string getConfigPath(string strApiKey)
        {
            return SSO.SSO.getConfigPath(strApiKey);
        }
    }
}
