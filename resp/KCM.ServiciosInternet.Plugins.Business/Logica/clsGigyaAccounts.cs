using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = KCM.ServiciosInternet.Plugins.Entities;
using Gigya.Socialize.SDK;

namespace KCM.ServiciosInternet.Plugins.Business.Logica
{
    class clsGigyaAccounts : IDisposable
    {

        private readonly string strAPIKey;
        private readonly string strAPISecretKey;
        private readonly string strUserKey;
        private readonly string strUserSecretKey;


        public clsGigyaAccounts()
        {
            strAPIKey = Entity.clsConfigPlugIn.strAPIKey;
            strAPISecretKey = Entity.clsConfigPlugIn.strAPISecretKey;
            strUserKey = Entity.clsConfigPlugIn.strUserKey;
            strUserSecretKey = Entity.clsConfigPlugIn.strUserSecretKey;
        }
        private void evalCodeAccounts(Entity.IPlugInLogInObjects objLogIn, GSResponse objGSResponse)
        {
            switch (objGSResponse.GetErrorCode())
            {
                case 206002:
                    objLogIn.strErrormessage = "Cuenta pendiente de verificación";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 401020:
                case 401021:
                    objLogIn.strErrormessage = "Debes verificar reCAPTCHA...";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 403042:
                    objLogIn.strErrormessage = "Correo o contraseña invalidos";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 403043:
                    objLogIn.strErrormessage = "El usuario ya existe";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 400003:
                    objLogIn.strErrormessage = "Ya existe el identificador/email";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 400006:
                    objLogIn.strErrormessage = objGSResponse.GetData().GetString("errorMessage");
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 403047:
                    objLogIn.strErrormessage = "Email en formato incorrecto";
                    objLogIn.boolIsInvalidRequest = false;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
                case 400009:
                    foreach (var objItem in objGSResponse.GetData().GetArray("validationErrors"))
                    {
                        var objErrorCls = new { errorCode = 0, fieldName = "", message = "" };
                        var objErrors = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(objItem.ToString(), objErrorCls);
                        if (objErrors.errorCode != 400027)
                        {
                            if (objErrors.errorCode == 400003)
                            {
                                objLogIn.strErrormessage = "Ya existe el identificador/email";
                                objLogIn.boolIsInvalidRequest = false;
                                objLogIn.boolIsCompletedOperation = false;
                            }
                            else
                            {
                                objLogIn.strErrormessage = objErrors.message;
                                objLogIn.boolIsInvalidRequest = false;
                                objLogIn.boolIsCompletedOperation = false;
                            }
                            break;
                        }
                    }
                    break;
                default:
                    objLogIn.strErrormessage = objGSResponse.GetErrorCode() + " : " + objGSResponse.GetData().GetString("errorMessage");
                    objLogIn.boolIsInvalidRequest = true;
                    objLogIn.boolIsCompletedOperation = false;
                    break;
            }
        }

        public bool logIn(Entity.clsLogin objLogIn)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.login", true);

            objGSRequest.SetParam("loginID", objLogIn.strEmail);
            objGSRequest.SetParam("password", objLogIn.strPassword);
            //objGSRequest.SetParam("captchaToken", string.IsNullOrWhiteSpace(objLogIn.strCaptchaToken) ? "NA" : objLogIn.strCaptchaToken);

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
                objLogIn.strSessionCookie = objGSRes.GetString("sessionInfo");
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

        public bool getAccountInfo(Entity.clsCompleteRegistration objLogIn, bool boolIsUID = true)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strUserSecretKey, "accounts.getAccountInfo", null, true, strUserKey);

            if (boolIsUID)
            {
                objGSRequest.SetParam("UID", objLogIn.strUID);
            }
            else
            {
                objGSRequest.SetParam("regToken", objLogIn.strRegToken);
            }


            objGSRequest.SetParam("include", "loginIDs, profile, data, lastLoginLocation");

            if (objLogIn.strExtraProfileFieldsDescriptor != "")
            {
                objGSRequest.SetParam("extraProfileFields", objLogIn.strExtraProfileFieldsDescriptor);
            }


            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                // Everything's okay
                GSObject objGSRes = objGSResponse.GetData();

                objLogIn.strEmail = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(
                    objGSRes.GetString("profile"), new { email = string.Empty }).email;
                objLogIn.strUID = objGSRes.GetString("UID");
                objLogIn.strProfile = objGSRes.GetString("profile");

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

        public bool setAccountInfo(Entity.clsCompleteRegistration objLogIn, bool boolIsUID = true)
        {
            bool boolResult = false;

            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.setAccountInfo", true);

            if (boolIsUID)
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

        public bool finalizeRegistration(Entity.clsCompleteRegistration objLogIn)
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

        public bool logOut(Entity.clsLogOut objLogIn)
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

        public bool resetPassword(Entity.clsResetPassword objLogIn)
        {
            bool boolResult = false;
            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.resetPassword", true);

            objGSRequest.SetParam("loginID", objLogIn.strEmail);
            //objGSRequest.SetParam("sendEmail", false);
            //objGSRequest.SetParam("passwordResetToken", "tk1..AcbHheXlCA.B2CMGyu5BTySgJPsORxCIVw0GQznuVeqH12byrisow5p9gqkhas3K9lDquKwdzQf.CtONeEb04a1mSJq1k51FQ3nmBu-FwRIFw92UcoE4BYbQyRSGxY-4z_pP7q0fcWixIXBvKrj24S5AEmBAk81RrQ.sc3");
            //objGSRequest.SetParam("newPassword", "Atend2017");

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

        public bool register(Entity.clsSignIn objLogIn)
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

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~clsGigyaAccounts() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
