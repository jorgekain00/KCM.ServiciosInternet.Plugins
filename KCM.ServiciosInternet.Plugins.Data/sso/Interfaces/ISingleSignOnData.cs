/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Plugins.Data.sso.Interfaces
{
    /// <summary>
    /// Constains all fields for REST communication
    /// </summary>
    /// <remarks>
    /// <para>Not all fields are necesary for every operation</para>
    /// </remarks>
    public interface ISingleSignOnData
    {
        /// <summary>
        /// Unique Id or Email
        /// </summary>
        string strEmail { get; set; }
        /// <summary>
        /// Password 
        /// </summary>
        string strPassword { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        string strUID { get; set; }
        /// <summay>
        /// Profile value-pair fields
        /// </summary>
        string strProfile { get; set; }
        /// <summary>
        /// List of Extra profile fields Descriptor
        /// </summary>
        string strExtraProfileFieldsDescriptor { get; set; }
        /// <summary>
        /// List of value-pair Extra fields
        /// </summary>
        string strExtraProfileFields { get; set; }
        /// <summary>
        /// Session cookie 
        /// </summary>
        ISingleSignOnCookie objSessionCookie { get; set; }
        /// <summary>
        /// Is Account Pending Registration due to missing values
        /// </summary>
        bool boolIsAccountPendingRegistrarion { get; set; }
        /// <summary>
        /// Is Account Pending Email verification
        /// </summary>
        bool boolIsAccountPendingVerification { get; set; }
        /// <summary>
        /// Error messages
        /// </summary>
        string strErrormessage { get; set; }
        /// <summary>
        /// Token For registration or other accounts commands
        /// </summary>
        string strRegToken { get; set; }
        /// <summary>
        /// provider
        /// </summary>
        string strProvider { get; set; }
    }
}
