using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Plugins.Business.Logica;
using Entity = KCM.ServiciosInternet.Plugins.Entities;

namespace KCM.ServiciosInternet.Plugins.Business
{
    public static class clsFachada
    {
        public static Entity.clsSessionState generateRandomKey( ref Entity.clsDataTransfer objData, string strCookieSessionId, Entity.clsSessionState.EventType eventType)
        {
            return clsPlugInLogInServices.generateRandomKey( ref objData, strCookieSessionId, eventType);
        }

        public static bool sendCredentialesToLogIn(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.sendCredentialesToLogIn(ref objData);
        }

        public static Entity.clsSessionState getAccountInfo(ref Entity.clsDataTransfer objData, Entity.clsSessionState.EventType eventType)
        {
            return clsPlugInLogInServices.getAccountInfo(ref objData, eventType);
        }
        public static bool sendMissingFields(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.sendMissingFields(ref objData);
        }

        public static bool deleteRandomKey(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.deleteRandomKey(ref objData);
        }

        public static bool LogoutSession(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.LogoutSession(ref objData);
        }

        public static bool requestResetPassword(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.requestResetPassword(ref objData);
        }

        public static bool sendCredentialesToSignIn(ref Entity.clsDataTransfer objData)
        {
            return clsPlugInLogInServices.sendCredentialesToSignIn(ref objData);
        }



    }
}
