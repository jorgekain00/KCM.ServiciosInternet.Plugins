using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace KCM.ServiciosInternet.Google.Services.Services
{
    class ReCaptcha
    {
        public static bool isExpiredReCaptcha(string strCatchapSecretKey, string strCaptchaToken)
        {
            string strWebAddress = "https://www.google.com/recaptcha/api/siteverify?secret=" + strCatchapSecretKey + "&response=" + strCaptchaToken;

            WebRequest objHttpWebRequest = WebRequest.Create(strWebAddress);
            HttpWebResponse objHttpResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();
            using (StreamReader objSdr = new StreamReader(objHttpResponse.GetResponseStream()))
            {
                JavaScriptSerializer objJSONSerializer = new JavaScriptSerializer();
                Dictionary<string, object> objResp = (Dictionary<string, object>)objJSONSerializer.DeserializeObject(objSdr.ReadToEnd());
                // todo: debes de poner registro de errores log
                if (!objResp["success"].ToString().Equals("True"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
