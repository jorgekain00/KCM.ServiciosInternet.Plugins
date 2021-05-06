using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace WcfLogInGiGya
{
    public partial class _default : System.Web.UI.Page
    {
        public string strAccessToken = string.Empty;
        public string strError = string.Empty;
        public string strErrorDescription = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString.AllKeys.Contains("error"))
                {
                    strError = Request.QueryString["error"];
                    strErrorDescription = Request.QueryString["error_description"];
                    if (strError.Equals("pending_registration"))
                    {
                        strAccessToken = Request.QueryString["x_regToken"];
                    }
                }
                else
                {
                    strAccessToken = Request.QueryString["access_token"];
                }
            }
        }
    }
}