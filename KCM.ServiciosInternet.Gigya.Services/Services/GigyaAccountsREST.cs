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
///     Constains all Single Sign On Operations for Gigya (SAP)
/// </summary>
using Gigya.Socialize.SDK;

namespace KCM.ServiciosInternet.Gigya.Services.Services
{
    using KCM.ServiciosInternet.Gigya.Services.Data;
    using KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    /// <summary>
    /// Constains all Single Sign On Operations for Gigya (SAP)
    /// </summary>
    public class GigyaAccountsREST : IAccountsREST
    {

        /// <summary>
        /// api ID (site in Gigya) like kotex.mx, jaboneskleenex.com, etc.
        /// </summary>
        private readonly string strAPIKey;
        /// <summary>
        /// Secret key for all the sites
        /// </summary>
        private readonly string strAPISecretKey;
        /// <summary>
        /// Custom App Key (dedicated access to gigya)
        /// </summary>
        private readonly string strUserKey;
        /// <summary>
        /// Custom App secret key (dedicated access to gigya)
        /// </summary>
        private readonly string strUserSecretKey;
        /// <summary>
        /// User Interface language
        /// </summary>
        private readonly string strLanguage;
        /// <summary>
        /// Expiration session in minutes (value for the cookie)
        /// </summary>
        private readonly int intExpirationSessionInMins;
        /// <summary>
        /// who provide with the service
        /// </summary>
        private readonly string strProvider;
        /// <summary>
        /// Error Codes
        /// </summary>
        private readonly Dictionary<string, string> objGigyaCodes;
        /// <summary>
        /// Global constructor
        /// </summary>
        /// <param name="strAPIKey"></param>
        /// <param name="strAPISecretKey"></param>
        /// <param name="strUserKey"></param>
        /// <param name="strUserSecretKey"></param>
        /// <param name="objDiCodes"></param>
        /// <param name="intExpirationSessionInMins"></param>
        /// <param name="strProvider"></param>
        /// <param name="strLanguage"></param>
        public GigyaAccountsREST(string strAPIKey, string strAPISecretKey, string strUserKey, string strUserSecretKey, Dictionary<string, string> objDiCodes, int intExpirationSessionInMins, string strProvider, string strLanguage = "en-US")
        {
            this.strAPIKey = strAPIKey;
            this.strAPISecretKey = strAPISecretKey;
            this.strUserKey = strUserKey;
            this.strUserSecretKey = strUserSecretKey;
            this.objGigyaCodes = objDiCodes;
            this.intExpirationSessionInMins = intExpirationSessionInMins;
            this.strLanguage = strLanguage;
        }
        /// <summary>
        /// Contructor values from a dynamic object
        /// </summary>
        /// <param name="objConfig"></param>
        public GigyaAccountsREST(dynamic objConfig)
        {
            this.strAPIKey = objConfig.Provider.keys.APIKey;
            this.strAPISecretKey = objConfig.Provider.keys.APISecretKey;
            this.strUserKey = objConfig.Provider.keys.UserKey;
            this.strUserSecretKey = objConfig.Provider.keys.UserSecretKey;
            this.strProvider = objConfig.Provider.dll;
            this.intExpirationSessionInMins = Convert.ToInt32(objConfig.Settings.Session.ExpirationSessionInMins);
            this.strLanguage = objConfig.Provider.Codes.language;

            this.objGigyaCodes = new Dictionary<string, string>();

            foreach (var itemCode in objConfig.Provider.Codes.code)
            {
                if (itemCode.lang == this.strLanguage)
                {
                    this.objGigyaCodes.Add((string)itemCode.number, (string)itemCode.description);
                }
            }
        }
        /// <summary>
        /// Constructor values from a json file
        /// </summary>
        /// <param name="strJsonFile"></param>
        public GigyaAccountsREST(string strJsonFile)
        {
            string strJson = string.Empty;


            using (FileStream objFs = new FileStream(strJsonFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader objSrd = new StreamReader(objFs))
                {
                    strJson = objSrd.ReadToEnd();
                }
            }


            dynamic objConfig = JsonConvert.DeserializeObject(strJson);
            this.strAPIKey = objConfig.Provider.keys.APIKey;
            this.strAPISecretKey = objConfig.Provider.keys.APISecretKey;
            this.strUserKey = objConfig.Provider.keys.UserKey;
            this.strUserSecretKey = objConfig.Provider.keys.UserSecretKey;
            this.strProvider = objConfig.Provider.dll;
            this.intExpirationSessionInMins = Convert.ToInt32(objConfig.Settings.Session.ExpirationSessionInMins);
            this.strLanguage = objConfig.Provider.Codes.language;

            this.objGigyaCodes = new Dictionary<string, string>();

            foreach (var itemCode in objConfig.Provider.Codes.code)
            {
                if (itemCode.lang == this.strLanguage)
                {
                    this.objGigyaCodes.Add((string)itemCode.number, (string)itemCode.description);
                }
            }
        }
        /// <summary>
        /// Eval Error codes
        /// </summary>
        /// <param name="objGSResponse">GSResponse for gigya interface</param>
        /// <returns>A message Error (string)</returns>
        private string evalCodeGigyaAccountService(GSResponse objGSResponse)
        {
            int intErrorCode = objGSResponse.GetErrorCode();
            string strErrorMessage = string.Empty;

            if (intErrorCode == 400006 || intErrorCode == 400009)
            {
                foreach (var objItem in objGSResponse.GetData().GetArray("validationErrors"))
                {
                    var objErrorCls = new { errorCode = 0, fieldName = "", message = "" };
                    var objErrors = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(objItem.ToString(), objErrorCls);
                    if (objErrors.errorCode != 400027)
                    {
                        if (!this.objGigyaCodes.TryGetValue(objErrors.errorCode.ToString(), out strErrorMessage))
                        {
                            strErrorMessage = objErrors.message;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (!this.objGigyaCodes.TryGetValue(intErrorCode.ToString(), out strErrorMessage))
                {
                    strErrorMessage = objGSResponse.GetData().GetString("errorMessage");
                }
            }

            return strErrorMessage;
        }

        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.logIn(ISingleSignOnData objData)
        {
            GSRequest objGSRequest;
            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.login", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.login", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("loginID", objData.strEmail);
            objGSRequest.SetParam("password", objData.strPassword);

            GSResponse objGSResponse = objGSRequest.Send();
            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0 ||
                intResponseCode == 206001 ||
                intResponseCode == 206002)
            {
                GSObject objGSRes = objGSResponse.GetData();

                objData.strRegToken = string.Empty;
                objData.strUID = string.Empty;
                objData.strProfile = objGSRes.GetString("profile");
                //objData.boolIsAccountPendingRegistration = (intResponseCode == 206001) ? true : false;
                //objData.boolIsAccountPendingVerification = (intResponseCode == 206002) ? true : false;
                objData.boolIsAccountPendingRegistration = Convert.ToBoolean(objGSRes.GetString("isRegistered"));
                objData.boolIsAccountPendingVerification = Convert.ToBoolean(objGSRes.GetString("isVerified"));
                objData.strLoginProvider = objGSRes.GetString("loginProvider");

                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objGSRes.GetString("UID"),
                    strRegToken = (intResponseCode == 206001) ? objGSRes.GetString("regToken") : null,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = (ISingleSignOnCookie)objSessionCookie;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;      //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;       //UNSUCCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: get account from UID, false: get account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.getAccountInfo(ISingleSignOnData objData, bool IsWithUID)
        {
            GSRequest objGSRequest;

            int intResponseCode = 0;
            string strToken = objData.objSessionCookie.strRegToken;  // backup token

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.getAccountInfo", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.getAccountInfo", null, true, this.strUserKey);
            }

            if (IsWithUID)
            {
                objGSRequest.SetParam("UID", objData.objSessionCookie.strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", strToken);
            }

            objGSRequest.SetParam("include", "loginIDs, profile, data, lastLoginLocation");

            if (!string.IsNullOrEmpty(objData.strExtraProfileFieldsDescriptor))
            {
                objGSRequest.SetParam("extraProfileFields", objData.strExtraProfileFieldsDescriptor);
            }

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                GSObject objGSRes = objGSResponse.GetData();

                objData.strEmail = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(
                    objGSRes.GetString("profile"), new { email = string.Empty }).email;
                objData.strUID = objGSRes.GetString("UID");
                objData.strProfile = objGSRes.GetString("profile");
                objData.strData = objGSRes.GetString("data");
                objData.boolIsAccountPendingRegistration = Convert.ToBoolean(objGSRes.GetString("isRegistered"));
                objData.boolIsAccountPendingVerification = Convert.ToBoolean(objGSRes.GetString("isVerified"));
                objData.strLoginProvider = objGSRes.GetString("loginProvider");

                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objData.strUID,
                    strRegToken = strToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;          //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;          //UNSUCCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// set account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="IsWithUID">true: set account from UID, false: set account from Token</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.setAccountInfo(ISingleSignOnData objData, bool IsWithUID)
        {

            GSRequest objGSRequest;

            int intResponseCode = 0;
            string strUID = objData.objSessionCookie.strUID;  // backup uid
            string strToken = objData.objSessionCookie.strRegToken;  // backup token

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.setAccountInfo", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.setAccountInfo", null, true, this.strUserKey);
            }

            if (IsWithUID)
            {
                objGSRequest.SetParam("UID", strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", strToken);
            }

            objGSRequest.SetParam("profile", objData.strProfile);
            objGSRequest.SetParam("data", objData.strData);


            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = strUID,
                    strRegToken = strToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;        //UNSUCCESSFUL;
            }
            return objData;
        }
        /// <summary>
        /// Complete registration
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.finalizeRegistration(ISingleSignOnData objData)
        {

            GSRequest objGSRequest;

            int intResponseCode = 0;
            string strUID = objData.objSessionCookie.strUID;  // backup uid

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.finalizeRegistration", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.finalizeRegistration", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("regToken", objData.objSessionCookie.strRegToken);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = strUID,
                    strRegToken = string.Empty,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;        //UNSUCCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.logOut(ISingleSignOnData objData)
        {
            GSRequest objGSRequest;

            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.logout", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.logout", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("UID", objData.objSessionCookie.strUID);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                objData.objSessionCookie = null;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;        //UNSUCCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.signUp(ISingleSignOnData objData)
        {
            GSRequest objGSRequest;

            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.register", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.register", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("email", objData.strEmail);
            objGSRequest.SetParam("password", objData.strPassword);
            objGSRequest.SetParam("finalizeRegistration", true);
            objGSRequest.SetParam("data", objData.strData);
            objGSRequest.SetParam("profile", objData.strProfile);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                GSObject objGSRes = objGSResponse.GetData();

                //objData.boolIsAccountPendingRegistration = (objGSResponse.GetErrorCode() == 206001) ? true : false;
                objData.strProfile = objGSRes.GetString("profile");
                objData.strData = objGSRes.GetString("data");
                objData.boolIsAccountPendingRegistration = Convert.ToBoolean(objGSRes.GetString("isRegistered"));
                objData.boolIsAccountPendingVerification = Convert.ToBoolean(objGSRes.GetString("isVerified"));
                objData.strLoginProvider = objGSRes.GetString("loginProvider");

                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objGSRes.GetString("UID"),
                    strRegToken = (objGSResponse.GetErrorCode() == 206001) ? objGSRes.GetString("regToken") : null,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;        //UNSUCCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// Request Reset Password via email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object (comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.requestResetPasswordEmail(ISingleSignOnData objData)
        {
            GSRequest objGSRequest;
            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.resetPassword", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.resetPassword", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("loginID", objData.strEmail);
            objGSRequest.SetParam("sendEmail", false);

            GSResponse objGSResponse = objGSRequest.Send();
            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                objData.strRegToken = objGSResponse.GetData().GetString("passwordResetToken");
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;       //UNSUCESSFUL;
            }

            return objData;
        }
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        ISingleSignOnData IAccountsREST.updatePasswordWithToken(ISingleSignOnData objData)
        {
            GSRequest objGSRequest;
            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.resetPassword", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.resetPassword", null, true, this.strUserKey);
            }


            objGSRequest.SetParam("loginID", objData.strEmail);
            objGSRequest.SetParam("passwordResetToken", objData.strRegToken);
            objGSRequest.SetParam("newPassword", objData.strPassword);

            GSResponse objGSResponse = objGSRequest.Send();
            intResponseCode = objGSResponse.GetErrorCode();

            objData = objData.Clone() as ISingleSignOnData;

            if (intResponseCode == 0)
            {
                objData.strErrormessage = string.Empty;
                objData.isSuccessful = true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.isSuccessful = false;       //UNSUCESSFUL;
            }

            return objData;
        }
      
        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Elimina el estado administrado (objetos administrados).
                }

                // libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        ~GigyaAccountsREST()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
