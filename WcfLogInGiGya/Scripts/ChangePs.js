function JFMGigya_ChangePassword(evt) {
    evt.preventDefault();    // avoid reset password via gigya interface

    let strServiceMethod = 'changePassword';              // service method
    let strServiceName = '${|ServiceName|}';                      // service url
    let strUrlImg = '${|UrlImg|}';

    let objDIV = document.createElement('div');
    objDIV.style.cssText = "width:100%;height:100%;background-color: black;opacity: 0.5;z-index:1000000013;position:absolute;top:0;left:0";

    let objIMG = document.createElement('img');
    objIMG.src = strUrlImg;
    objIMG.style.cssText = "margin-top:20%;margin-left:50%;position:absolute;top:0;left:0";
    objDIV.appendChild(objIMG);

    let objColInputs = Array.prototype.slice.call(document.getElementsByTagName("input"), 0);
    let objPassField = objColInputs.find((e) => e.dataset.ctrl == "JFMGigyaPassCtrl");
    let objConfirmField = objColInputs.find((e) => e.dataset.ctrl == "JFMGigyaConfirmCtrl");
    let objEmailField = objColInputs.find((e) => e.dataset.ctrl == "JFMGigyaEmail");
    let objRegTokenField = objColInputs.find((e) => e.dataset.ctrl == "JFMGigyaRegToken");
    let objSiteUrl = objColInputs.find((e) => e.dataset.ctrl == "JFMGigyaSiteUrl");
    let objSubmit = document.getElementById('JFMGigyaSubmit');
    let strRecatchap = grecaptcha.getResponse();      

    let objErrorLabel = Array.prototype.slice.call(document.getElementsByTagName("span"), 0).find((e) => e.dataset.ctrl == "JFMGigyaErrorMessage");

    if (objPassField && objPassField.value == '') {
        objPassField.classList.add("JFMError");
        objErrorLabel.innerText = 'Debe ingresar una contraseña válida';
        objPassField.focus();
    }
    else if (objConfirmField && objConfirmField.value == '') {
        objPassField.classList.remove("JFMError");
        objConfirmField.classList.add("JFMError");
        objErrorLabel.innerText = 'Debe confirmar la contraseña';
        objPassField.focus();
    }
    else if (objPassField.value != objConfirmField.value) {
        objPassField.classList.add("JFMError");
        objConfirmField.classList.add("JFMError");
        objErrorLabel.innerText = 'Las contraseñas no son iguales';
    }
    else if(!strRecatchap)
    {
        objErrorLabel.innerText = 'Debe verificar recaptcha';
    }
    else {
        objPassField.classList.remove("JFMError");
        objConfirmField.classList.remove("JFMError");
        objErrorLabel.innerText = '';

        let objXHR = new XMLHttpRequest();

        objXHR.addEventListener('readystatechange', function () {
            if (objXHR.readyState == 4) {
                document.body.removeChild(objDIV);
                if (objXHR.status == 200 || objXHR.status == 304) {
                    let objResp = JSON.parse(objXHR.responseText);
                    if (objResp.boolIsInvalidRequest) {
                        objErrorLabel.innerText = objResp.strErrormessage;
                    }
                    else {
                        objSubmit.style.display = "none";
                        objErrorLabel.innerHTML = "<span style='color:blue'>Contraseña cambiada, Será redireccionado a la brevedad...</span>";
                        setTimeout("window.location ='" + objSiteUrl.value +"';" ,5000);
                    }
                }
                else
                    alert('El servicio no pudo llevarse a acabo: status' + objXHR.statusText + ' - detail error:' + objXHR.responseText);
            }
        }, false);

        objXHR.open("post", strServiceName + '/' + strServiceMethod, true);
        objXHR.setRequestHeader("Content-Type", "application/json");
        objXHR.send(JSON.stringify({
            strPassword: objPassField.value,
            strEmail: objEmailField.value,
            strGigyaTokenForResetPS: objRegTokenField.value,
            recatchapToken: strRecatchap
        }));

        document.body.appendChild(objDIV);
    }

}