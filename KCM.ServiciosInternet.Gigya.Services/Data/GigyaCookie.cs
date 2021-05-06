/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Gigya.Services.Data
{
    using KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces;
    using System;
    using Newtonsoft.Json;
    using System.Text;
    using System.Text.RegularExpressions;

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
        /// Deserialize to a GigyaCookie struct
        /// </summary>
        /// <param name="strCookieName">Cookie name</param>
        /// <param name="strCookieContent">Cookie content</param>
        /// <returns>True if Deserialize process was succesful</returns>
        public bool Deserialize(string strCookieName, string strCookieContent)
        {
            string strKey = string.Empty;
            string strValue = string.Empty;

            var objRegex = Regex.Match(strCookieContent, @"([^;=]+)=([^;]+)(;|$)", RegexOptions.IgnoreCase);


            while (objRegex.Success)
            {
                var objGroups = objRegex.Groups;
                strKey = objGroups[1].Value;
                strValue = objGroups[2].Value;

                if (strKey == strCookieName)
                {
                    this = JsonConvert.DeserializeObject<GigyaCookie>(strValue);
                    return true;
                }
                else
                {
                    objRegex.NextMatch();
                }
            }

            return false;
        }
        /// <summary>
        /// Create a string for a cookie
        /// </summary>
        /// <param name="strCookieName">CookieName</param>
        /// <param name="IsDebug"></param>
        /// <returns>string for a cookie</returns>
        public string Serialize(string strCookieName, bool IsDebug)
        {
            StringBuilder objSB = new StringBuilder();
            string strValue = JsonConvert.SerializeObject(this);

            objSB.Append(string.Format("{0}={1}; Max-Age={2};", strCookieName, strValue, this.intExpirationSessionInMins * 60));

            if (!IsDebug)
            {
                objSB.Append("; HttpOnly; Secure; SameSite=None");
            }
            return objSB.ToString();
        }
    }
}
