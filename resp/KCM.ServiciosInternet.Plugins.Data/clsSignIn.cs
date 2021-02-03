using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public class clsSignIn : IPlugInLogInObjects
    {
        public string strRegToken { get; set; }

        public string strEmail { get; set; }

        public string strPassword { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }

        public bool boolIsAccountPendingRegistration { get; set; }

        public string strUID { get; set; }

        public string strProfile { get; set; }

        public string strData { get; set; }

        public bool boolIsInvalidRequest { get; set; }

        public string strSessionCookie { get; set; }

        public string strErrormessage { get; set; }

        public int intExpirationSessionDays { get; set;}

        public string strCaptchaToken { get; set; }
        public bool boolIsCompletedOperation { get; set; }

        clsDataTransfer IPlugInLogInObjects.sendData()
        {
            return new clsDataTransfer
            {
                strEmail = this.strEmail,

                strPassword = this.strPassword,

                strProfile = this.strProfile,

                strData = this.strData,

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

                boolIsSocialLogin = false,

                intExpirationSessionDays = this.intExpirationSessionDays,

                strCaptchaToken = null,

                boolIsCompletedOperation = this.boolIsCompletedOperation
            };
        }

        void IPlugInLogInObjects.receiveData(clsDataTransfer objData)
        {
            this.strEmail = objData.strEmail;

            this.strPassword = objData.strPassword;

            this.intExpirationSessionDays = objData.intExpirationSessionDays;

            this.strCaptchaToken = objData.strCaptchaToken;

            this.strData = objData.strData;
        }
    }
}
