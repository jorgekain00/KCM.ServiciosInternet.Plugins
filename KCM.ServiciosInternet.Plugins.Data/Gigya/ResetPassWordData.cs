using System.Runtime.Serialization;

namespace KCM.ServiciosInternet.Plugins.Entities.Gigya
{
    /// <Author>
    ///     Ing. Jorge Flores Miguel  KCUS/C84818
    /// </Author>
    /// <CreationDate>
    ///     January 2021
    /// </CreationDate>
    /// <summary>
    ///     Comunication Area for reset Password
    /// </summary>
    [DataContract]
    public class ResetPassWordData
    {
        /// <summary>
        /// Unique Id or Email (Gigya Account)
        /// </summary>
        [DataMember]
        public string strEmail { get; set; }
        /// <summary>
        /// recatchapToken
        /// </summary>
        [DataMember]
        public string recatchapToken { get; set; }
        /// <summary>
        /// Set invalidRequest Flag
        /// </summary>
        [DataMember]
        public bool boolIsInvalidRequest { get; set; }
        /// <summary>
        /// Error messages
        /// </summary>
        [DataMember]
        public string strErrormessage { get; set; }
        /// <summary>
        /// New Password
        /// </summary>
        [DataMember]
        public string strPassword { get; set; }
        /// <summary>
        /// Reset PS Token for Gigya
        /// </summary>
        [DataMember]
        public string strGigyaTokenForResetPS { get; set; }
    }
}
