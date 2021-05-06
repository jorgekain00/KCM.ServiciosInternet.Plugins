using System.Collections.Generic;

namespace KCM.ServiciosInternet.Plugins.Data.SSO.Config
{
    public class SSOConfig
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Session
        {
            public int expirationSessionInMins { get; set; }
            public string sessionExpiredMessage { get; set; }
        }

        public class Mail
        {
            public string resetPSMailSubject { get; set; }
            public string fromMail { get; set; }
        }

        public class ResetPassword
        {
            public Mail mail { get; set; }
            public bool isEnableMessage { get; set; }
            public string genericMessage { get; set; }
        }

        public class Templates
        {
            public string mailPath { get; set; }
            public string resetFormPath { get; set; }
            public string loadPath { get; set; }
            public string loginPath { get; set; }
            public string signInPath { get; set; }
            public string continuePath { get; set; }
            public string completionPath { get; set; }
            public string resetPath { get; set; }
        }

        public class Site
        {
            public string Url { get; set; }
            public ResetPassword resetPassword { get; set; }
            public Templates templates { get; set; }
        }

        public class LoginFlow
        {
            public string apiKeyVc { get; set; }
            public string secretKeyVc { get; set; }
            public string apiProviderUrlVc { get; set; }
            public string providerUrlValidatorVc { get; set; }
        }

        public class RegistrationFlow
        {
            public string apiKeyVc { get; set; }
            public string secretKeyVc { get; set; }
            public string apiProviderUrlVc { get; set; }
            public string providerUrlValidatorVc { get; set; }
        }

        public class ReCAPTCHA
        {
            public bool isDisableReCAPTCHA { get; set; }
            public string genericMessage { get; set; }
            public LoginFlow loginFlow { get; set; }
            public RegistrationFlow registrationFlow { get; set; }
        }

        public class Settings
        {
            public Session session { get; set; }
            public Site site { get; set; }
            public ReCAPTCHA reCAPTCHA { get; set; }
        }

        public class Keys
        {
            public string aPIKey { get; set; }
            public string aPISecretKey { get; set; }
            public string userKey { get; set; }
            public string userSecretKey { get; set; }
        }

        public class Login
        {
            public bool isEnableMessage { get; set; }
            public string genericMessage { get; set; }
        }

        public class SingIn
        {
            public bool isEnableMessage { get; set; }
            public string genericMessage { get; set; }
        }

        public class Messages
        {
            public Login login { get; set; }
            public SingIn singIn { get; set; }
            public ResetPassword resetPassword { get; set; }
        }

        public class Code
        {
            public string number { get; set; }
            public string lang { get; set; }
            public string description { get; set; }
        }

        public class Codes
        {
            public Messages messages { get; set; }
            public string language { get; set; }
            public List<Code> code { get; set; }
        }

        public class Provider
        {
            public string dll { get; set; }
            public Keys keys { get; set; }
            public Codes codes { get; set; }
        }

        public class Root
        {
            public Settings settings { get; set; }
            public Provider provider { get; set; }
        }


    }
}
