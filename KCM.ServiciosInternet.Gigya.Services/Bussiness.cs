using KCM.ServiciosInternet.Gigya.Services.Services;
using Entities = KCM.ServiciosInternet.Plugins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Plugins.Entities.Gigya;
using KCM.ServiciosInternet.Gigya.Services.Data;

namespace KCM.ServiciosInternet.Gigya.Services
{
    public class Bussiness
    {
        //public static bool resetPassword(Entities.Gigya.ResetPassWordData objDataRs)
        //{
        //    using (GigyaAccountsREST objGigyaAccounts = new GigyaAccountsREST(@"C:\KCM\Servicios de Internet\KCM.ServiciosInternet.Plugins\WcfLogInGiGya\Resources\Codes\SSOConfig.json"))
        //    {
        //        GigyaData objData = new GigyaData();
        //        objData.strEmail = objData.strEmail;
        //        return objGigyaAccounts.requestResetPasswordEmail(objData);
        //    }
        //}

        //public static bool changePassword(ResetPassWordData objDataRs)
        //{
        //    using (GigyaAccountsREST objGigyaAccounts = new GigyaAccountsREST(@"C:\KCM\Servicios de Internet\KCM.ServiciosInternet.Plugins\WcfLogInGiGya\Resources\Codes\SSOConfig.json"))
        //    {
        //        GigyaData objData = new GigyaData();
        //        objData.strEmail = objDataRs.strEmail;
        //        objData.strRegToken = objDataRs.strGigyaTokenForResetPS;
        //        objData.strPassword = objDataRs.strPassword;
        //        return objGigyaAccounts.updatePasswordWithToken(objData);
        //    }
        //}
    }
}
