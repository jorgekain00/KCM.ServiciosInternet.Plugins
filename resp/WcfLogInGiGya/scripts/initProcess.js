/// <reference path="loadpage.js" />
/// <reference path="entities.js" />

let objLogicaSessionPlugInLogIn = {};

function gigyaJFMIngreso() {
    LogicaSessionPlugInLogIn.goToSignUpControl()
}

document.addEventListener('readystatechange', evt => {
    if (document.readyState == "complete") {
        console.log('gigyaJFM -  Inicia PlugIn')
        objLogicaSessionPlugInLogIn = new LogicaSessionPlugInLogIn();
        objLogicaSessionPlugInLogIn.showLoadControl(false);
        let buttonSignMB = document.getElementById('gigyJFMLogInMB');
        //console.log(buttonSignMB);
        //buttonSignMB.addEventListener('click', gigyaJFMIngreso, false);
    }
}, false);


window.addEventListener('message', function (evt) {
    if (evt.origin.indexOf('jaboneskleenex.mx') != -1) {
        objLogicaSessionPlugInLogIn.receivePostMessageFromWindow(evt);
    }
}, false);

window.addEventListener('beforeunload', function (event) {
    var strMessage = 'I�m really going to miss you if you go.';
    objLogicaSessionPlugInLogIn.closeWindowPlugIn();
    event.returnValue = message;
    return message;
});

