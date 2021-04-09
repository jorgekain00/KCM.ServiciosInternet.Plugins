/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/

using Gigya.Socialize.SDK;

namespace KCM.ServiciosInternet.Gigya.Services.Services
{
    using KCM.ServiciosInternet.Gigya.Services.Data;
    using KCM.ServiciosInternet.Plugins.Data.sso.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    /// <summary>
    /// Constains all Single Sign On Operations for Gigya (SAP)
    /// </summary>
    internal class GigyaAccountsREST : IAccountsREST
    {

        private readonly string strAPIKey;
        private readonly string strAPISecretKey;
        private readonly string strUserKey;
        private readonly string strUserSecretKey;
        private readonly string strLanguage;
        private readonly int intExpirationSessionInMins;
        private readonly string strProvider;
        private readonly Dictionary<string, string> objGigyaCodes;

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
        bool IAccountsREST.requestResetPasswordEmail(ISingleSignOnData objData)
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

            if (intResponseCode == 0)
            {
                objData.strRegToken = objGSResponse.GetData().GetString("passwordResetToken");
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;       //UNSUCESSFUL;
            }
        }
        bool IAccountsREST.updatePasswordWithToken(ISingleSignOnData objData)
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

            if (intResponseCode == 0)
            {
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;       //UNSUCESSFUL;
            }
        }

        bool IAccountsREST.logIn(ISingleSignOnData objData)
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

            if (intResponseCode == 0 ||
                intResponseCode == 206001 ||
                intResponseCode == 206002)
            {
                GSObject objGSRes = objGSResponse.GetData();

                objData.strRegToken = (intResponseCode == 206001) ? objGSRes.GetString("regToken") : null;
                //objData.boolIsAccountPendingRegistration = (intResponseCode == 206001) ? true : false;
                //objData.boolIsAccountPendingVerification = (intResponseCode == 206002) ? true : false;
                objData.strUID = objGSRes.GetString("UID");
                objData.strProfile = objGSRes.GetString("profile");
                objData.boolIsAccountPendingRegistration = Convert.ToBoolean(objGSRes.GetString("isRegistered"));
                objData.boolIsAccountPendingVerification = Convert.ToBoolean(objGSRes.GetString("isVerified"));
                objData.strLoginProvider = objGSRes.GetString("loginProvider");

                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objData.strUID,
                    strRegToken = objData.strRegToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
        }

        bool IAccountsREST.getAccountInfo(ISingleSignOnData objData, bool IsWithUID)
        {
            GSRequest objGSRequest;

            int intResponseCode = 0;

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
                objGSRequest.SetParam("UID", objData.strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", objData.strRegToken);
            }

            objGSRequest.SetParam("include", "loginIDs, profile, data, lastLoginLocation");

            if (!string.IsNullOrEmpty(objData.strExtraProfileFieldsDescriptor))
            {
                objGSRequest.SetParam("extraProfileFields", objData.strExtraProfileFieldsDescriptor);
            }

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

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
                    strRegToken = objData.strRegToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
        }

        bool IAccountsREST.setAccountInfo(ISingleSignOnData objData, bool IsWithUID)
        {

            GSRequest objGSRequest;

            int intResponseCode = 0;

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
                objGSRequest.SetParam("UID", objData.strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", objData.strRegToken);
            }

            objGSRequest.SetParam("profile", objData.strProfile);
            objGSRequest.SetParam("data", objData.strData);


            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            if (intResponseCode == 0)
            {
                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objData.strUID,
                    strRegToken = objData.strRegToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
        }

        bool IAccountsREST.finalizeRegistration(ISingleSignOnData objData)
        {

            GSRequest objGSRequest;

            int intResponseCode = 0;

            if (string.IsNullOrEmpty(this.strUserKey) || string.IsNullOrEmpty(this.strUserSecretKey))
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.finalizeRegistration", true);
            }
            else
            {
                objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.finalizeRegistration", null, true, this.strUserKey);
            }

            objGSRequest.SetParam("regToken", objData.strRegToken);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            if (intResponseCode == 0)
            {
                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objData.strUID,
                    strRegToken = objData.strRegToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
        }

        bool IAccountsREST.logOut(ISingleSignOnData objData)
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

            objGSRequest.SetParam("UID", objData.strUID);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            if (intResponseCode == 0)
            {
                objData.objSessionCookie = null;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
        }

        bool IAccountsREST.signUp(ISingleSignOnData objData)
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

            bool boolResult = false;

            objGSRequest.SetParam("email", objData.strEmail);
            objGSRequest.SetParam("password", objData.strPassword);
            objGSRequest.SetParam("finalizeRegistration", true);
            objGSRequest.SetParam("data", objData.strData);
            objGSRequest.SetParam("profile", objData.strProfile);

            GSResponse objGSResponse = objGSRequest.Send();

            intResponseCode = objGSResponse.GetErrorCode();

            if (intResponseCode == 0)
            {
                GSObject objGSRes = objGSResponse.GetData();

                objData.strRegToken = (objGSResponse.GetErrorCode() == 206001) ? objGSRes.GetString("regToken") : null;
                objData.boolIsAccountPendingRegistration = (objGSResponse.GetErrorCode() == 206001) ? true : false;
                objData.strUID = objGSRes.GetString("UID");
                objData.strProfile = objGSRes.GetString("profile");
                objData.strData = objGSRes.GetString("data");
                objData.boolIsAccountPendingRegistration = Convert.ToBoolean(objGSRes.GetString("isRegistered"));
                objData.boolIsAccountPendingVerification = Convert.ToBoolean(objGSRes.GetString("isVerified"));
                objData.strLoginProvider = objGSRes.GetString("loginProvider");

                GigyaCookie objSessionCookie = new GigyaCookie
                {
                    strUID = objData.strUID,
                    strRegToken = objData.strRegToken,
                    dtLastAccess = DateTime.Now,
                    intExpirationSessionInMins = this.intExpirationSessionInMins
                };
                objData.objSessionCookie = objSessionCookie;
                objData.strErrormessage = string.Empty;
                return true;        //SUCCESSFUL;
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                return false;        //UNSUCCESSFUL;
            }
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
