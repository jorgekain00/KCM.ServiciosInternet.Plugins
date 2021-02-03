using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.LogIn.Data
{
    class CompleteRegistration
    {
        public string strRegToken { get; set; }

        public string strEmail { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }

        public bool boolIsAccountPendingRegistrarion { get; set; }

        public string strUID { get; set; }

        public string strProfile { get; set; }

        public string strExtraProfileFields { get; set; }

    }
}
