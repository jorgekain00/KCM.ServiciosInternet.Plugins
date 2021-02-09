/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Plugins.Data.sso.Interfaces
{
    /// <summary>
    /// Constains all Single Sign On Operations
    /// </summary>
    public interface IAccountsREST
    {
        /// <summary>
        /// Request Reset Password via email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object (comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool requestResetPasswordEmail(ISingleSignOnData objData);
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool updatePasswordWithToken(ISingleSignOnData objData);
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool logIn(ISingleSignOnData objData);
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: get account from UID, false: get account from Token</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool getAccountInfo(ISingleSignOnData objData, bool IsWithUID = true);
        /// <summary>
        /// set account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: set account from UID, false: set account from Token</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool setAccountInfo(ISingleSignOnData objData, bool IsWithUID = true);
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool logOut(ISingleSignOnData objData);
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool signUp(ISingleSignOnData objData);
        /// <summary>
        /// Complete registration
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A boolean value indicateting a sucessful (true) or unsucessful result</returns>
        bool finalizeRegistration(ISingleSignOnData objData);
    }
}
