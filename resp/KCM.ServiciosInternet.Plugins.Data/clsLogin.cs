using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public class clsLogin : IPlugInLogInObjects
    {
        public string strRegToken { get; set; }

        public string strEmail { get; set; }

        public string strPassword { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }

        public string strUID { get; set; }

        public string strProfile { get; set; }

        public bool boolIsSocialLogin { get; set; }

        public bool boolIsInvalidRequest { get; set; }

        public string strSessionCookie { get; set; }

        public string strErrormessage { get; set; }
        public int intExpirationSessionDays { get; set; }
        public bool boolIsAccountPendingRegistration { get; set; }
        public string strCaptchaToken { get; set; }
        public bool boolIsCompletedOperation { get; set; }

        #region propiedades unicamebte para socialLogin

        struct SocialLoginResponseType
        {

            public string strAccessToken { get; set; }

            public int intExpiresIn { get; set; }

            public string strState { get; set; }

            public bool boolIsNewUser { get; set; }
        }

        struct SocialLoginErrorResponse
        {

            public string strError { get; set; }
            
            public string strErrorDescription { get; set; }

            public string strState { get; set; }
        }

        #endregion


        clsDataTransfer IPlugInLogInObjects.sendData()
        {
            return new clsDataTransfer
            {
                strEmail = this.strEmail,

                strPassword = this.strPassword,

                strProfile = this.strProfile,

                strExtraProfileFields = null,

                strExtraProfileFieldsDescriptor = null,

                boolIsInvalidRequest = this.boolIsInvalidRequest,

                strSessionCookie = this.strSessionCookie,

                boolIsAccountPendingRegistration = this.boolIsAccountPendingRegistration,

                strErrormessage = this.strErrormessage,

                strRandomKey = this.strRandomKey,

                strComputedKey = null,

                strUID = this.strUID,

                strRegToken = this.strRegToken,

                boolIsSocialLogin = this.boolIsSocialLogin,

                intExpirationSessionDays = this.intExpirationSessionDays,

                boolIsCompletedOperation = this.boolIsCompletedOperation
            };
        }

        void IPlugInLogInObjects.receiveData(clsDataTransfer objData)
        {
            this.strEmail = objData.strEmail;

            this.strPassword = objData.strPassword;

            this.boolIsSocialLogin = objData.boolIsSocialLogin;

            this.intExpirationSessionDays = objData.intExpirationSessionDays;

            this.strCaptchaToken = objData.strCaptchaToken;
        }


    }
}
