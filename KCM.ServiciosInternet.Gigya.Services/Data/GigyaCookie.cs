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
    /// <summary>
    /// Constaint fields for the cookie
    /// </summary>
    public struct GigyaCookie : ISingleSignOnCookie
    {
        /// <summary>
        /// User id
        /// </summary>
        public string strUID { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        public string strRegToken { get; set; }
        /// <summary>
        /// Last access
        /// </summary>
        public DateTime dtLastAccess { get; set; }
        /// <summary>
        /// Expiration Session in terms of minutes
        /// </summary>
        public int intExpirationSessionInMins { get; set; }


        // TODO: JFM implementar métodos para serializar y deserializar este objeto en JSON
    }
}
