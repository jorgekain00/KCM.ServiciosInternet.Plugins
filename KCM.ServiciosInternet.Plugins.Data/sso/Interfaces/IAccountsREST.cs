/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
using System;

namespace KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces
{
    /// <summary>
    /// Constains all Single Sign On Operations
    /// </summary>
    public interface IAccountsREST : IDisposable
    {
        /// <summary>
        /// Request Reset Password via email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object (comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData requestResetPasswordEmail(ISingleSignOnData objData);
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData updatePasswordWithToken(ISingleSignOnData objData);
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData logIn(ISingleSignOnData objData);
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: get account from UID, false: get account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData getAccountInfo(ISingleSignOnData objData, bool IsWithUID = false);
        /// <summary>
        /// set account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: set account from UID, false: set account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData setAccountInfo(ISingleSignOnData objData, bool IsWithUID = false);
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData logOut(ISingleSignOnData objData);
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData signUp(ISingleSignOnData objData);
        /// <summary>
        /// Complete registration
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData finalizeRegistration(ISingleSignOnData objData);
    }
}
