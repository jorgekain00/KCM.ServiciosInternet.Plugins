function JFMGigya_EnviaCorreo(evt, strPanelResetPS, strErrorClassLabel, strErrorClassLabelPS, strIdPassWordField) {
    let strServiceMethod = 'requestResetPassword';              // service method
    let strServiceName = '${|ServiceName|}';                      // service url
    let objResetPSScreen = document.getElementById(strPanelResetPS);     // Reset PS panel
    let strUrlImg = '${|UrlImg|}';

    let objDIV = document.createElement('div');
    objDIV.style.cssText = "width:100%;height:100%;background-color: black;opacity: 0.5;z-index:1000000013;position:absolute;top:0;left:0";

    let objIMG = document.createElement('img');
    objIMG.src = strUrlImg;
    objIMG.style.cssText = "margin:44%;position:absolute";
    objDIV.appendChild(objIMG);

    evt.preventDefault();    // avoid reset password via gigya interface

    if (objResetPSScreen) {
        let objLabelError = Array.prototype.slice.call(objResetPSScreen.getElementsByClassName(strErrorClassLabel), 0)
            .find((e) => e.dataset.boundTo == strErrorClassLabelPS);        //get error label
        let objEmailField = document.getElementById(strIdPassWordField);    // email field
        let objGrecaptcha = grecaptcha.getResponse();                       // get recaptcha value

        objLabelError.style.visibility = 'visible';                         //Enable error messages

        // validate email
        if (!objEmailField.value) {
            objLabelError.innerText = 'Ingrese un correo válido';
        }
        else if (!objGrecaptcha) {
            objLabelError.innerText = 'Debe verificar recaptcha';
        }
        else {
            objLabelError.innerText = '';

            let objXHR = new XMLHttpRequest();

            objXHR.addEventListener('readystatechange', function () {
                if (objXHR.readyState == 4) {
                    objResetPSScreen.removeChild(objDIV);
                    if (objXHR.status == 200 || objXHR.status == 304) {
                        let objResp = JSON.parse(objXHR.responseText);
                        if (objResp.boolIsInvalidRequest) {
                            objLabelError.innerText = objResp.strErrormessage;
                        }
                        else {
                            objResetPSScreen.innerHTML = "<span>Correo enviado a su buzón de correo</span>";
                        }
                    }
                    else
                        alert('El servicio no pudo llevarse a acabo: status' + objXHR.statusText + ' - detail error:' + objXHR.responseText);
                }
            }, false);

            objXHR.open("post", strServiceName + '/' + strServiceMethod, true);
            objXHR.setRequestHeader("Content-Type", "application/json");
            objXHR.send(JSON.stringify({
                strEmail: objEmailField.value,
                recatchapToken: objGrecaptcha
            }));
            objResetPSScreen.appendChild(objDIV);
        }
    }
}
