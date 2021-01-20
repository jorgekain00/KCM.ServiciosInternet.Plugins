using System.ServiceModel;
using System.ServiceModel.Web;
using Entities = KCM.ServiciosInternet.Plugins.Entities;

namespace WcfLogInGiGya.Interfaces
{
    /// <Author>
    ///     Ing. Jorge Flores Miguel  KCUS/C84818
    /// </Author>
    /// <CreationDate>
    ///     January 2021
    /// </CreationDate>
    /// <summary>
    ///     reset Password vía Gigya Rest API (interface)
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IJFMGigyaResetPassword
    {

        /// <summary>
        ///     send ResetToken  Via Email
        /// </summary>
        /// <param name="objData">ResetPassWordData (Reset Comunication Area Object)</param>
        /// <returns>ResetPassWordData (Reset Comunication Area Object)</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/requestResetPassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.Gigya.ResetPassWordData requestResetPassword(Entities.Gigya.ResetPassWordData objData); 
        

        [OperationContract]
        [WebInvoke(UriTemplate = "/changePassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.Gigya.ResetPassWordData changePassword(Entities.Gigya.ResetPassWordData objData);

        /// <summary>
        ///     
        /// </summary>
        /// <param name="script"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/JS/JFMGigya{script}?apiKey={apiKey}")]
        System.IO.Stream requestScriptJS(string script, string apiKey);

    }


}
