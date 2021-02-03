using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    [DataContract]
    public class clsDataTransfer
    {
        [DataMember]
        public string strEmail { get; set; }

        [DataMember]
        public string strPassword { get; set; }

        [DataMember]
        public string strProfile { get; set; }

        [DataMember]
        public string strData { get; set; }

        [DataMember]
        public string strExtraProfileFields { get; set; }

        [DataMember]
        public string strExtraProfileFieldsDescriptor { get; set; }

        [DataMember]
        public bool boolIsInvalidRequest { get; set; }

        [DataMember]
        public string strSessionCookie { get; set; }

        [DataMember]
        public bool boolIsAccountPendingRegistration { get; set; }

        [DataMember]
        public bool boolIsSocialLogin { get; set; }

        [DataMember]
        public string strErrormessage { get; set; }

        [DataMember]
        public string strRandomKey { get; set; }

        [DataMember]
        public string strComputedKey { get; set; }

        [DataMember]
        public string strUID { get; set; }

        [DataMember]
        public string strRegToken { get; set; }

        [DataMember]
        public int intExpirationSessionDays { get; set; }

        [DataMember]
        public string strCaptchaToken { get; set; }

        [DataMember]
        public bool boolIsCompletedOperation { get; set; }
    }
}
