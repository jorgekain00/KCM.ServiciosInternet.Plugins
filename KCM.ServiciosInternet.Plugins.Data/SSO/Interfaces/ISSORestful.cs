/// <Author>
///     Ing. Jorge Flores Miguel  KCUS/C84818
/// </Author>
/// <CreationDate>
///     January 2021
/// </CreationDate>
/// <summary>
///     Interface Single Sign On Operations (REST API)
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Data.SSO.Interfaces
{
    using KCM.ServiciosInternet.Plugins.Data.SSO.HTML;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface ISSORestful
    {
        /// <summary>
        /// get account info from UID or token mode
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/getAccountInfo", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData getAccountInfo(ISingleSignOnData objData);
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [WebInvoke(UriTemplate = "/sendCredentialesToSignUp", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData sendCredentialesToSignUp(ISingleSignOnData objData);
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [WebInvoke(UriTemplate = "/sendCredentialesToLogIn", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData sendCredentialesToLogIn(ISingleSignOnData objData);
        /// <summary>
        /// set account info from UID or token mode (Complete registration)
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [WebInvoke(UriTemplate = "/sendMissingFields", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData sendMissingFields(ISingleSignOnData objData);
        /// <summary>
        /// Close user session
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [WebInvoke(UriTemplate = "/LogoutSession", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData LogoutSession(ISingleSignOnData objData);
        /// <summary>
        /// Request Reset Password via custom email
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/requestResetPassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData requestResetPassword(ISingleSignOnData objData);
        /// <summary>
        /// Update the password with a Token value
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/changePassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ISingleSignOnData changePassword(ISingleSignOnData objData);
        /// <summary>
        /// Get JS script according to the request
        /// </summary>
        /// <param name="objData">ISingleSignOnData object(comunication area)</param>
        /// <param name="strApiKey">Api ID for www.kcmsso.com_1 DB</param>
        /// <returns>A Result ISingleSignOnData object (comunication area)</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/JS/JFMGigya{script}?apiKey={apiKey}")]
        System.IO.Stream requestScriptJS(string script, string apiKey);
        /// <summary>
        /// Load template HTML for the SSO Plug In
        /// </summary>
        /// <param name="objHtmlPlugIn"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/getHTMLLogin", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        htmlPlugIn getHTMLLogin(htmlPlugIn objHtmlPlugIn);
    }
}
