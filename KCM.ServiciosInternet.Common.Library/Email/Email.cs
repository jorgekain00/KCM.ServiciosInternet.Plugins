using KCM.ServiciosInternet.Common.Library.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KCM.ServiciosInternet.Common.Library.Email
{
    public class Email
    {
        public static void sendNormalEmailWithHTMLBody(string srtEmailTo, string strEmailCC, string strEmailBcc, string strSubject, string strBody, string strAttachment, string strSender)
        {
            clsEscribirLog.EscribeDebug("Envio Correo contacto", clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Common.Library.Email.Email.sendNormalEmailWithHTMLBody", "Inicia envío correo");

            MailMessage objMsg = new MailMessage();

            if (string.IsNullOrEmpty(srtEmailTo))
            {
                throw new Exception("Cannot send email to an empty <<srtEmailTo>>");
            }

            foreach (string strEmail in srtEmailTo.Split(';', ','))
            {
                objMsg.To.Add(strEmail);
            }


            if (!string.IsNullOrEmpty(strEmailCC))
            {
                foreach (string strEmail in strEmailCC.Split(';', ','))
                {
                    objMsg.CC.Add(strEmail);

                }
            }

            if (!string.IsNullOrEmpty(strEmailBcc))
            {
                foreach (string strEmail in strEmailBcc.Split(';', ','))
                {
                    objMsg.Bcc.Add(strEmail);
                }
            }

            objMsg.Subject = strSubject;// Subject of Email  
            objMsg.From = new System.Net.Mail.MailAddress(strSender);// Email-ID of Sender  
            objMsg.Body = strBody;
            objMsg.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(strAttachment))
            {
                foreach (string strMedia in strAttachment.Split(';', ','))
                {
                    if (!string.IsNullOrEmpty(strMedia))
                        objMsg.Attachments.Add(new Attachment(strMedia));
                }
            }

            SmtpClient objSMTP = new SmtpClient();
            objMsg.BodyEncoding = Encoding.Default;
            objMsg.Priority = MailPriority.Normal;
            objSMTP.Send(objMsg); //Smtpclient to send the mail message  

            clsEscribirLog.EscribeDebug("Envio Correo contacto", clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Common.Library.Email.Email.sendNormalEmailWithHTMLBody", "Fin envío correo");
        }
    }
}
