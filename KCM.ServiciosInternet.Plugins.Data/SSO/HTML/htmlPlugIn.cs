/// <Author>
///     Ing. Jorge Flores Miguel  KCUS/C84818
/// </Author>
/// <CreationDate>
///     April 2021
/// </CreationDate>
/// <summary>
///    Communication AREA to LOAD html template
/// </summary>
namespace KCM.ServiciosInternet.Plugins.Data.SSO.HTML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    [DataContract]
    public class htmlPlugIn
    {
        /// <summary>
        /// template type to load 
        /// </summary>
        public enum templateCtrl
        {
            LOAD,
            LOGIN,
            SIGNIN,
            CONTINUE,
            COMPLETION,
            RESETPS,
            RESETFORM
        }
        /// <summary>
        /// template type to load
        /// </summary>
        [DataMember]
        public templateCtrl enumState;
        /// <summary>
        /// HTML template for SSO operations
        /// </summary>
        [DataMember]
        public string strHtml;
        /// <summary>
        /// API key ID
        /// </summary>
        [DataMember]
        public string strApiID { get; set; }
    }
}
