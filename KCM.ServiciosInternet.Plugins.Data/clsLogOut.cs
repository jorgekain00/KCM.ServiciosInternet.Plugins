using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public class clsLogOut : IPlugInLogInObjects
    {
        public string strUID { get; set; }
        public string strErrormessage { get; set; }
        public bool boolIsInvalidRequest { get; set; }
        public string strComputedKey { get; set; }
        public DateTime dtRandomKeyDate { get; set; }
        public int intExpirationSessionDays { get; set; }
        public string strRandomKey { get; set; }
        public string strRegToken { get; set; }
        public string strEmail { get; set; }
        public bool boolIsAccountPendingRegistration { get; set; }
        public string strCaptchaToken { get; set; }
        public bool boolIsCompletedOperation { get; set; }

        clsDataTransfer IPlugInLogInObjects.sendData()
        {
            return new clsDataTransfer
            {
                strEmail = this.strEmail,

                strPassword = null,

                strProfile = null,

                strExtraProfileFields = null,

                strExtraProfileFieldsDescriptor = null,

                boolIsInvalidRequest = this.boolIsInvalidRequest,

                strSessionCookie = null,

                boolIsAccountPendingRegistration = this.boolIsAccountPendingRegistration,

                strErrormessage = this.strErrormessage,

                strRandomKey = this.strRandomKey,

                strComputedKey = null,

                strUID = this.strUID,

                strRegToken = this.strRegToken,

                boolIsSocialLogin = false,

                intExpirationSessionDays = this.intExpirationSessionDays,

                strCaptchaToken = null,

                boolIsCompletedOperation= this.boolIsCompletedOperation
            };
        }

        void IPlugInLogInObjects.receiveData(clsDataTransfer objData)
        {
            this.strUID = objData.strUID;
            this.intExpirationSessionDays = objData.intExpirationSessionDays;
            this.strEmail = objData.strEmail;
            this.strCaptchaToken = objData.strCaptchaToken;
        }
    }
}
