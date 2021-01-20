using KCM.Common.Log;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;
using System;
using System.IO;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using WcfLogInGiGya.Interfaces;
using Business = KCM.ServiciosInternet.Plugins.Business;
using Entities = KCM.ServiciosInternet.Plugins.Entities;

namespace WcfLogInGiGya.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ResetPS : IJFMGigyaResetPassword
    {
        public ResetPassWordData changePassword(ResetPassWordData objData)
        {
            string strLogName = Entities.Config.PlugInConfig.strLogName;
            objData.boolIsInvalidRequest = true;

            using (clsLog objLog = new clsLog(HttpContext.Current.Server.MapPath("~/"), strLogName, true, strLogName + ".err.xml", HttpContext.Current.Server.MapPath("~/")))
            {
                try
                {
                    if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.strEmail).Length == 0)
                    {
                        objData.strErrormessage = "Ingrese un correo válido'";
                    }
                    else if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.recatchapToken).Length == 0)
                    {
                        objData.strErrormessage = "Debe verificar recaptcha";
                    }
                    else if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.strGigyaTokenForResetPS).Length == 0)
                    {
                        objData.strErrormessage = "Debe Solicitar de nuevo el reseteo de password en el sitio";
                    }
                    else if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.strPassword).Length == 0)
                    {
                        objData.strErrormessage = "Debe ingresar un password válido";
                    }
                    else
                    {
                        if (Business.BussinessResetPs.changePassword(objData))
                        {
                            objData.boolIsInvalidRequest = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objLog.EscribeLog(DateTime.Now.ToString(), clsLog.enumTipoMensaje.Excepcion, "Se ha producido una excepción en WcfLogInGiGya/Services/resetPS/changePassword", ex, true);
                    objData.boolIsInvalidRequest = true;
                    objData.strErrormessage = ex.ToString();
                }
            }
            return objData;
        }

        public Entities.Gigya.ResetPassWordData requestResetPassword(Entities.Gigya.ResetPassWordData objData)
        {
            string strLogName = Entities.Config.PlugInConfig.strLogName;
            string strHtmlPath = HttpContext.Current.Server.MapPath("~/html");
            objData.boolIsInvalidRequest = true;

            using (clsLog objLog = new clsLog(HttpContext.Current.Server.MapPath("~/"), strLogName, true, strLogName + ".err.xml", HttpContext.Current.Server.MapPath("~/")))
            {
                try
                {
                    if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.strEmail).Length == 0)
                    {
                        objData.strErrormessage = "Ingrese un correo válido'";
                    }
                    else if (Microsoft.Security.Application.Encoder.HtmlEncode(objData.recatchapToken).Length == 0)
                    {
                        objData.strErrormessage = "Debe verificar recaptcha";
                    }
                    else
                    {
                        if (Business.BussinessResetPs.requestResetPassword(objData, strHtmlPath))
                        {
                            objData.boolIsInvalidRequest = false;
                        }
                        else
                        {
                            if (!objData.boolIsInvalidRequest)   // boolIsInvalidRequest was updated by requestResetPassword method
                            {
                                throw new Exception(string.Format("Business.BussinessResetPs.requestResetPassword Message : {0}", objData.strErrormessage));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    objLog.EscribeLog(DateTime.Now.ToString(), clsLog.enumTipoMensaje.Excepcion, "Se ha producido una excepción en WcfLogInGiGya/Services/resetPS/requestResetPassword", ex, true);
                    objData.boolIsInvalidRequest = true;
                    objData.strErrormessage = ex.ToString();
                }
            }
            return objData;
        }

        /// <summary>
        ///     Get JS Script for download on the client Site
        /// </summary>
        /// <param name="script">Script Name</param>
        /// <param name="apiKey">Gigya Api Key</param>
        /// <returns>Encode script JS </returns>
        /// <remarks>
        ///     JS Script is reading from disk [2021/01/11]
        /// </remarks>
        public System.IO.Stream requestScriptJS(string script, string apiKey)
        {
            string strResponse = string.Empty;  // selected script
            string strLogName = Entities.Config.PlugInConfig.strLogName;


            using (clsLog objLog = new clsLog(HttpContext.Current.Server.MapPath("~/Logs"), strLogName, true, strLogName + ".err.xml", HttpContext.Current.Server.MapPath("~/Logs")))
            {
                try
                {
                    string strScriptName = script.Replace("JFMGigya", "");
                    string strPathScripts = HttpContext.Current.Server.MapPath("~/Scripts");

                    if (strScriptName.Length > 0)
                    {
                        strResponse = Business.BussinessResetPs.requestScriptJS(strPathScripts, strScriptName, apiKey);
                    }
                }
                catch (Exception ex)
                {
                    objLog.EscribeLog(DateTime.Now.ToString(), clsLog.enumTipoMensaje.Excepcion, "Se ha producido una excepción en WcfLogInGiGya/Services/resetPS/requestScriptJS", ex, true);
                }
                finally
                {
                    if (strResponse.Length == 0)
                    {
                        strResponse = "console.log(\"JFMGigya" + script + " cannot load\")";
                    }
                }
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            byte[] bytesResponse = Encoding.UTF8.GetBytes(strResponse);   // Encode script into a Stream
            return new MemoryStream(bytesResponse);
        }
    }

}
