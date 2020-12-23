using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.LogIn.Data
{
    class DataTransfer
    {
        public string strEmail { get; set; }

        public string strPassword { get; set; }

        public string strProfile { get; set; }

        public string strExtraProfileFields { get; set; }

        public string strExtraProfileFieldsDescriptor { get; set; }

        public bool boolIsInvalidRequest { get; set; }

        public string strSessionCookie { get; set; }

        public bool boolIsAccountPendingRegistrarion { get; set; }

        public string strErrormessage { get; set; }

        public string strRandomKey { get; set; }

        public string strComputedKey { get; set; }

        public string strUID { get; set; }

        public string strRegToken { get; set; }
    }
}
