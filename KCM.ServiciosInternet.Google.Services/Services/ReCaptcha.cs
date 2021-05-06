/// <author>
///     Eng. Jorge Flores Miguel  C84818
/// </author>
/// <creationDate>
///     April 2021
/// </creationDate>
/// <summary>
///     Verify Google reCAPTCHA v2 
/// </summary>
/// <remarks>
/// 
///     Google API request URL: https://www.google.com/recaptcha/api/siteverify 
///     
///     POST Parameter	Description
///     
///     secret          Required.The shared key between your site and reCAPTCHA.
///     response        Required. The user response token provided by the reCAPTCHA client-side integration on your site.
///     remoteip        Optional. The user's IP address.
///     
///     JSON object Response
/// 
///     {
///         "success": true|false,
///         "challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
///         "hostname": string,         // the hostname of the site where the reCAPTCHA was solved
///         "error-codes": [...]        // optional
///     }
///     
///     Error code	                Description
///     
///     missing-input-secret        The secret parameter is missing.
///     invalid-input-secret        The secret parameter is invalid or malformed.
///     missing-input-response      The response parameter is missing.
///     invalid-input-response      The response parameter is invalid or malformed.
///     bad-request                 The request is invalid or malformed.
///     timeout-or-duplicate        The response is no longer valid: either is too old or has been used previously.
/// </remarks>
namespace KCM.ServiciosInternet.Google.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Web.Script.Serialization;


    internal static class ReCaptcha
    {
        /// <summary>
        /// Verify Google reCAPTCHA v2 
        /// </summary>
        /// <param name="strCatchapSecretKey">Google Secret Key</param>
        /// <param name="strCaptchaToken">Google reCAPTCHA from our form</param>
        /// <param name="strProviderUrlValidator">Google Url for the verify Service</param>
        /// <returns>Return KCM.ServiciosInternet.Google.Services.Services.ResponseCaptcha object from Google</returns>
        public static ResponseCaptcha verifySite(string strCatchapSecretKey, string strCaptchaToken, string strProviderUrlValidator)
        {
            /*--------------------------------------------
             * Replace values for Google params
             *--------------------------------------------*/
            string strWebAddress = string.Format("{0}?secret={1}&response={2}", strProviderUrlValidator, strCatchapSecretKey, strCaptchaToken);

            WebRequest objHttpWebRequest = WebRequest.Create(strWebAddress);
            HttpWebResponse objHttpResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

            using (StreamReader objSrdr = new StreamReader(objHttpResponse.GetResponseStream()))
            {
                JavaScriptSerializer objJSONSerializer = new JavaScriptSerializer();
                Dictionary<string, object> objResp = (Dictionary<string, object>)objJSONSerializer.DeserializeObject(objSrdr.ReadToEnd());

                // Get Google response
                ResponseCaptcha objResponse = new ResponseCaptcha();
                objResponse.isSuccess = Convert.ToBoolean(objResp["success"].ToString());

                if (objResponse.isSuccess)
                {
                    objResponse.dtChallenge_ts = Convert.ToDateTime(objResp["challenge_ts"].ToString());
                    objResponse.hostname = objResp["hostname"].ToString();
                }
                else
                {
                    // Recupera los mensajes de error
                    int intIndice = 0;
                    System.Array objArray = objResp["error-codes"] as System.Array;
                    objResponse.errorCodes = new string[objArray.Length];

                    foreach (var item in objArray)
                    {
                        objResponse.errorCodes[intIndice++] = item.ToString();
                    }
                }

                return objResponse;
            }
        }
    }

    /// <summary>
    /// ResponseCaptcha to store Google response from Google reCAPTCHA v2 
    /// </summary>
    public struct ResponseCaptcha
    {
        /// <summary>
        /// Successful operation 
        /// </summary>
        public bool isSuccess { get; set; }
        /// <summary>
        /// timestamp of the challenge load
        /// </summary>
        public DateTime dtChallenge_ts { get; set; }
        /// <summary>
        /// the hostname of the site where the reCAPTCHA was solved
        /// </summary>
        public string hostname { get; set; }
        /// <summary>
        /// list of errors
        /// </summary>
        public string[] errorCodes { get; set; }
        /// <summary>
        /// Return reponse in plain texts
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("isSuccess '{3}' hostName '{0}' challenge_ts '{1}' errorCodes '{2}'",
                this.hostname,
                this.dtChallenge_ts,
                this.errorCodes == null ? "" : string.Join("/", this.errorCodes),
                this.isSuccess);
        }

    }
}
