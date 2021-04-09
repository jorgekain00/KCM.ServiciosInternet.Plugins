/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///     Verify Google reCAPTCHA v2 
/// </summary>
namespace KCM.ServiciosInternet.Google.Services
{
    using KCM.ServiciosInternet.Common.Library.Log;
    using KCM.ServiciosInternet.Google.Services.Services;
    using System;

    public static class Bussiness
    {
        /// <summary>
        /// Verify Google reCAPTCHA v2 
        /// </summary>
        /// <param name="strCatchapSecretKey">Google Secret Key</param>
        /// <param name="strCaptchaToken">Google reCAPTCHA from our form</param>
        /// <param name="strUrlGoogle">Google Url for the verify Service</param>
        /// <param name="isDebug">Print Response in the output Log</param>
        /// <returns>Return true for isExpiredReCaptcha otherwise false</returns>
        public static bool isExpiredReCaptcha(string strCatchapSecretKey, string strCaptchaToken, string strUrlGoogle, bool isDebug = false)
        {
            string strFecha = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            ResponseCaptcha objResponse = ReCaptcha.verifySite(strCatchapSecretKey, strCaptchaToken, strUrlGoogle);

            if (isDebug)
            {
                clsEscribirLog.EscribeLog(strFecha, clsEscribirLog.enumTipoMensaje.Informativo, "KCM.ServiciosInternet.Google.Services.Bussiness.isExpiredReCaptcha", "Response from Google ReCaptcha " + objResponse.ToString());
            }

            return objResponse.isSuccess;
        }
        /// <summary>
        /// Verify Google reCAPTCHA v2 
        /// </summary>
        /// <param name="strCatchapSecretKey">Google Secret Key</param>
        /// <param name="strCaptchaToken">Google reCAPTCHA from our form</param>
        /// <param name="strUrlGoogle">Google Url for the verify Service</param>
        /// <returns>Return KCM.ServiciosInternet.Google.Services.Services.ResponseCaptcha object from Google</returns>
        public static ResponseCaptcha verifySite(string strCatchapSecretKey, string strCaptchaToken, string strUrlGoogle)
        {
            return ReCaptcha.verifySite(strCatchapSecretKey, strCaptchaToken, strUrlGoogle);
        }

    }
}
