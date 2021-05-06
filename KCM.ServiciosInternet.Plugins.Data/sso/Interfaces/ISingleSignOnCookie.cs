/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/

namespace KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces
{
    using System;
    /// <summary>
    /// Constaint fields for the cookie
    /// </summary>
    public interface ISingleSignOnCookie
    {
        /// <summary>
        /// User id
        /// </summary>
        string strUID { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        string strRegToken { get; set; }
        /// <summary>
        /// Last access
        /// </summary>
        DateTime dtLastAccess { get; set; }
        /// <summary>
        /// Expiration Session in terms of minutes
        /// </summary>
        int intExpirationSessionInMins { get; set; }
        /// <summary>
        /// Create a string for a cookie
        /// </summary>
        /// <param name="nameCookie">CookieName</param>
        /// <param name="IsDebug"></param>
        /// <returns>string for a cookie</returns>
        string Serialize(string strCookieName, bool IsDebug=false);
        /// <summary>
        /// Deserialize to a ISingleSignOnCookie object
        /// </summary>
        /// <param name="strCookieName">Cookie name</param>
        /// <param name="strCookieContent">Cookie content</param>
        bool Deserialize(string strCookieName, string strCookieContent);
    }
}
