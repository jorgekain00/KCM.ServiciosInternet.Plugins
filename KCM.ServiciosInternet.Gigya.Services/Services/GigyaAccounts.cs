using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigya.Socialize.SDK;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;
using Entities = KCM.ServiciosInternet.Plugins.Entities;

namespace KCM.ServiciosInternet.Gigya.Services.Services
{
    class GigyaAccounts : IDisposable
    {

        private readonly string strAPIKey;
        private readonly string strAPISecretKey;
        private readonly string strUserKey;
        private readonly string strUserSecretKey;

        public GigyaAccounts()
        {
            this.strAPIKey = Entities.Config.GigyaConfig.strAPIKey;
            this.strAPISecretKey = Entities.Config.GigyaConfig.strAPISecretKey;
            this.strUserKey = Entities.Config.GigyaConfig.strUserKey;
            this.strUserSecretKey = Entities.Config.GigyaConfig.strUserSecretKey;
        }

        public GigyaAccounts(string strAPIKey, string strAPISecretKey, string strUserKey, string strUserSecretKey)
        {
            this.strAPIKey = strAPIKey;
            this.strAPISecretKey = strAPISecretKey;
            this.strUserKey = strUserKey;
            this.strUserSecretKey = strUserSecretKey;
        }

        private string evalCodeGigyaAccountService(GSResponse objGSResponse)
        {
            string strErrorMessage = string.Empty;
            switch (objGSResponse.GetErrorCode())
            {
                case 206002:
                    strErrorMessage = "Cuenta pendiente de verificación";
                    break;
                case 401020:
                case 401021:
                    strErrorMessage = "Debes verificar reCAPTCHA...";
                    break;
                case 403042:
                    strErrorMessage = "Correo o contraseña invalidos";
                    break;
                case 403043:
                    strErrorMessage = "El usuario ya existe";
                    break;
                case 400003:
                    strErrorMessage = "Ya existe el identificador/email";
                    break;
                case 400006:
                    strErrorMessage = objGSResponse.GetData().GetString("errorMessage");
                    break;
                case 403047:
                    strErrorMessage = "Email en formato incorrecto o no existe";
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
                                strErrorMessage = "Ya existe el identificador/email";
                            }
                            else
                            {
                                strErrorMessage = objErrors.message;
                            }
                            break;
                        }
                    }
                    break;
                default:
                    strErrorMessage = objGSResponse.GetErrorCode() + " : " + objGSResponse.GetData().GetString("errorMessage");
                    break;
            }

            return strErrorMessage;
        }

        internal bool resetPassword(Entities.Gigya.ResetPassWordData objData)
        {
            bool boolResult = false;
            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.resetPassword", true);

            objGSRequest.SetParam("loginID", objData.strEmail);
            objGSRequest.SetParam("sendEmail", false);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                objData.strGigyaTokenForResetPS = objGSResponse.GetData().GetString("passwordResetToken");
                objData.strErrormessage = "";
                boolResult = true;  // Sucessful login
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.boolIsInvalidRequest = false;
            }

            return boolResult;
        }

        internal bool changePassword(ResetPassWordData objData)
        {
            bool boolResult = false;
            GSRequest objGSRequest = new GSRequest(this.strAPIKey, this.strAPISecretKey, "accounts.resetPassword", true);

            objGSRequest.SetParam("loginID", objData.strEmail);
            objGSRequest.SetParam("passwordResetToken", objData.strGigyaTokenForResetPS);
            objGSRequest.SetParam("newPassword", objData.strPassword);

            GSResponse objGSResponse = objGSRequest.Send();

            if (objGSResponse.GetErrorCode() == 0 || objGSResponse.GetErrorCode() == 206001)
            {
                boolResult = true;  // Sucessful login
            }
            else
            {
                objData.strErrormessage = evalCodeGigyaAccountService(objGSResponse);
                objData.boolIsInvalidRequest = true;
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
