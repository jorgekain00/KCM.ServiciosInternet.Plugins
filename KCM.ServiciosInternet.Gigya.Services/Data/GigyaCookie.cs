/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Gigya.Services.Data
{
    using KCM.ServiciosInternet.Plugins.Data.sso.Interfaces;
    using System;
    using Newtonsoft.Json;
    /// <summary>
    /// Constaint fields for the cookie
    /// </summary>
    public struct GigyaCookie : ISingleSignOnCookie
    {
        /// <summary>
        /// User id
        /// </summary>
        [JsonProperty]
        public string strUID { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        [JsonProperty]
        public string strRegToken { get; set; }
        /// <summary>
        /// Last access
        /// </summary>
        [JsonProperty]
        public DateTime dtLastAccess { get; set; }
        /// <summary>
        /// Expiration Session in terms of minutes
        /// </summary>
        [JsonProperty]
        public int intExpirationSessionInMins { get; set; }
        /// <summary>
        /// Serialize fields into a output string
        /// </summary>
        /// <returns>A string gigyaCookie value</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// Update gigyaCookie from a json string
        /// </summary>
        /// <param name="strJson">json string for update gigyaCookie</param>
        public void Deserialize(string strJson)
        {
            this =  JsonConvert.DeserializeObject<GigyaCookie>(strJson);
        }
    }
}
