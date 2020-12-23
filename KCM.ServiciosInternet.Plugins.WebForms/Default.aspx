<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KCM.ServiciosInternet.Plugins.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

    <button id="presiona">presioname</button>

    <script>
        var presiona = document.getElementById('presiona');

        presiona.addEventListener('click', function (evt) {
            var xhr = new XMLHttpRequest();
            xhr.addEventListener('readystatechange', function () {
                if (xhr.readyState == 4) {
                    if ((xhr.status >= 200 && xhr.status < 300) || xhr.status == 304) {
                        postContenido.textContent = xhr.responseText;
                        console.log(xhr.getAllResponseHeaders().split('\n'));
                    }
                    else
                        contenido.textContent = "Request was unsuccessful: " + xhr.status;
                }
            }, false);

            //xhr.open("post", "https://localhost:443/projects/create", true);
            xhr.open("post", "http://localhost:441/Service.svc/GetData", true);
            xhr.setRequestHeader("Content-Type", "application/json");
            //xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            var prueba = new clsDataTransfer()
            prueba.strEmail = 'jorge_kain@yahoo.com';

            xhr.send(JSON.stringify(prueba));
            evt.preventDefault();
        }, false);

        function clsDataTransfer() {
            this.strEmail = '';
            this.strPassword = '';
            this.strProfile = '';
            this.strExtraProfileFields = '';
            this.strExtraProfileFieldsDescriptor = '';
            this.boolIsInvalidRequest = false;
            this.strSessionCookie = '';
            this.boolIsAccountPendingRegistration = false;
            this.boolIsSocialLogin = false;
            this.strErrormessage = '';
            this.strRandomKey = '';
            this.strComputedKey = '';
            this.strUID = '';
            this.strRegToken = '';
            this.intExpirationSessionDays = 7;
            this.strCaptchaToken = '';
            this.boolIsCompletedOperation = false;
        }
    </script>
</asp:Content>
