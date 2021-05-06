<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AfterLogin.aspx.cs" Inherits="WcfLogInGiGya._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
                done by Eng. Jorge Flores Miguel

        </div>
    </form>
    <script type="text/javascript">

        let strAccessToken = '<%:this.strAccessToken%>',
            strError = '<%:this.strError%>',
            strErrorDescription = '<%:this.strErrorDescription%>';
		
        let objResp = {
            MSG: "SocialLogin:completed",
            ERROR: strError,
            ERRORDESCRIPTION: strErrorDescription,
            ACCESSTOKEN: strAccessToken
        };
		
        if (typeof window.opener.postMessage == 'function') {
            window.opener.postMessage(JSON.stringify(objResp),location.href);
        }
    </script>
</body>
</html>
