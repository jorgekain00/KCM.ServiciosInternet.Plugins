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
            gonmes

            <button id="entradaDatos" >click aqui</button>
        </div>
    </form>
    <script>
        window.addEventListener('message', function (evt) {
            alert('vamos jorge:' + evt.origin)    
            //if (!!evt.origin) {
                alert(evt.data);
                evt.source.postMessage("Received!!", "*");
            //}
        },false)
        entradaDatos.addEventListener('click', function () {

            if (typeof window.opener.postMessage == 'function') {
                alert('no ways');
                window.opener.postMessage('unomas','*');
            }
            else {
                window.opener.unomas('unomas');
            }
            
            alert('no way');

            //document.location.href = "http://www.google.com";
        }, false);
    </script>
</body>
</html>
