/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <email>
///     jorgekain00@gmail.com
/// </email>
/// <summary>
///     Business Rules for the SSO system (only for Login, singin, Reset password, logout operations)
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Business.LogicaSSO
{
    using KCM.ServiciosInternet.Common.Library.Log;
    using KCM.ServiciosInternet.Plugins.Data.SSO.HTML;
    using KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using Gigya = KCM.ServiciosInternet.Gigya.Services.Services;
    using Google = KCM.ServiciosInternet.Google.Services;
    using Media = KCM.ServiciosInternet.Plugins.Media;
    using SSOConfig = KCM.ServiciosInternet.Plugins.Data.SSO.Config;

    internal class SSO : IDisposable
    {
        #region private variables
        /// <summary>
        /// Api ID for www.kcmsso.com_1 DB
        /// </summary>
        private string strApiKey;
        /// <summary>
        /// config api values from a JSON file 
        /// </summary>
        private SSOConfig.SSOConfig.Root objSSOConfig;
        /// <summary>
        /// Starting datetime
        /// </summary>
        private string strDate;
        /// <summary>
        /// Data Folder (get the api resources)
        /// </summary>
        private string strDataDirectory;
        /// <summary>
        /// SSO object
        /// </summary>
        private IAccountsREST objAccountREST;
        /// <summary>
        /// Mail template path for Password Reset
        /// </summary>
        private string strResetPSMailPath;
        /// <summary>
        /// Web Service URL
        /// </summary>
        private string strServiceUrl;
        /// <summary>
        /// Reset Password Form (aspx)
        /// </summary>
        private string strResetPSUrl;
        /// <summary>
        /// Returning Site after changing the password
        /// </summary>
        private string strSite;
        /// <summary>
        /// Mail Subject for Reset Password
        /// </summary>
        private string strResetPSMailSubject;
        /// <summary>
        /// Reset PS MAIL from
        /// </summary>
        private string strResetPSFromMail;
        /// <summary>
        /// Scripts Library
        /// </summary>
        private string strScriptsLibPath;
        /// <summary>
        /// Main Folder where resources are saved
        /// </summary>
        private string strMainFolder;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializing values
        /// </summary>
        /// <param name="strApiKey">Api ID for www.kcmsso.com_1 DB</param>
        /// <param name="strDataDirectory">Data Folder (get the api resources)</param>
        public SSO(string strApiKey, string strDataDirectory)
        {
            System.Configuration.AppSettingsReader objAppSettings = new System.Configuration.AppSettingsReader();  // Get object to retrieve keys from config appSettings
            Dictionary<string, string> dcResponseCodes = new Dictionary<string, string>();

            this.strDate = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");   // key for loggin porpuses

            this.strDataDirectory = strDataDirectory;  // get physical Data path
            this.strMainFolder = Path.Combine(strDataDirectory, strApiKey); // Main Folder where resources are saved

            // Get appSettings values
            this.strResetPSUrl = SSOConfig.GlobalConfig.strResetPSUrl;
            this.strServiceUrl = SSOConfig.GlobalConfig.strServiceUrl;
            this.strScriptsLibPath = SSOConfig.GlobalConfig.strScriptsLibPath;

            this.strApiKey = strApiKey;

            // Get site config from a json file
            this.objSSOConfig = Tools.SiteConfig.getSiteConfig(Path.Combine(this.strMainFolder,                 Media.BusinessMediaSSO.getConfigPath(this.strApiKey)));

            // get the path for the mail template
            this.strResetPSMailPath = this.objSSOConfig.settings.site.templates.mailPath;
            // get url client site
            this.strSite = this.objSSOConfig.settings.site.Url;

            // Get mailsubject
            this.strResetPSMailSubject = this.objSSOConfig.settings.site.resetPassword.mail.resetPSMailSubject;
            this.strResetPSFromMail = this.objSSOConfig.settings.site.resetPassword.mail.fromMail;

            // Config the Gigya Service
            this.objAccountREST = new Gigya.GigyaAccountsREST(
                    strAPIKey: strApiKey,
                    strAPISecretKey: this.objSSOConfig.provider.keys.aPISecretKey,
                    strUserKey: this.objSSOConfig.provider.keys.userKey,
                    strUserSecretKey: this.objSSOConfig.provider.keys.userSecretKey,
                    objDiCodes: (from c in this.objSSOConfig.provider.codes.code where c.lang == this.objSSOConfig.provider.codes.language select c).ToDictionary((c) => c.number, (c) => c.description),
                    intExpirationSessionInMins: this.objSSOConfig.settings.session.expirationSessionInMins,
                    strProvider: this.objSSOConfig.provider.dll,
                    strLanguage: this.objSSOConfig.provider.codes.language
                );
        }
        #endregion

        #region private methods
        /// <summary>
        /// is ReCaptcha expired?
        /// </summary>
        /// <param name="strCaptchaToken">Token to evaluate</param>
        /// <param name="isLoginFlow">To get the appropiate keys</param>
        /// <returns></returns>        
        private bool isExpiredReCaptcha(string strCaptchaToken, bool isLoginFlow = true)
        {
            if (isLoginFlow)
            {
                return Google.BussinessGoogle.isExpiredReCaptcha(
                    strCatchapSecretKey: this.objSSOConfig.settings.reCAPTCHA.loginFlow.secretKeyVc,
                    strCaptchaToken: strCaptchaToken,
                    strProviderUrlValidator: this.objSSOConfig.settings.reCAPTCHA.loginFlow.providerUrlValidatorVc
                    );
            }
            else
            {
                return Google.BussinessGoogle.isExpiredReCaptcha(
                    strCatchapSecretKey: this.objSSOConfig.settings.reCAPTCHA.registrationFlow.secretKeyVc,
                    strCaptchaToken: strCaptchaToken,
                    strProviderUrlValidator: this.objSSOConfig.settings.reCAPTCHA.registrationFlow.providerUrlValidatorVc
                    );
            }
        }
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="strEmail">Sended to</param>
        /// <param name="strFrom">Received from</param>
        /// <param name="strSubject">Email subject</param>
        /// <param name="strBody">Content for the message</param>
        /// <returns></returns>
        /// <remarks>
        ///     Get values from Config file
        ///       <system.net>
        ///         <mailSettings>
        ///             <smtp deliveryMethod = "Network" from="No-reply &lt;no-reply@kcc.com&gt;">
        ///                 <network host = "mailhost.gwsweb.net" />
        ///             </smtp >
        ///         </mailSettings >
        ///     </system.net >
        /// </remarks>
        private bool sendEmailToUser(string strEmail, string strFrom, string strSubject, string strBody)
        {
            MailMessage message = new MailMessage();

            message.To.Add(strEmail); // Email-ID of Receiver  

            message.Subject = strSubject;
            message.From = new System.Net.Mail.MailAddress(strFrom);// Email-ID of Sender  
            message.IsBodyHtml = true;
            message.Body = strBody;
            message.BodyEncoding = Encoding.Default;
            message.Priority = MailPriority.High;

            SmtpClient SmtpMail = new SmtpClient();
            SmtpMail.UseDefaultCredentials = true;
            SmtpMail.Send(message); //Smtpclient to send the mail message 

            return true;
        }
        #endregion

        #region SSO operations
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData sendCredentialesToLogIn(ISingleSignOnData objData)
        {
            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendCredentialesToLogIn", string.Format("Initiating Login for '{0}'", objData.strEmail));

            if (this.objSSOConfig.settings.reCAPTCHA.isDisableReCAPTCHA || !this.isExpiredReCaptcha(objData.strReCaptchaToken))
            {
                objData = objAccountREST.logIn(objData);            // Login porpuses

                if (!objSSOConfig.provider.codes.messages.login.isEnableMessage)
                {
                    objData.strErrormessage = objSSOConfig.provider.codes.messages.login.genericMessage;
                }
            }
            else
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.reCAPTCHA.genericMessage;
                objData.isSuccessful = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendCredentialesToLogIn", string.Format("Ending Login for '{0}', Successful '{1}' - '{2}'", objData.strEmail, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData sendCredentialesToSignIn(ISingleSignOnData objData)
        {

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendCredentialesToSignIn", string.Format("Initiating SignIn for '{0}'", objData.strEmail));

            if (this.objSSOConfig.settings.reCAPTCHA.isDisableReCAPTCHA || !this.isExpiredReCaptcha(objData.strReCaptchaToken, isLoginFlow: false))
            {
                objData = objAccountREST.signUp(objData);       // Sign up porpuses

                if (!objSSOConfig.provider.codes.messages.singIn.isEnableMessage)
                {
                    objData.strErrormessage = objSSOConfig.provider.codes.messages.singIn.genericMessage;
                }
            }
            else
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.reCAPTCHA.genericMessage;
                objData.isSuccessful = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendCredentialesToSignIn", string.Format("Ending SignIn for '{0}', Successful '{1}' - '{2}'", objData.strEmail, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: get account from UID, false: get account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData getAccountInfo(ISingleSignOnData objData)
        {
            bool IsWithUID = (objData.objSessionCookie == null) ? false : string.IsNullOrEmpty(objData.objSessionCookie.strUID) ? false : true;        // Get account info by UID or by token

            // variables for logging porpuses
            string strKey = IsWithUID ? "UID" : "RegToken";
            string strValue = (objData.objSessionCookie == null) ? "" : IsWithUID ? objData.objSessionCookie.strUID : objData.objSessionCookie.strRegToken;

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.getAccountInfo", string.Format("Initiating get Account Info '{0}' : '{1}'", strKey, strValue));

            if (objData.objSessionCookie == null)
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.session.sessionExpiredMessage;
                objData.isSuccessful = false;
                objData.isExpiredSession = true;
            }
            else
            {
                objData = objAccountREST.getAccountInfo(objData, IsWithUID);       // get Account info
                objData.isExpiredSession = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.getAccountInfo", string.Format("Ending get Account Info for '{0}' : '{1}, Successful '{2}' - '{3}'", strKey, strValue, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// set account info from UID or token mode (Complete registration)
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: set account from UID, false: set account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData sendMissingFields(ISingleSignOnData objData)
        {

            bool IsWithUID = (objData.objSessionCookie == null) ? false : string.IsNullOrEmpty(objData.objSessionCookie.strUID) ? false : true;        // Get account info by UID or by token

            // variables for logging porpuses
            string strKey = IsWithUID ? "UID" : "RegToken";
            string strValue = (objData.objSessionCookie == null) ? "" : IsWithUID ? objData.objSessionCookie.strUID : objData.objSessionCookie.strRegToken;

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendMissingFields", string.Format("Initiating Set Account Info '{0}' : '{1}'", strKey, strValue));

            if (this.objSSOConfig.settings.reCAPTCHA.isDisableReCAPTCHA || !this.isExpiredReCaptcha(objData.strReCaptchaToken, isLoginFlow: false))
            {
                if (objData.objSessionCookie == null)
                {
                    objData = (ISingleSignOnData)objData.Clone();
                    objData.strErrormessage = this.objSSOConfig.settings.session.sessionExpiredMessage;
                    objData.isSuccessful = false;
                    objData.isExpiredSession = true;
                }
                else
                {
                    objData = objAccountREST.setAccountInfo(objData);       // set account by token

                    if (objData.isSuccessful)
                    {
                        objAccountREST.finalizeRegistration(objData);       // Finalize registration by token
                    }

                    if (!objSSOConfig.provider.codes.messages.singIn.isEnableMessage)
                    {
                        objData.strErrormessage = objSSOConfig.provider.codes.messages.singIn.genericMessage;
                    }
                    objData.isExpiredSession = false;
                }
            }
            else
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.reCAPTCHA.genericMessage;
                objData.isSuccessful = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.sendMissingFields", string.Format("Ending Set Account Info for '{0}' : '{1}, Successful '{2}' - '{3}'", strKey, strValue, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData LogoutSession(ISingleSignOnData objData)
        {
            bool IsWithUID = (objData.objSessionCookie == null) ? false : string.IsNullOrEmpty(objData.objSessionCookie.strUID) ? false : true;        // Get account info by UID or by token

            // variables for logging porpuses
            string strKey = IsWithUID ? "UID" : "RegToken";
            string strValue = (objData.objSessionCookie == null) ? "" : IsWithUID ? objData.objSessionCookie.strUID : objData.objSessionCookie.strRegToken;

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.LogoutSession", string.Format("Initiating Log out for '{0}' : '{1}'", strKey, strValue));

            if (objData.objSessionCookie == null)
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.session.sessionExpiredMessage;
                objData.isSuccessful = false;
                objData.isExpiredSession = true;
            }
            else
            {
                objData = objAccountREST.logOut(objData);       // set account by token
                objData.isExpiredSession = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.LogoutSession", string.Format("Ending Log out for '{0}' : '{1}, Successful '{2}' - '{3}'", strKey, strValue, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// Request Reset Password via custom email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object (comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData requestResetPassword(ISingleSignOnData objData)
        {

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestResetPassword", string.Format("Initiating Reset Password for '{0}'", objData.strEmail));

            if (this.objSSOConfig.settings.reCAPTCHA.isDisableReCAPTCHA || !this.isExpiredReCaptcha(objData.strReCaptchaToken))
            {
                objData = objAccountREST.requestResetPasswordEmail(objData);            // Request token for reset password

                if (objData.isSuccessful)
                {
                    string strUrlResetPS = string.Format("{0}?regToken={1}&email={2}&apiKey={3}", this.strResetPSUrl, objData.strRegToken, objData.strEmail, this.strApiKey);

                    string strMailPath = Path.Combine(this.strMainFolder, strResetPSMailPath);

                    string strBody = File.ReadAllText(strMailPath).Replace("${|urlResertPS|}", strUrlResetPS); // Load mail template and set Password reset URL


                    clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestResetPassword", string.Format("Sending Email for '{0}'", objData.strEmail));


                    this.sendEmailToUser(strEmail: objData.strEmail,
                                         strFrom: this.strResetPSFromMail,
                                         strSubject: this.strResetPSMailSubject,
                                         strBody: strBody);

                    clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestResetPassword", string.Format("Email Sended for '{0}'", objData.strEmail));
                }

                if (!objSSOConfig.provider.codes.messages.resetPassword.isEnableMessage)
                {
                    objData.strErrormessage = objSSOConfig.provider.codes.messages.resetPassword.genericMessage;
                }
            }
            else
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.reCAPTCHA.genericMessage;
                objData.isSuccessful = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestResetPassword", string.Format("Ending Reset Password '{0}', Successful '{1}' - '{2}'", objData.strEmail, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        internal ISingleSignOnData changePassword(ISingleSignOnData objData)
        {
            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.changePassword", string.Format("Changing Password for '{0}'", objData.strEmail));

            if (this.objSSOConfig.settings.reCAPTCHA.isDisableReCAPTCHA || !this.isExpiredReCaptcha(objData.strReCaptchaToken))
            {
                objData = objAccountREST.updatePasswordWithToken(objData);            // Request token for reset password
            }
            else
            {
                objData = (ISingleSignOnData)objData.Clone();
                objData.strErrormessage = this.objSSOConfig.settings.reCAPTCHA.genericMessage;
                objData.isSuccessful = false;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.changePassword", string.Format("Password is changed for '{0}', Successful '{1}' - '{2}'", objData.strEmail, objData.isSuccessful, objData.strErrormessage));

            return objData;
        }
        /// <summary>
        ///     Get JS script according to the request
        /// </summary>
        /// <param name="strScriptName">Script name</param>
        /// <returns>Return JS content with replaced values</returns>
        internal string requestScriptJS(string strScriptName)
        {

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestScriptJS", string.Format("Requested script '{0}.js'", strScriptName));

            string strScript = string.Empty;   // script function
            try
            {
                //TODO : get script info for DB like paths and replacemente values (catalog scripts)

                // Get JS Script
                // strApiKey for future DB implementations
                strScript = File.ReadAllText(Path.Combine(this.strScriptsLibPath, strScriptName) + ".js");

                // Replace placeholder with values
                switch (strScriptName)
                {
                    case "SendResetPSEmail":
                    case "ChangePs":
                        strScript = strScript
                            .Replace("${|ServiceName|}", SSOConfig.GlobalConfig.strServiceUrl);
                            //.Replace("${|UrlImg|}", Entities.Config.PlugInConfig.strLoadingImage);
                        break;
                    default:
                        break;
                }

            }
            catch (System.IO.FileNotFoundException)
            {
                strScript = "";
            }
            catch (Exception ex)
            {
                throw;
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.requestScriptJS", string.Format("Requested script '{0}.js' - size '{1}'", strScriptName, strScript.Length));

            return strScript;
        }
        /// <summary>
        /// Get html template for SSO operations
        /// </summary>
        /// <param name="objHtmlPlugIn">htmlPlugIn object</param>
        /// <returns>htmlPlugIn object with html template</returns>
        internal htmlPlugIn geHtml(htmlPlugIn objHtmlPlugIn)
        {
            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.geHtml", string.Format("Get template for '{0}'", objHtmlPlugIn.enumState.ToString()));

            switch (objHtmlPlugIn.enumState)
            {
                case htmlPlugIn.templateCtrl.LOAD:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.loadPath));
                    break;
                case htmlPlugIn.templateCtrl.LOGIN:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.loginPath));
                    break;
                case htmlPlugIn.templateCtrl.SIGNIN:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.signInPath));
                    break;
                case htmlPlugIn.templateCtrl.CONTINUE:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.continuePath));
                    break;
                case htmlPlugIn.templateCtrl.COMPLETION:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.completionPath));
                    break;
                case htmlPlugIn.templateCtrl.RESETPS:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.resetPath));
                    break;
                case htmlPlugIn.templateCtrl.RESETFORM:
                    objHtmlPlugIn.strHtml = File.ReadAllText(Path.Combine(this.strMainFolder, this.objSSOConfig.settings.site.templates.resetFormPath));
                    break;
                default:
                    throw new Exception("Template '{0}' not found");
            }

            clsEscribirLog.EscribeDebug(this.strDate, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Plugins.Business.SSO.geHtml", string.Format("Template obtained for '{0}'", objHtmlPlugIn.enumState.ToString()));

            return objHtmlPlugIn;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.objAccountREST.Dispose();
                    this.objSSOConfig = null;
                }

                disposedValue = true;
            }
        }

        ~SSO()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
