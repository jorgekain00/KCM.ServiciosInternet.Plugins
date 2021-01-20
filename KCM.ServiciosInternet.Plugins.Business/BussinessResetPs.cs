using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Plugins.Business.Logica;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;
using Entity = KCM.ServiciosInternet.Plugins.Entities;

namespace KCM.ServiciosInternet.Plugins.Business
{
    public static class BussinessResetPs
    {
        public static string requestScriptJS(string strPathScripts, string strScriptName, string strApiKey)
        {
            return clsPlugInLogInServices.requestScriptJS(strPathScripts, strScriptName,  strApiKey);
        }

        public static bool requestResetPassword(ResetPassWordData objData, string strHtmlPath)
        {
            return clsPlugInLogInServices.requestResetPassword(objData, strHtmlPath);
        }

        public static bool changePassword(ResetPassWordData objData)
        {
            return clsPlugInLogInServices.changePassword(objData);
        }
    }
}
