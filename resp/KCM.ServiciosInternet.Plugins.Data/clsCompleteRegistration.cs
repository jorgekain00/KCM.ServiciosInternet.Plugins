using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public class clsCompleteRegistration : IPlugInLogInObjects
    {
        public string strRegToken { get; set; }

        public string strEmail { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }

        public string strUID { get; set; }

        public string strProfile { get; set; }

        public string strData { get; set; }

        public string strExtraProfileFields { get; set; }

        public string strExtraProfileFieldsDescriptor { get; set; }

        public string strErrormessage { get; set; }

        public bool boolIsInvalidRequest { get; set; }

        public int intExpirationSessionDays { get; set; }
        public bool boolIsAccountPendingRegistration { get; set; }
        public string strCaptchaToken { get; set; }
        public bool boolIsCompletedOperation { get; set; }

        public string strSessionCookie { get; set; }

        clsDataTransfer IPlugInLogInObjects.sendData()
        {
            return new clsDataTransfer
            {
                strEmail = this.strEmail,

                strPassword = null,

                strProfile = this.strProfile,

                strData = this.strData,

                strExtraProfileFields = this.strExtraProfileFields,

                strExtraProfileFieldsDescriptor = this.strExtraProfileFieldsDescriptor,

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

                boolIsCompletedOperation = this.boolIsCompletedOperation
            };
        }

      

        void IPlugInLogInObjects.receiveData(clsDataTransfer objData)
        {
            this.strEmail = objData.strEmail;
            this.strRegToken = objData.strRegToken;
            this.strUID = objData.strUID;
            this.strProfile = objData.strProfile;
            this.strData = objData.strData;
            this.strExtraProfileFields = objData.strExtraProfileFields;
            this.strExtraProfileFieldsDescriptor = objData.strExtraProfileFieldsDescriptor;
            this.intExpirationSessionDays = objData.intExpirationSessionDays;
            this.strCaptchaToken = objData.strCaptchaToken;
        }
    }
}
