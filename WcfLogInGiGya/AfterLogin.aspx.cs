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
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAccessToken = "";
            string strError = "";
            string strErrorDescription = "";

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

            string strArchivo = string.Join("", File.ReadAllLines(Server.MapPath("~/html/SocialResponse.html")));
            strArchivo = strArchivo.Replace("%strAccessToken%", strAccessToken).Replace("%strError%", strError).Replace("%strErrorDescription%", strErrorDescription);

            Response.Clear();
            //Response.ContentType = "html/text";
            Response.Write(strArchivo);
            Response.Flush();
            Response.End();
        }
    }
}