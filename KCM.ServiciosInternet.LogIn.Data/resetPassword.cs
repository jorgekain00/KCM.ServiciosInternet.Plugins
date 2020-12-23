using System;
using System.Collections.Generic;
using System.Text;

namespace KCM.ServiciosInternet.LogIn.Data
{
    class resetPassword
    {
        public string strEmail { get; set; }

        public string strRandomKey { get; set; }

        public DateTime dtRandomKeyDate { get; set; }

        public string strComputedKey { get; set; }
    }
}
