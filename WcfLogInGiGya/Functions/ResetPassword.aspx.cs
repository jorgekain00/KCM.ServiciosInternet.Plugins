using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KCM.ServiciosInternet.Plugins.Business;
using KCM.ServiciosInternet.Plugins.Data.SSO.Config;
using KCM.ServiciosInternet.Plugins.Data.SSO.HTML;

namespace WcfLogInGiGya.Functions
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strHTMLOutput = null; // html template 

            if (!this.Page.IsPostBack)
            {
                string strRegToken = Request.QueryString["regToken"];
                string strEmail = Request.QueryString["email"];
                string strApiKey = Request.QueryString["apiKey"];

                if (string.IsNullOrEmpty(strRegToken) || string.IsNullOrEmpty(strEmail) || string.IsNullOrEmpty(strApiKey))
                {
                    // TODO: Improvemente - like redirect
                    // Show Not found page if any missing token
                }
                else {
                    htmlPlugIn ObjHtml = new htmlPlugIn();
                    ObjHtml.enumState = htmlPlugIn.templateCtrl.RESETFORM;
                    ObjHtml.strApiID = strApiKey;
                    ObjHtml.strHtml = string.Empty;

                    ObjHtml = BussinessSSO.getHtml(ObjHtml, GlobalConfig.strDataPath);   // get template
                    strHTMLOutput = ObjHtml.strHtml.Replace("${|Email|}", strEmail).Replace("${|RegToken|}", strRegToken);

                    Response.Clear();
                    Response.ContentType = "text/html";
                    Response.Write(strHTMLOutput);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}