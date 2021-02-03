using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.LogIn.Data
{
    class Login
    {
        public string strRegToken { get; set; }

        public string strEmail { get; set; }

        public string strPassword { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }

        public bool boolIsAccountPendingRegistrarion { get; set; }

        public string strUID { get; set; }

        public string strProfile { get; set; }

        public string strExtraProfileFields { get; set; }

        public bool boolIsPreviousLogIn { get; set; }

        public bool boolIsIsNewUser { get; set; }

        public bool boolIsSocialLogin { get; set; }

    }
}
