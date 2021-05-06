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
    using System.Runtime.Serialization;

    /// <summary>
    /// Constains all fields for Gigya REST communication
    /// </summary>
    /// <remarks>
    /// <para>Not all fields are necesary for every operation</para>
    /// </remarks>
    [DataContract]
    public struct GigyaData : ISingleSignOnData
    {
        /// <summary>
        /// Unique Id or Email
        /// </summary>
        public string strEmail { get; set; }
        /// <summary>
        /// Password 
        /// </summary>
        public string strPassword { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        public string strUID { get; set; } 
        /// <summay>
        /// Profile value-pair fields
        /// </summary>
        public string strProfile { get; set; }
        /// <summay>
        /// Data value-pair fields
        /// </summary>
        public string strData { get; set; }
        /// <summary>
        /// List of Extra profile fields Descriptor
        /// </summary>
        public string strExtraProfileFieldsDescriptor { get; set; }
        /// <summary>
        /// List of value-pair Extra fields
        /// </summary>
        public string strExtraProfileFields { get; set; }
        /// <summary>
        /// Session cookie from giGya Accounts
        /// </summary>
        public ISingleSignOnCookie objSessionCookie { get; set; }
        /// <summary>
        /// Is Account Pending Registration due to missing values
        /// </summary>
        public bool boolIsAccountPendingRegistration { get; set; }
        /// <summary>
        /// Is Account Pending Email verification
        /// </summary>
        public bool boolIsAccountPendingVerification { get; set; }
        /// <summary>
        /// Error messages
        /// </summary>
        public string strErrormessage { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        public string strRegToken { get; set; }
        /// <summary>
        /// Provider
        /// </summary>
        public string strLoginProvider { get; set; }
        /// <summary>
        /// Operation Result
        /// </summary>
        public bool isSuccessful { get; set; }
        /// <summary>
        /// Session is Expired
        /// </summary>
        public bool isExpiredSession { get; set; }
        /// <summary>
        /// ReCaptcha token
        /// </summary>
        public string strReCaptchaToken { get; set; }
        /// <summary>
        /// API key ID
        /// </summary>
        public string strApiID { get; set; }
        /// <summary>
        /// Generate a copy
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var objclone = new GigyaData();

            objclone.strEmail = this.strEmail;
            objclone.strPassword = this.strPassword;
            objclone.strData = this.strData;
            objclone.strExtraProfileFieldsDescriptor = this.strExtraProfileFieldsDescriptor;
            objclone.strExtraProfileFields = this.strExtraProfileFields;
            objclone.objSessionCookie = null;
            objclone.strApiID = this.strApiID;



            return objclone;
        }
    }
}
