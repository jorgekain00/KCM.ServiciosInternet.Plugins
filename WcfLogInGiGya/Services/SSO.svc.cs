/// <Author>
///     Ing. Jorge Flores Miguel  KCUS/C84818
/// </Author>
/// <CreationDate>
///     January 2021
/// </CreationDate>
/// <summary>
///     Single Sign On Operations (REST API)
/// </summary>
namespace WcfLogInGiGya.Services
{
    using KCM.ServiciosInternet.Common.Library.Log;
    using KCM.ServiciosInternet.Plugins.Business;
    using KCM.ServiciosInternet.Plugins.Data.SSO.Config;
    using KCM.ServiciosInternet.Plugins.Data.SSO.HTML;
    using KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces;
    using System;
    using System.IO;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web;
    public class SSO : ISSORestful
    {
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData changePassword(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objData = BussinessSSO.changePassword(objData, strDataDirectory);
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.changePassword", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.changePassword", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData getAccountInfo(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objData = BussinessSSO.getAccountInfo(objData, strDataDirectory);

                if (objData.isSuccessful)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.SetCookie] = objData.objSessionCookie.Serialize(objData.strApiID, IsDebug: GlobalConfig.IsAppInDebugMode); 
                }
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.getAccountInfo", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.getAccountInfo", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// Load template HTML for the SSO Plug In
        /// </summary>
        /// <param name="objHtmlPlugIn"></param>
        /// <returns></returns>
        public htmlPlugIn getHTMLLogin(htmlPlugIn objHtmlPlugIn)
        {
            string strTemplate = string.Empty;
            string strContent = string.Empty;


            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objHtmlPlugIn = BussinessSSO.getHtml(objHtmlPlugIn, strDataDirectory);
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.getHTMLLogin", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.getHTMLLogin", ex, GlobalConfig.IsAppInDebugMode);
            }

            return objHtmlPlugIn;
        }
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData LogoutSession(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                string strCookieHeader = WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.Cookie];

                objData.objSessionCookie.Deserialize(objData.strApiID, strCookieHeader);

                objData = BussinessSSO.LogoutSession(objData, strDataDirectory);

            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.LogoutSession", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.LogoutSession", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// Request Reset Password via custom email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData requestResetPassword(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objData = BussinessSSO.requestResetPassword(objData, strDataDirectory);
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.requestResetPassword", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.requestResetPassword", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// Get JS script according to the request
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strApiKey">Api ID for www.kcmsso.com_1 DB</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public Stream requestScriptJS(string script, string apiKey)
        {
            string strResponse = string.Empty;
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                string strScriptName = script.Replace("JFMGigya", "");
                strResponse = BussinessSSO.requestScriptJS(strScriptName, apiKey, strDataDirectory);
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.requestScriptJS", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.requestScriptJS", ex, GlobalConfig.IsAppInDebugMode);
            }
            finally
            {
                if (strResponse.Length == 0)
                {
                    strResponse = "console.log(\"JFMGigya" + script + " cannot load\")";
                }
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            byte[] bytesResponse = Encoding.UTF8.GetBytes(strResponse);   // Encode script into a Stream
            return new MemoryStream(bytesResponse);
        }
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData sendCredentialesToLogIn(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objData = BussinessSSO.sendCredentialesToLogIn(objData, strDataDirectory);

                if (objData.isSuccessful)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.SetCookie] = objData.objSessionCookie.Serialize(objData.strApiID, IsDebug: GlobalConfig.IsAppInDebugMode);
                }
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendCredentialesToLogIn", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendCredentialesToLogIn", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData sendCredentialesToSignUp(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                objData = BussinessSSO.sendCredentialesToSignIn(objData, strDataDirectory);

                if (objData.isSuccessful)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.SetCookie] = objData.objSessionCookie.Serialize(objData.strApiID, IsDebug: GlobalConfig.IsAppInDebugMode);
                }
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendCredentialesToSignUp", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendCredentialesToSignUp", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
        /// <summary>
        /// set account info from UID or token mode (Complete registration)
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        public ISingleSignOnData sendMissingFields(ISingleSignOnData objData)
        {
            try
            {
                string strDataDirectory = HttpContext.Current.Server.MapPath(GlobalConfig.strDataPath);

                string strCookieHeader = WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.Cookie];

                objData.objSessionCookie.Deserialize(objData.strApiID, strCookieHeader);

                objData = BussinessSSO.sendMissingFields(objData, strDataDirectory);

                if (objData.isSuccessful)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.SetCookie] = objData.objSessionCookie.Serialize(objData.strApiID, IsDebug: GlobalConfig.IsAppInDebugMode);
                }
            }
            catch (Exception ex)
            {
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendMissingFields", "Exception was encountered...");
                clsEscribirLog.EscribeLog(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), clsEscribirLog.enumTipoMensaje.Excepcion, "WcfLogInGiGya.Services.SSO.sendMissingFields", ex, GlobalConfig.IsAppInDebugMode);
            }
            return objData;
        }
    }
}
