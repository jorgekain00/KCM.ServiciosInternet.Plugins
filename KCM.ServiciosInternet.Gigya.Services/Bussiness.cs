using KCM.ServiciosInternet.Gigya.Services.Services;
using Entities = KCM.ServiciosInternet.Plugins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;

namespace KCM.ServiciosInternet.Gigya.Services
{
    public class Bussiness
    {
        public static bool resetPassword(Entities.Gigya.ResetPassWordData objData)
        {
            using (GigyaAccounts objGigyaAccounts = new GigyaAccounts())
            {
                return objGigyaAccounts.resetPassword(objData);
            }
        }

        public static bool changePassword(ResetPassWordData objData)
        {
            using (GigyaAccounts objGigyaAccounts = new GigyaAccounts())
            {
                return objGigyaAccounts.changePassword(objData);
            }
        }
    }
}
