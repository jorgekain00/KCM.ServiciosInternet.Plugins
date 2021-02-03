

class DataTransferCom {

    constructor() {
        this.strEmail = '';
        this.strPassword = '';
        this.strProfile = '';
        this.strData = '';
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

    setData(obj) {
        Object.assign(this, this, obj);
    }

    setDataFromJSON(strValues) {
        this.setData(JSON.parse(strValues));
    }

    getJSONData() {
        return JSON.stringify(this);
    }

    isPreviousLogIn(objLocalStorageSession) {
        return objLocalStorageSession.getLocalStorage();
    }
};

class LocalStorageSession {

    strUID;
    dtLastAccess;
    strRegToken;
    intExpirationSessionDays;
    isPendingRegistration;


    setLocalStorage() {
        window.localStorage.setItem('gigyaJFMData', JSON.stringify(this));
    }

    getLocalStorage() {
        let objGigyaTemp = window.localStorage.getItem('gigyaJFMData');
        if (objGigyaTemp !== null) {
            Object.assign(this, this, JSON.parse(objGigyaTemp));
            return true;
        }
        return false;
    }

    removeLocalStorage() {
        window.localStorage.removeItem('gigyaJFMData');
    }
};

class AJAXService {

    constructor(strURL) {
        this.strURL = strURL;
        this.objHandleLoadingImage = null;
        this.objHTMLLoading = null;
        this.objDivHTML = document.createElement('div');
        this.objDivHTML.id = 'gigyaJFMLoadingImage';
        this.objDivHTML.style.cssText = "background-color:black;opacity:0.5;z-index: 1000000007";
        this.objDivHTML.style.width = "100%";
        this.objDivHTML.style.height = "100%";
        this.objDivHTML.style.position = "fixed";
        this.objDivHTML.style.top = "0";

        let objImgHTML = document.createElement('img');
        objImgHTML.src = '/gigyaJFM/images/icon-loading.gif';
        objImgHTML.style = "padding-top: 250px;margin: auto;display: block;z-index: 1000000008;opacity: 1;top: 200px;"
        this.objDivHTML.appendChild(objImgHTML);
    }

    initLoadingImage(boolWithLoadingImage) {
        if (boolWithLoadingImage) {
            this.objHandleLoadingImage = document.getElementById('gigyaJFMLoadingImage');
            if (this.objHandleLoadingImage) {
                this.destroyLoadingIMage(boolWithLoadingImage)
            }
            document.body.appendChild(this.objDivHTML);
        }
    }

    destroyLoadingIMage(boolWithLoadingImage) {
        if (boolWithLoadingImage) {
            this.objHandleLoadingImage = document.getElementById('gigyaJFMLoadingImage');
            if (this.objHandleLoadingImage) {
                document.body.removeChild(this.objHandleLoadingImage);
            }
        }
    }

    callService(objLogicaSessionPlugInLogIn, objDataTransfer, strMethod, fnCallBack, boolWithLoadingImage=false) {
        let objXHR = new XMLHttpRequest();
        let objTHIS = this;

        objXHR.addEventListener('readystatechange', function () {
            if (objXHR.readyState == 4) {
                objTHIS.destroyLoadingIMage(boolWithLoadingImage);
                if (objXHR.status == 200 || objXHR.status == 304) {
                    if (fnCallBack) {
                        fnCallBack.call(objLogicaSessionPlugInLogIn, objXHR.responseText);
                    }
                }
                else
                    alert('El servicio no pudo llevarse a acabo: status' + objXHR.statusText + ' - detail error:' + objXHR.responseText);
            }
        }, false);

        objXHR.open("post", this.strURL + strMethod, true);
        objXHR.setRequestHeader("Content-Type", "application/json");
        this.initLoadingImage(boolWithLoadingImage);
        objXHR.send(objDataTransfer.getJSONData());

    }

    callServiceToGetHTML(objLogicaSessionPlugInLogIn, strControl, strMethod, fnCallBack, boolWithLoadingImage=false) {
        let objXHR = new XMLHttpRequest();
        let objTHIS = this;
        objXHR.addEventListener('readystatechange', function () {
            if (objXHR.readyState == 4) {
                objTHIS.destroyLoadingIMage(boolWithLoadingImage);
                if (objXHR.status == 200 || objXHR.status == 304) {
                    fnCallBack.call(objLogicaSessionPlugInLogIn, objXHR.responseText);
                }
                else
                    alert('El servicio no pudo llevarse a acabo: status' + objXHR.statusText + ' - detail error:' + objXHR.responseText);
            }
        }, false);

        objXHR.open("post", this.strURL + strMethod, true);
        objXHR.setRequestHeader("Content-Type", "application/json");
        this.initLoadingImage(boolWithLoadingImage);
        objXHR.send(JSON.stringify({ enumState: strControl }));
    }

};

class WidgetLogIn {
    constructor() {
        this.strLoginName = 'undefined';
        this.strLogout = 'undefined';
        this.boolValid = true;
        this.strMsgErr = '';
        this.objCtrlFields = {};  // depending the fields captured in the form
        this.objSendButton = {};
        this.objSocialGmailLink = {
            objHTML: null,
            strUrl: 'https://socialize.us1.gigya.com/accounts.socialLogin?x_provider=googleplus&x_lang=es-mx&x_regSource=https%3A%2F%2Fwww.jaboneskleenex.mx%2F&x_sessionExpiration=1200&x_enabledProviders=facebook%2Ctwitter%2Cgoogleplus&x_finalizeRegistration=true&x_include=profile%2Cdata%2Cemails%2Csubscriptions%2Cpreferences%2C&x_includeUserInfo=true&x_loginMode=standard&x_apiDomain=us1.gigya.com&x_source=showScreenSet&x_sdk=js_latest&client_id=3_tZ5kn2zQOZSMJ2firwgDhbob4CbC7wsA26bGt3IRct2imQ6q4fw4zYE5Z0G3Gaj6&redirect_uri=https%3A%2F%2Fdev1.jaboneskleenex.mx%2Fsso%2FAfterLogin.aspx&response_type=server_token&state=domain%3Dhttps%253A%252F%252Fwww.jaboneskleenex.mx%252F%26lid%3Dflid1604682925633%26messaging%3D1%26id%3Daccounts_socialLogin_16047013555901604701355590&authMode=cookie'
        };
        this.objSocialFacebookLink = {
            objHTML: null,
            strUrl: 'https://socialize.us1.gigya.com/accounts.socialLogin?x_provider=facebook&x_lang=es-mx&x_regSource=https%3A%2F%2Fwww.jaboneskleenex.mx%2F&x_sessionExpiration=1200&x_enabledProviders=facebook%2Ctwitter%2Cgoogleplus&x_finalizeRegistration=true&x_include=profile%2Cdata%2Cemails%2Csubscriptions%2Cpreferences%2C&x_includeUserInfo=true&x_loginMode=standard&x_apiDomain=us1.gigya.com&x_source=showScreenSet&x_sdk=js_latest&client_id=3_tZ5kn2zQOZSMJ2firwgDhbob4CbC7wsA26bGt3IRct2imQ6q4fw4zYE5Z0G3Gaj6&redirect_uri=https%3A%2F%2Fdev1.jaboneskleenex.mx%2Fsso%2FAfterLogin.aspx&response_type=server_token&state=domain%3Dhttps%253A%252F%252Fwww.jaboneskleenex.mx%252F%26lid%3Dflid1604682925633%26messaging%3D1%26id%3Daccounts_socialLogin_16047086408001604708640800&authMode=cookie'
        };
        this.objSocialTwitterLink = {
            objHTML: null,
            strUrl: 'https://socialize.us1.gigya.com/accounts.socialLogin?x_provider=twitter&x_lang=es-mx&x_regSource=https%3A%2F%2Fwww.jaboneskleenex.mx%2F&x_sessionExpiration=1200&x_enabledProviders=facebook%2Ctwitter%2Cgoogleplus&x_finalizeRegistration=true&x_include=profile%2Cdata%2Cemails%2Csubscriptions%2Cpreferences%2C&x_includeUserInfo=true&x_loginMode=standard&x_apiDomain=us1.gigya.com&x_source=showScreenSet&x_sdk=js_latest&client_id=3_tZ5kn2zQOZSMJ2firwgDhbob4CbC7wsA26bGt3IRct2imQ6q4fw4zYE5Z0G3Gaj6&redirect_uri=https%3A%2F%2Fdev1.jaboneskleenex.mx%2Fsso%2FAfterLogin.aspx&response_type=server_token&state=domain%3Dhttps%253A%252F%252Fwww.jaboneskleenex.mx%252F%26lid%3Dflid1604682925633%26messaging%3D1%26id%3Daccounts_socialLogin_16047086698651604708669865&authMode=cookie'
        };

        this.strHTMLLOAD = {
            id: 'gigyaJFMLOADCtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {}
        };
        this.strHTMLLOGIN = {
            id: 'gigyaJFMLOGINCtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {
                recaptcha1: {
                    widgetId: '',
                    id: 'gigyaJFMsignUprecaptcha',
                    parameters: {
                        sitekey: '6LeoG1UUAAAAAOU6oPBHxkyRyfER2WuVNhxlumUs',
                        callback: LogicaSessionPlugInLogIn.validateRecaptch
                    }
                },
                recaptcha2: {
                    widgetId: '',
                    id: 'gigyaJFMlogInrecaptcha',
                    parameters: {
                        sitekey: '6LfEGVUUAAAAAGlDorUbOF5S2fhALwihqQhA7oRJ',
                        callback: LogicaSessionPlugInLogIn.validateRecaptch
                    }
                }
            }
        };
        this.strHTMLCONTINUE = {
            id: 'gigyaJFMCONTINUECtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {}
        };
        this.strHTMLCOMPLETION = {
            id: 'gigyaJFMCOMPLETIONCtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {
                recaptcha1: {
                    widgetId: '',
                    id: 'gigyaJFMCompletionrecaptcha',
                    parameters: {
                        sitekey: '6LfEGVUUAAAAAGlDorUbOF5S2fhALwihqQhA7oRJ',
                        callback: LogicaSessionPlugInLogIn.validateRecaptch
                    }
                }
            }
        };
        this.strHTMLRESETPS = {
            id: 'gigyaJFMRESETPSCtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {
                recaptcha1: {
                    widgetId: '',
                    id: 'gigyaJFMresetPSrecaptcha',
                    parameters: {
                        sitekey: '6LfEGVUUAAAAAGlDorUbOF5S2fhALwihqQhA7oRJ',
                        callback: LogicaSessionPlugInLogIn.validateRecaptch
                    }
                }
            }
        };
        this.strHTMLSIGNIN = {
            id: 'gigyaJFMSIGNINCtrl',
            strHTML: '',
            boolOpenCtrl: false,
            recaptchaIds: {}
        };
    }
};


const StatesGigya = {
    LOAD: 'LOAD',
    LOGIN: 'LOGIN',
    LOGINWITHEMAIL: 'LOGINWITHEMAIL',
    SOCIALLOGIN: 'SOCIALLOGIN',
    COMPLETION: 'COMPLETION',
    CONTINUE: 'CONTINUE',
    SIGNIN: 'SIGNIN',
    RESETPS: 'RESETPS',
    LOGOUT: 'LOGOUT'
};