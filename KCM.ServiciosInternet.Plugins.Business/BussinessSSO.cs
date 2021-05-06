/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///     Main Logic for SSO
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Business
{
    using System;
    using KCM.ServiciosInternet.Plugins.Business.LogicaSSO;
    using KCM.ServiciosInternet.Plugins.Data.SSO.HTML;
    using KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces;

    public static class BussinessSSO
    {
        #region SSO operations
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData sendCredentialesToLogIn(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.sendCredentialesToLogIn(objData);
            }
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData sendCredentialesToSignIn(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.sendCredentialesToSignIn(objData);
            }
        }
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData getAccountInfo(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.getAccountInfo(objData);
            }
        }
        /// <summary>
        /// set account info from UID or token mode (Complete registration)
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData sendMissingFields(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.sendMissingFields(objData);
            }
        }
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData LogoutSession(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.LogoutSession(objData);
            }
        }
        /// <summary>
        /// Get html template for SSO operations
        /// </summary>
        /// <param name="objHtmlPlugIn">htmlPlugIn object</param>
        /// <param name="strDataDirectory">Data Folder</param>
        /// <returns>htmlPlugIn object with html template</returns>
        public static htmlPlugIn getHtml(htmlPlugIn objHtmlPlugIn, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objHtmlPlugIn.strApiID, strDataDirectory))
            {
                return objSSO.geHtml(objHtmlPlugIn);
            }
        }
        /// <summary>
        /// Request Reset Password via custom email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData requestResetPassword(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.requestResetPassword(objData);
            }
        }
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static ISingleSignOnData changePassword(ISingleSignOnData objData, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(objData.strApiID, strDataDirectory))
            {
                return objSSO.changePassword(objData);
            }
        }
        /// <summary>
        /// Get JS script according to the request
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strApiKey">Api ID for www.kcmsso.com_1 DB</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public static string requestScriptJS(string strScriptName, string strApiKey, string strDataDirectory)
        {
            using (SSO objSSO = new SSO(strApiKey, strDataDirectory))
            {
                return objSSO.requestScriptJS(strScriptName);
            }
        }
        #endregion
    }
}
