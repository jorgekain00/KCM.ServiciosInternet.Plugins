using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Google.Services.Services;


namespace KCM.ServiciosInternet.Google.Services
{
    public static class Bussiness
    {
        public static bool isExpiredReCaptcha(string strCatchapSecretKey, string strCaptchaToken)
        {
            return ReCaptcha.isExpiredReCaptcha(strCatchapSecretKey, strCaptchaToken);
        }
    }
}
