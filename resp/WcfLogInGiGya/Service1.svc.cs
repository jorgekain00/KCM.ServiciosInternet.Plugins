using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Entities = KCM.ServiciosInternet.Plugins.Entities;
using Business = KCM.ServiciosInternet.Plugins.Business;
using System.Web;
using System.ServiceModel.Activation;
using System.IO;
using System.ServiceModel.Channels;

namespace WcfLogInGiGya
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Service1 : IService1
    {
        public Entities.clsDataTransfer deleteRandomHex(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.deleteRandomKey(ref objData);
            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            objData.strErrormessage = "OK";
            return objData;
        }



        public Entities.clsDataTransfer generateRandomKeyForLogIn(Entities.clsDataTransfer objData)
        {
            try
            {
                HttpRequestMessageProperty objHttpRequest = (HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties["httpRequest"];
                string[] strCookies= objHttpRequest.Headers["Cookie"].Split(';');
                string strCookieSessionId = strCookies.Where((str) => str.Contains("ASP.NET_SessionId=")).ToArray()[0].Replace("ASP.NET_SessionId=", "");
                Entities.clsSessionState objSessionState = Business.clsFachada.generateRandomKey(ref objData, strCookieSessionId, Entities.clsSessionState.EventType.LogIn);
            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }

        public Entities.clsDataTransfer getAccountInfoCompleteRegistration(Entities.clsDataTransfer objData)
        {
            try
            {
                Entities.clsSessionState objSessionState = Business.clsFachada.getAccountInfo(ref objData, Entities.clsSessionState.EventType.CompleteRegistration);

            }
            catch (Exception ex)
            {
                objData.strErrormessage = ex.ToString();
            }
            return objData;
        }
        public StatePlugIn getHTMLLogin(StatePlugIn objState)
        {
            string strTemplate = string.Empty;
            string strContent = string.Empty;
            try
            {

                switch (objState.enumState)
                {
                    case "LOAD":
                        strTemplate = HttpContext.Current.Server.MapPath("~/html/load.html");
                        break;
                    case "LOGIN":
                        strTemplate = HttpContext.Current.Server.MapPath("~/html/SignUpLogIn.html");
                        break;
                    case "CONTINUE":
                        strTemplate = HttpContext.Current.Server.MapPath("~/html/Continue.html");
                        break;
                    case "COMPLETION":
                        strTemplate = HttpContext.Current.Server.MapPath("~/html/CompletionRegister.html");
                        break;
                    case "RESETPS":
                        strTemplate = HttpContext.Current.Server.MapPath("~/html/ResetPS.html");
                        break;
                }

                strContent = string.Join("", File.ReadAllLines(strTemplate));
            }
            catch (Exception ex)
            {

                strContent = ex.ToString();
            }
            objState.strHTML = strContent;

            return objState;
        }

        public Entities.clsDataTransfer LogoutSession(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.LogoutSession(ref objData);

            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }

        public Entities.clsDataTransfer requestResetPassword(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.requestResetPassword(ref objData);

                // Se forza a que termine bien para mandar el msg genérico al usuario.
                // NOTA: En la siguiente versión desarrollar el log de errores
                if (!objData.strErrormessage.Equals("Debes verificar reCAPTCHA..."))
                {
                    objData.strErrormessage = "OK";
                }
            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }

        public Entities.clsDataTransfer sendCredentialesToLogIn(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.sendCredentialesToLogIn(ref objData);

            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }

        public Entities.clsDataTransfer sendCredentialesToSignUp(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.sendCredentialesToSignIn(ref objData);

            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }

        public Entities.clsDataTransfer sendMissingFields(Entities.clsDataTransfer objData)
        {
            try
            {
                Business.clsFachada.sendMissingFields(ref objData);

            }
            catch (Exception ex)
            {

                objData.strErrormessage = ex.ToString();
                objData.boolIsInvalidRequest = true;
            }
            return objData;
        }
    }

}
