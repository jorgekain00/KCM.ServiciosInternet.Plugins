using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities = KCM.ServiciosInternet.Plugins.Entities;
using System.Web.Script.Serialization;
using System.Net.Mail;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;
using Gigya = KCM.ServiciosInternet.Gigya.Services;

namespace KCM.ServiciosInternet.Plugins.Business.Logica
{
    static class clsPlugInLogInServices
    {
        internal static bool requestResetPassword(ResetPassWordData objData, string strHtmlPath)
        {
            objData.boolIsInvalidRequest = false;
            if (KCM.ServiciosInternet.Google.Services.BussinessGoogle.isExpiredReCaptcha(Entities.Config.PlugInConfig.strReCAPTCHALogInFlowSecret, objData.recatchapToken))
            {
                objData.strErrormessage = "Debe verificar recaptcha";
                objData.boolIsInvalidRequest = true;
            }
            else
            {
                if (Gigya.Services.BussinessGigya.resetPassword(objData))
                {
                    return sendEmailToUser(objData, strHtmlPath);
                }
            }
            return false;
        }

        internal static bool changePassword(ResetPassWordData objData)
        {

            if (KCM.ServiciosInternet.Google.Services.BussinessGoogle.isExpiredReCaptcha(Entities.Config.PlugInConfig.strReCAPTCHALogInFlowSecret, objData.recatchapToken))
            {
                objData.strErrormessage = "Debe verificar recaptcha";
                objData.boolIsInvalidRequest = true;
            }
            else
            {
                return Gigya.Services.BussinessGigya.changePassword(objData);
            }

            return false;
        }

        private static bool sendEmailToUser(ResetPassWordData objData, string strHtmlPath)
        {

            string strUrlResetPS = string.Format("{0}?regToken={1}&email={2}", Entities.Config.PlugInConfig.strResetPSUrl, objData.strGigyaTokenForResetPS, objData.strEmail);
            string strBody = string.Join("", File.ReadAllLines(Path.Combine(strHtmlPath, Entities.Config.PlugInConfig.StrMailHTML))).Replace("${|urlResertPS|}", strUrlResetPS); // Load mail template


            MailMessage message = new MailMessage();

            message.To.Add(objData.strEmail); // Email-ID of Receiver  


            message.Subject = Entities.Config.PlugInConfig.strResetEmailSubject;
            message.From = new System.Net.Mail.MailAddress(Entities.Config.PlugInConfig.strFromAddressEmail);// Email-ID of Sender  
            message.IsBodyHtml = true;
            message.Body = strBody;
            message.BodyEncoding = Encoding.Default;
            message.Priority = MailPriority.High;

            SmtpClient SmtpMail = new SmtpClient(Entities.Config.PlugInConfig.strSMTPServer);
            SmtpMail.UseDefaultCredentials = true;
            SmtpMail.Send(message); //Smtpclient to send the mail message 

            return true;
        }

        internal static string requestScriptJS(string strPathScripts, string strScriptName, string strApiKey)
        {
            string strResponse = string.Empty;   // script function
            try
            {

                // Get JS Script
                // strApiKey for future DB implementations

                strResponse = File.ReadAllText(Path.Combine(strPathScripts, strScriptName));


                // Replace placeholder with values

                strResponse = strResponse
                    .Replace("${|ServiceName|}", Entities.Config.PlugInConfig.strServiceUrl)
                    .Replace("${|UrlImg|}", Entities.Config.PlugInConfig.strLoadingImage);

            }
            catch (System.IO.FileNotFoundException)
            {
                strResponse = "";
            }
            catch (Exception ex)
            {
                throw;
            }

            return strResponse;
        }

    }
}
