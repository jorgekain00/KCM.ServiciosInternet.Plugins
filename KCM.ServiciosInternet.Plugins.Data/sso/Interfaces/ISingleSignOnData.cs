/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
using System;
using System.Runtime.Serialization;

namespace KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces
{
    /// <summary>
    /// Constains all fields for REST communication
    /// </summary>
    /// <remarks>
    /// <para>Not all fields are necesary for every operation</para>
    /// </remarks>
    public interface ISingleSignOnData : ICloneable
    {
        /// <summary>
        /// Unique Id or Email
        /// </summary>
        [DataMember]
        string strEmail { get; set; }
        /// <summary>
        /// Password 
        /// </summary>
        [DataMember]
        string strPassword { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [DataMember]
        string strUID { get; set; }
        /// <summay>
        /// Profile value-pair fields
        /// </summary>
        [DataMember]
        string strProfile { get; set; }
        /// <summay>
        /// Data value-pair fields
        /// </summary>
        [DataMember]
        string strData { get; set; }
        /// <summary>
        /// List of Extra profile fields Descriptor
        /// </summary>
        [DataMember]
        string strExtraProfileFieldsDescriptor { get; set; }
        /// <summary>
        /// Session cookie 
        /// </summary>
        ISingleSignOnCookie objSessionCookie { get; set; }
        /// <summary>
        /// Is Account Pending Registration due to missing values
        /// </summary>
        [DataMember]
        bool boolIsAccountPendingRegistration { get; set; }
        /// <summary>
        /// Is Account Pending Email verification
        /// </summary>
        [DataMember]
        bool boolIsAccountPendingVerification { get; set; }
        /// <summary>
        /// Error messages
        /// </summary>
        [DataMember]
        string strErrormessage { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        [DataMember]
        string strRegToken { get; set; }
        /// <summary>
        /// ReCaptcha token
        /// </summary>
        [DataMember]
        string strReCaptchaToken { get; set; }
        /// <summary>
        /// Login provider
        /// </summary>
        [DataMember]
        string strLoginProvider { get; set; }
        /// <summary>
        /// Operation Result
        /// </summary>
        [DataMember]
        bool isSuccessful { get; set; }
        /// <summary>
        /// Is session Expired
        /// </summary>
        [DataMember]
        bool isExpiredSession { get; set; }
        /// <summary>
        /// API key ID
        /// </summary>
        [DataMember]
        string strApiID { get; set; }
    }
}
