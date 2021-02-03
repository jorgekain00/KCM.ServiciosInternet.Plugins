using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public interface IPlugInLogInObjects
    {
        void receiveData(clsDataTransfer objData);
        clsDataTransfer sendData();


        string strErrormessage { get; set; }

        bool boolIsInvalidRequest { get; set; }

        string strComputedKey { get; set; }

        DateTime dtRandomKeyDate { get; set; }

        int intExpirationSessionDays { get; set; }

        string strRandomKey { get; set; }

        string strRegToken { get; set; }

        string strEmail { get; set; }

        string strUID { get; set; }

        bool boolIsAccountPendingRegistration { get; set; }
    
        string strCaptchaToken { get; set; }

        bool boolIsCompletedOperation { get; set; }
    }
}
