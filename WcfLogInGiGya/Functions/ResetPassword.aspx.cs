using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities = KCM.ServiciosInternet.Plugins.Entities;


namespace WcfLogInGiGya.Functions
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string strRegToken = Request.QueryString["regToken"];
                string strEmail = Request.QueryString["email"];

                if (string.IsNullOrEmpty(strRegToken) || string.IsNullOrEmpty(strEmail))
                {
                    Response.Redirect(Entities.Config.PlugInConfig.strSiteUrl);
                }

                string strHTMLPath = Server.MapPath("~/Html");
                string strHTMLReset = Entities.Config.PlugInConfig.strResetHTML;
                string strHTMLOutput = System.IO.File.ReadAllText(System.IO.Path.Combine(strHTMLPath, strHTMLReset))
                    .Replace("${|Email|}", strEmail)
                    .Replace("${|RegToken|}", strRegToken)
                    .Replace("${|SiteUrl|}", Entities.Config.PlugInConfig.strSiteUrl);

                Response.Clear();
                Response.ContentType = "text/html";
                Response.Write(strHTMLOutput);
                Response.Flush();
                Response.End();
            }
        }
    }
}