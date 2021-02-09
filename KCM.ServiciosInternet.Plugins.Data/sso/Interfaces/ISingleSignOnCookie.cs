﻿/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/

namespace KCM.ServiciosInternet.Plugins.Data.sso.Interfaces
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
    }
}