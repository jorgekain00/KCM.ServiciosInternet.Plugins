using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gigya.Socialize.SDK;
using Newtonsoft.Json;
using KCM.ServiciosInternet.Gigya.Services.Data;
using KCM.ServiciosInternet.Plugins.Data.sso.Interfaces;

namespace KCM.ServiciosInternet.Gigya.Services.Services
{
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

        public GigyaAccountsREST(string strAPIKey, string strAPISecretKey, string strUserKey, string strUserSecretKey, Dictionary<string, string> objDiCodes, int intExpirationSessionInMins,string strProvider, string strLanguage = "en-US")
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
            this.intExpirationSessionInMins =  Convert.ToInt32(objConfig.Settings.Session.ExpirationSessionInMins);
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
        public bool requestResetPasswordEmail(ISingleSignOnData objData)
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
        public bool updatePasswordWithToken(ISingleSignOnData objData)
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

        public bool logIn(ISingleSignOnData objData)
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
                objData.boolIsAccountPendingRegistrarion = (intResponseCode == 206001) ? true : false;
                objData.boolIsAccountPendingVerification = (intResponseCode == 206002) ? true : false;
                objData.strUID = objGSRes.GetString("UID");
                objData.strProfile = objGSRes.GetString("profile");

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

        public bool getAccountInfo(ISingleSignOnData objData, bool IsWithUID = true)
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
                // TODO : JFM adecuar al proceso 
                // TODO : devolver cookie actualizada GigyaCookie
                GSObject objGSRes = objGSResponse.GetData();

                objData.strEmail = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(
                    objGSRes.GetString("profile"), new { email = string.Empty }).email;
                objData.strUID = objGSRes.GetString("UID");
                objData.strProfile = objGSRes.GetString("profile");

                if (objLogIn.strExtraProfileFieldsDescriptor != "")
                {
                    objLogIn.strExtraProfileFields = objGSRes.GetString("extraProfileFields");
                }

                objLogIn.boolIsAccountPendingRegistration = !objGSRes.GetBool("isRegistered");

                objLogIn.boolIsInvalidRequest = false;
                objLogIn.strErrormessage = "OK";
                objLogIn.boolIsCompletedOperation = true;

                boolResult = true;  // Sucessful login
            }
            else
            {
                evalCodeAccounts(objLogIn, objGSResponse);
            }

            return boolResult;
        }

        public bool setAccountInfo(ISingleSignOnData objData, bool IsWithUID = true)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.setAccountInfo", true);

            if (IsWithUID)
            {
                objGSRequest.SetParam("UID", objLogIn.strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", objLogIn.strRegToken);
            }

            objGSRequest.SetParam("profile", objLogIn.strProfile);
            //objGSRequest.SetParam("extraProfileFields", objLogIn.strExtraProfileFields);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                objLogIn.boolIsAccountPendingRegistration = (objGSResponse.GetErrorCode() == 206001) ? true : false;

                objLogIn.boolIsInvalidRequest = false;
                objLogIn.strErrormessage = "OK";
                objLogIn.boolIsCompletedOperation = true;

                boolResult = true;  // Sucessful login
            }
            else
            {
                evalCodeAccounts(objLogIn, objGSResponse);
            }

            return boolResult;
        }

        public bool finalizeRegistration(ISingleSignOnData objData)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.finalizeRegistration", true);

            objGSRequest.SetParam("regToken", objLogIn.strRegToken);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                GSObject objGSRes = objGSResponse.GetData();
                objLogIn.boolIsAccountPendingRegistration = false;
                objLogIn.boolIsInvalidRequest = false;
                objLogIn.strErrormessage = "OK";
                objLogIn.boolIsCompletedOperation = true;
                objLogIn.strSessionCookie = objGSRes.GetString("sessionInfo");
                boolResult = true;  // Sucessful login
            }
            else
            {
                objLogIn.boolIsAccountPendingRegistration = true;
                evalCodeAccounts(objLogIn, objGSResponse);
            }

            return boolResult;
        }

        public bool logOut(ISingleSignOnData objData)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.logout", true);

            objGSRequest.SetParam("UID", objLogIn.strUID);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                objLogIn.boolIsAccountPendingRegistration = (objGSResponse.GetErrorCode() == 206001) ? true : false;

                objLogIn.boolIsInvalidRequest = false;
                objLogIn.strErrormessage = "OK";
                objLogIn.boolIsCompletedOperation = true;

                boolResult = true;  // Sucessful login
            }
            else
            {
                evalCodeAccounts(objLogIn, objGSResponse);
            }

            return boolResult;
        }

        public bool signUp(ISingleSignOnData objData)
        {
            bool boolResult = false;

            //GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.login", true);
            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.register", null, true, strUserKey);
            //objGSRequest.SetParam("username", objLogIn.strEmail);
            objGSRequest.SetParam("email", objLogIn.strEmail);
            objGSRequest.SetParam("password", objLogIn.strPassword);
            objGSRequest.SetParam("finalizeRegistration", true);
            //objGSRequest.SetParam("captchaToken", string.IsNullOrWhiteSpace(objLogIn.strCaptchaToken) ? "NA" : objLogIn.strCaptchaToken);
            objGSRequest.SetParam("data", objLogIn.strData);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                // Everything's okay
                GSObject objGSRes = objGSResponse.GetData();

                objLogIn.strRegToken = (objGSResponse.GetErrorCode() == 206001) ? objGSRes.GetString("regToken") : null;
                objLogIn.boolIsAccountPendingRegistration = (objGSResponse.GetErrorCode() == 206001) ? true : false;
                objLogIn.strUID = objGSRes.GetString("UID");
                objLogIn.strProfile = objGSRes.GetString("profile");
                objLogIn.boolIsInvalidRequest = false;
                objLogIn.strErrormessage = "OK";
                objLogIn.boolIsCompletedOperation = true;

                boolResult = true;  // Sucessful login
            }
            else
            {
                evalCodeAccounts(objLogIn, objGSResponse);
            }

            return boolResult;
        }
    }
}
