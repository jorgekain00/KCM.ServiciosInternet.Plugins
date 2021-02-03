using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Entities = KCM.ServiciosInternet.Plugins.Entities;

namespace WcfLogInGiGya
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    //[ServiceContract]
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    
    public interface IService1
    {


        [OperationContract]

        [WebInvoke(UriTemplate = "/generateRandomKeyForLogIn", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer generateRandomKeyForLogIn(Entities.clsDataTransfer objData);

        [OperationContract]

        [WebInvoke(UriTemplate = "/getAccountInfoCompleteRegistration", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer getAccountInfoCompleteRegistration(Entities.clsDataTransfer objData);


        [WebInvoke(UriTemplate = "/sendCredentialesToSignUp", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer sendCredentialesToSignUp(Entities.clsDataTransfer objData);

        [WebInvoke(UriTemplate = "/sendCredentialesToLogIn", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer sendCredentialesToLogIn(Entities.clsDataTransfer objData);

        [WebInvoke(UriTemplate = "/sendMissingFields", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer sendMissingFields(Entities.clsDataTransfer objData);

        [WebInvoke(UriTemplate = "/LogoutSession", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer LogoutSession(Entities.clsDataTransfer objData);

        [WebInvoke(UriTemplate = "/requestResetPassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer requestResetPassword(Entities.clsDataTransfer objData);

        [WebInvoke(UriTemplate = "/deleteRandomHex", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entities.clsDataTransfer deleteRandomHex(Entities.clsDataTransfer objData);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getHTMLLogin", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        StatePlugIn getHTMLLogin(StatePlugIn enumState);

        // TODO: agregue aquí sus operaciones de servicio

    }

    [DataContract]
    public class StatePlugIn
    {
        [DataMember]
        public string enumState;
        [DataMember]
        public string strHTML;
    }

    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
