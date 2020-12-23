/// <reference path="entities.js" />
/// <reference path="initprocess.js" />



class LogicaSessionPlugInLogIn {
    static boolIsValidCatchap = false;

    constructor() {
        this.objAJAXService = new AJAXService("http://localhost:15467/Service1.svc/");
        //this.objAJAXService = new AJAXService("http://localhost:441/Service1.svc/");
        //this.objAJAXService = new AJAXService("https://dev1.jaboneskleenex.mx/sso/Service1.svc/");
        this.objWindow = null;
        this.enumState = StatesGigya.LOAD;
        this.objLocalStorageSession = new LocalStorageSession();
        this.objDataTransfer = new DataTransferCom();
        this.objWidgetLogIn = new WidgetLogIn();
        this.strControlClasses = 'gigyaJFMCtrl';
        this.strClassForHiding = 'gigyaJFMOcultaControl';
        this.strClassForShowing = 'gigyaJFMMuestraControl';

        this.strIDCtrl = null;
        this.strClassAnchor = null;
        this.strControlLoading = null;
        this.funcInitializer = null;
        this.boolIsPostBack = false;
        this.boolIsSocialLogin = false;
    }

    // #region Métodos para el estado LOAD

    showLoadControl(boolIsPostBack) {
        this.boolIsPostBack = boolIsPostBack;
        this.enumState = 'LOAD';
        this.funcInitializer = this.initShowLoadControl;
        this.loadControl([StatesGigya.LOAD], StatesGigya.LOAD, false)
    }

    initShowLoadControl() {
        let objHTML;

        if (this.objDataTransfer.isPreviousLogIn(this.objLocalStorageSession)) {
            this.objDataTransfer.strUID = this.objLocalStorageSession.strUID;
            this.objDataTransfer.strRegToken = this.objLocalStorageSession.strRegToken;
            this.objAJAXService.callService(this, this.objDataTransfer, 'getAccountInfoCompleteRegistration', this.setUsernameFromLoadPage);
        }
        else {
            this.objWidgetLogIn.strLoginName = 'LogIn/SignIn';
            this.objWidgetLogIn.strLogout = '';

            objHTML = document.getElementById(this.objWidgetLogIn.strHTMLLOAD.id);

            if (objHTML) {
                objHTML.getElementsByTagName('a')[0].innerHTML = this.objWidgetLogIn.strLoginName;
                objHTML.getElementsByTagName('a')[1].innerHTML = this.objWidgetLogIn.strLogout;
                this.initEventsLoadControl(objHTML);
            }
        }

        if (!this.boolIsPostBack) {
            let objGoogleAPI = document.createElement('script');
            objGoogleAPI.id = "gigyaJFMGoogleRecaptcha"
            objGoogleAPI.src = 'https://www.google.com/recaptcha/api.js';
            //objGoogleAPI.src = 'https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit';
            objGoogleAPI.async = true;
            objGoogleAPI.defer = true;
            document.getElementsByTagName('head')[0].appendChild(objGoogleAPI);
        }
    }

    setUsernameFromLoadPage(strResponseText) {
        let objProfile;
        let objHTML;

        this.objDataTransfer.setDataFromJSON(strResponseText);

        if (this.objDataTransfer.strErrormessage != 'OK') {
            alert('Se presento un error que impide continuar - MSG: ' + this.objDataTransfer.strErrormessage);
            return;
        }

        /**
         * Is session expired?
        **/
        this.objLocalStorageSession.strUID = this.objDataTransfer.strUID;
        this.objLocalStorageSession.intExpirationSessionDays = this.objDataTransfer.intExpirationSessionDays;
        this.objLocalStorageSession.isPendingRegistration = this.objDataTransfer.boolIsAccountPendingRegistration;


        let dtLocalDate = new Date(this.objLocalStorageSession.dtLastAccess);
        let intAddMinutes = dtLocalDate.getMinutes() + this.objDataTransfer.intExpirationSessionDays;
        let objNewDate = new Date(dtLocalDate.setMinutes(intAddMinutes));
        let objToday = new Date();

        if (objToday > objNewDate) {
            this.objLocalStorageSession.removeLocalStorage();
            this.objWidgetLogIn.strLoginName = 'LogIn/SignIn';
            this.objWidgetLogIn.strLogout = '';
        }
        else {
            objProfile = JSON.parse(this.objDataTransfer.strProfile);
            this.objWidgetLogIn.strLoginName = objProfile.email;
            this.objWidgetLogIn.strLogout = 'Logout';
            this.objLocalStorageSession.dtLastAccess = new Date();
            this.objLocalStorageSession.setLocalStorage();
        }

        objHTML = document.getElementById(this.objWidgetLogIn.strHTMLLOAD.id);
        if (objHTML) {
            objHTML.getElementsByTagName('a')[0].innerHTML = this.objWidgetLogIn.strLoginName;
            objHTML.getElementsByTagName('a')[1].innerHTML = this.objWidgetLogIn.strLogout;
            this.initEventsLoadControl(objHTML);
        }

        if (this.boolIsSocialLogin) {
            this.boolIsSocialLogin = false;
            if (this.objLocalStorageSession.isPendingRegistration) {
                this.goToThankyouPage();
            }
        }
    }


    initEventsLoadControl(objHTML) {
        objHTML.getElementsByTagName('a')[0].removeEventListener('click', LogicaSessionPlugInLogIn.goToSignUpControl, false);
        objHTML.getElementsByTagName('a')[1].removeEventListener('click', LogicaSessionPlugInLogIn.goTocloseSession, false);

        objHTML.getElementsByTagName('a')[0].addEventListener('click', LogicaSessionPlugInLogIn.goToSignUpControl, false);
        objHTML.getElementsByTagName('a')[1].addEventListener('click', LogicaSessionPlugInLogIn.goTocloseSession, false);
    }

    // #endregion

    // #region Métodos para el estado RESETPS

    showForgotPaswordCtrl() {
        this.funcInitializer = this.initResetPasswordControlEvents;
        this.loadControl([StatesGigya.LOGINWITHEMAIL], StatesGigya.RESETPS, true);
    }

    initResetPasswordControlEvents() {
        let objHTMLLinks = document.getElementsByClassName('gigyaJFMLink');
        if (objHTMLLinks) {
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.removeEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.addEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }
        let objHTMLSubmit = document.getElementsByClassName('gigyaJFMform-detail');
        if (objHTMLSubmit) {
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.removeEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.addEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }

        document.getElementById('gigyaJFMClosingResetPs').removeEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById('gigyaJFMClosingResetPs').addEventListener('click', this.closeWindowPlugIn, false);

        this.objWidgetLogIn.objCtrlFields = {
            objemailText: document.getElementById('gigyaJFMemailResetPs'),
            objerrFormSignUpSpan: document.getElementById('gigyaJFMerrFormResetPS')
        }

        this.objWidgetLogIn.objSendButton = document.getElementById('gigyaJFMBTNResetPs');

        this.objWidgetLogIn.objCtrlFields.objemailText.value = '';
        this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.value = '';

        //this.objWidgetLogIn.objCtrlFields.objemailText.removeEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objSendButton.removeEventListener('click', this.validateResetPSForm, false);

        //this.objWidgetLogIn.objCtrlFields.objemailText.addEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objSendButton.addEventListener('click', this.validateResetPSForm, false);
        this.objAJAXService.callService(this, this.objDataTransfer, 'generateRandomKeyForLogIn', this.receiveRandomKey);
    }

    validateResetPSForm(evt) {
        //objLogicaSessionPlugInLogIn.lostFocusEmail();
        objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = true;
        if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
            objLogicaSessionPlugInLogIn.objDataTransfer.strCaptchaToken = LogicaSessionPlugInLogIn.validateRecaptcha();
            if (LogicaSessionPlugInLogIn.boolIsValidCatchap) {
                objLogicaSessionPlugInLogIn.requestResetPassword();
            }
            else {
                objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Debes verificar reCAPTCHA...';
            }
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    requestResetPassword() {
        this.objDataTransfer.strEmail = this.objWidgetLogIn.objCtrlFields.objemailText.value;
        this.objDataTransfer.strPassword = '';
        this.objDataTransfer.strProfile = '';
        this.objDataTransfer.strData = '';
        this.objDataTransfer.strExtraProfileFields = '';
        this.objDataTransfer.strExtraProfileFieldsDescriptor = '';
        this.objDataTransfer.boolIsInvalidRequest = false;
        this.objDataTransfer.strSessionCookie = '';
        this.objDataTransfer.boolIsAccountPendingRegistration = false;
        this.objDataTransfer.boolIsSocialLogin = false;
        this.objDataTransfer.strErrormessage = '';
        this.objDataTransfer.strUID = '';
        this.objDataTransfer.strRegToken = '';
        this.objDataTransfer.intExpirationSessionDays = 7;
        this.objDataTransfer.boolIsCompletedOperation = false;

        this.objAJAXService.callService(this, this.objDataTransfer, 'requestResetPassword', this.processRequestResetPasswordAceptance, true);
    }

    processRequestResetPasswordAceptance(strResponseText) {
        this.objDataTransfer.setDataFromJSON(strResponseText);

        if (this.objDataTransfer.strErrormessage != 'OK') {
            if (this.objDataTransfer.boolIsInvalidRequest) {
                alert('Se presento un error que impide continuar - MSG: ' + this.objDataTransfer.strErrormessage);
                // Cierre de la ventana sin continuar el flujo
                this.closeWindowPlugIn();
                return;
            }
            else {
                this.objWidgetLogIn.strMsgErr = this.objDataTransfer.strErrormessage;
                this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = this.objWidgetLogIn.strMsgErr;
                grecaptcha.reset(this.objWidgetLogIn.strHTMLRESETPS.recaptchaIds.recaptcha1.widgetId);
                this.getcomputedKey(this.objDataTransfer.strRandomKey).then(x => this.getcomputedKey(x).then(x => this.objDataTransfer.strComputedKey = x));
            }
        }
        else {
            alert('Debera recibir un correo con instrucciones de como cambiar el password');
            this.closeWindowPlugIn();
        }
    }

    // #endregion

    // #region Métodos para el estado LOGIN

    initLogInEvents() {
        this.objWidgetLogIn.objCtrlFields = {
            objemailText: document.getElementById('gigyaJFMEmailLogIn'),
            objPasswordText: document.getElementById('gigyaJFMPasswordLogIn'),
            objForgotPSLink: document.getElementById('gigyaJFMForgotPsLogIn'),
            objerrFormSignUpSpan: document.getElementById('gigyaJFMerrFormlogIn')
        };

        this.objWidgetLogIn.objSendButton = document.getElementById('gigyaJFMBTNLogIn');

        this.objWidgetLogIn.objCtrlFields.objemailText.value = '';
        this.objWidgetLogIn.objCtrlFields.objPasswordText.value = '';
        this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.value = '';

        this.objWidgetLogIn.objCtrlFields.objemailText.removeEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objCtrlFields.objForgotPSLink.removeEventListener('click', LogicaSessionPlugInLogIn.goFromLogInToForgotPassword, false);
        this.objWidgetLogIn.objSendButton.removeEventListener('click', this.validateLogInForm, false);

        this.objWidgetLogIn.objCtrlFields.objemailText.addEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objCtrlFields.objForgotPSLink.addEventListener('click', LogicaSessionPlugInLogIn.goFromLogInToForgotPassword, false);
        this.objWidgetLogIn.objSendButton.addEventListener('click', this.validateLogInForm, false);
        this.objAJAXService.callService(this, this.objDataTransfer, 'generateRandomKeyForLogIn', this.receiveRandomKey);
    }

    validateLogInForm() {
        objLogicaSessionPlugInLogIn.lostFocusEmail();
        if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
            objLogicaSessionPlugInLogIn.validateRequieredPasswordField()
            if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
                objLogicaSessionPlugInLogIn.objDataTransfer.strCaptchaToken = LogicaSessionPlugInLogIn.validateRecaptcha();
                if (LogicaSessionPlugInLogIn.boolIsValidCatchap) {
                    objLogicaSessionPlugInLogIn.sendCredentialesToLogIn();
                }
                else {
                    objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Debes verificar reCAPTCHA...';
                }
            }
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    sendCredentialesToLogIn() {
        this.objDataTransfer.strEmail = this.objWidgetLogIn.objCtrlFields.objemailText.value;
        this.objDataTransfer.strPassword = this.objWidgetLogIn.objCtrlFields.objPasswordText.value;
        this.objDataTransfer.strProfile = '';
        this.objDataTransfer.strData = '';
        this.objDataTransfer.strExtraProfileFields = '';
        this.objDataTransfer.strExtraProfileFieldsDescriptor = '';
        this.objDataTransfer.boolIsInvalidRequest = false;
        this.objDataTransfer.strSessionCookie = '';
        this.objDataTransfer.boolIsAccountPendingRegistration = false;
        this.objDataTransfer.boolIsSocialLogin = false;
        this.objDataTransfer.strErrormessage = '';
        this.objDataTransfer.strUID = '';
        this.objDataTransfer.strRegToken = '';
        this.objDataTransfer.intExpirationSessionDays = 7;
        this.objDataTransfer.boolIsCompletedOperation = false;

        this.objAJAXService.callService(this, this.objDataTransfer, 'sendCredentialesToLogIn', this.processSignUpLogInAceptance, true);
    }



    // #endregion

    // #region thank you page
    goToThankyouPage() {
        this.funcInitializer = this.initThankYoupageControlEvents;
        this.loadControl([StatesGigya.LOGIN, StatesGigya.LOGINWITHEMAIL, StatesGigya.SOCIALLOGIN], StatesGigya.CONTINUE, true);
    }


    initThankYoupageControlEvents() {
        let objHTMLLinks = document.getElementsByClassName('gigyaJFMLink');
        if (objHTMLLinks) {
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.removeEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.addEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }
        let objHTMLSubmit = document.getElementsByClassName('gigyaJFMform-detail');
        if (objHTMLSubmit) {
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.removeEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.addEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }

        document.getElementById('gigyaJFMCONTINUEClosing').removeEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById('gigyaJFMContinueSalidaBTN').removeEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById('gigyaJFMCONTINUEClosing').addEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById('gigyaJFMContinueSalidaBTN').addEventListener('click', this.closeWindowPlugIn, false);

        this.objWidgetLogIn.objCtrlFields = {
        }

        this.objWidgetLogIn.objSendButton = document.getElementById('gigyaJFMContinueBTN');

        this.objWidgetLogIn.objSendButton.removeEventListener('click', LogicaSessionPlugInLogIn.goFromThankyouPageToCompletionPage, false);

        this.objWidgetLogIn.objSendButton.addEventListener('click', LogicaSessionPlugInLogIn.goFromThankyouPageToCompletionPage, false);
    }
    // #endregion

    // #region métodos para CompletionPage


    showCompletionPage() {
        this.funcInitializer = this.initCompletionRegistrationControlEvents;
        this.loadControl([StatesGigya.CONTINUE], StatesGigya.COMPLETION, true);
    }

    initCompletionRegistrationControlEvents() {
        let objHTMLLinks = document.getElementsByClassName('gigyaJFMLink');
        if (objHTMLLinks) {
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.removeEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.addEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }
        let objHTMLSubmit = document.getElementsByClassName('gigyaJFMform-detail');
        if (objHTMLSubmit) {
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.removeEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.addEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }

        document.getElementById('gigyaJFMClosingCompletion').removeEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById('gigyaJFMClosingCompletion').addEventListener('click', this.closeWindowPlugIn, false);

        this.objWidgetLogIn.objCtrlFields = {
            objFirstNameText: document.getElementById('gigyaJFMCompletionNombre'),
            objLastNameText: document.getElementById('gigyaJFMCompletionApellidos'),
            objDateOfBirthText: document.getElementById('gigyaJFMCompletionFecha'),
            objGenreSelect: document.getElementById('gigyaJFMCompletionGenero'),
            objCountryText: document.getElementById('gigyaJFMCompletionPais'),
            objStateText: document.getElementById('gigyaJFMCompletionEstado'),
            objCityText: document.getElementById('gigyaJFMCompletionCiudad'),
            objPostalCodeText: document.getElementById('gigyaJFMCompletionPostal'),
            objNewsletterChk: document.getElementById('gigyaJFMCompletionNewsletter'),
            objerrFormSignUpSpan: document.getElementById('gigyaJFMerrFormCompletionRegistration')
        }

        this.objWidgetLogIn.objSendButton = document.getElementById('gigyaJFMCompletionBTN');

        this.objWidgetLogIn.objCtrlFields.objFirstNameText.value = '';
        this.objWidgetLogIn.objCtrlFields.objLastNameText.value = '';
        this.objWidgetLogIn.objCtrlFields.objDateOfBirthText.value = '';
        this.objWidgetLogIn.objCtrlFields.objGenreSelect.selectedIndex = 0;
        this.objWidgetLogIn.objCtrlFields.objCountryText.value = 'México';
        this.objWidgetLogIn.objCtrlFields.objStateText.value = '';
        this.objWidgetLogIn.objCtrlFields.objCityText.value = '';
        this.objWidgetLogIn.objCtrlFields.objPostalCodeText.value = '';
        this.objWidgetLogIn.objCtrlFields.objNewsletterChk.checked = false;
        this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.value = '';

        this.objWidgetLogIn.objSendButton.removeEventListener('click', this.validateCompletionRegistration, false);
        this.objWidgetLogIn.objSendButton.addEventListener('click', this.validateCompletionRegistration, false);

        this.objAJAXService.callService(this, this.objDataTransfer, 'generateRandomKeyForLogIn', this.receiveRandomKey);
    }

    validateCompletionRegistration() {
        objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = '';

        if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objFirstNameText.value == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Falta ingresar su nombre';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objFirstNameText.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objLastNameText.value == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Falta ingresar sus apellidos';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objLastNameText.focus();
        } else if (isNaN(Date.parse(objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objDateOfBirthText.value)) ||
            parseInt(objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objDateOfBirthText.value.split('-')[0]) > (new Date().getFullYear())) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Fecha Inválida';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objDateOfBirthText.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objGenreSelect.selectedIndex == 0) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Seleccione su género';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objGenreSelect.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objCountryText.value == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Ingrese su País';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objCountryText.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objStateText.value == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Ingrese su Estado';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objStateText.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objCityText.value == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Ingrese su Ciudad';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objCityText.focus();
        } else if (objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPostalCodeText.value == '' || isNaN(objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPostalCodeText.value)) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Ingrese su código Postal';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPostalCodeText.focus();
        }
        else {
            objLogicaSessionPlugInLogIn.objDataTransfer.strCaptchaToken = LogicaSessionPlugInLogIn.validateRecaptcha();
            if (!LogicaSessionPlugInLogIn.boolIsValidCatchap) {
                objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Debes verificar reCAPTCHA...';
            }
        }



        if (objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr == '') {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = true;
            objLogicaSessionPlugInLogIn.sendMissingFields();
        }
        else {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = false;
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    sendMissingFields() {

        this.objLocalStorageSession.getLocalStorage();

        // Separar día, mes y año de nacimiento
        let objTodayDt = new Date();
        let strDateParts = this.objWidgetLogIn.objCtrlFields.objDateOfBirthText.value.split('-');
        let intBirthYear = parseInt(strDateParts[0]);
        let intBirthMonth = parseInt(strDateParts[1]);
        let intBirthDay = parseInt(strDateParts[2]);

        let intAge = objTodayDt.getFullYear() - intBirthYear;

        if (objTodayDt.getMonth() < intBirthMonth || (objTodayDt.getMonth() == intBirthMonth && objTodayDt.getDate() < intBirthDay)) {
            intAge--;
        }

        this.objDataTransfer.strEmail = '';
        this.objDataTransfer.strPassword = '';
        this.objDataTransfer.strProfile = JSON.stringify({
            age: intAge,
            birthDay: intBirthDay,
            birthMonth: intBirthMonth,
            birthYear: intBirthYear,
            city: this.objWidgetLogIn.objCtrlFields.objCountryText.value,
            country: this.objWidgetLogIn.objCtrlFields.objCountryText.value,
            firstName: this.objWidgetLogIn.objCtrlFields.objFirstNameText.value,
            gender: this.objWidgetLogIn.objCtrlFields.objGenreSelect.value,
            lastName: this.objWidgetLogIn.objCtrlFields.objLastNameText.value,
            state: this.objWidgetLogIn.objCtrlFields.objStateText.value,
            zip: this.objWidgetLogIn.objCtrlFields.objPostalCodeText.value
        });
        this.objDataTransfer.strData = JSON.stringify({ subscribe: this.objWidgetLogIn.objCtrlFields.objNewsletterChk.checked, terms: true });
        this.objDataTransfer.strExtraProfileFields = '';
        this.objDataTransfer.strExtraProfileFieldsDescriptor = '';
        this.objDataTransfer.boolIsInvalidRequest = false;
        this.objDataTransfer.strSessionCookie = '';
        this.objDataTransfer.boolIsAccountPendingRegistration = false;
        this.objDataTransfer.boolIsSocialLogin = false;
        this.objDataTransfer.strErrormessage = '';
        this.objDataTransfer.strUID = this.objLocalStorageSession.strUID;
        this.objDataTransfer.strRegToken = this.objLocalStorageSession.strRegToken;
        this.objDataTransfer.boolIsCompletedOperation = false;

        this.objAJAXService.callService(this, this.objDataTransfer, 'sendMissingFields', this.processCompleteRegistrarionAceptance, true);
    }

    processCompleteRegistrarionAceptance(strResponseText) {
        this.objDataTransfer.setDataFromJSON(strResponseText);

        if (this.objDataTransfer.strErrormessage != 'OK') {
            if (this.objDataTransfer.boolIsInvalidRequest) {
                alert('Se presento un error que impide continuar - MSG: ' + this.objDataTransfer.strErrormessage);
                // Cierre de la ventana sin continuar el flujo
                this.closeWindowPlugIn();
                return;
            }
            else {
                this.objWidgetLogIn.strMsgErr = this.objDataTransfer.strErrormessage;
                this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = this.objWidgetLogIn.strMsgErr;
                grecaptcha.reset(this.objWidgetLogIn.strHTMLCOMPLETION.recaptchaIds.recaptcha1.widgetId);
                this.getcomputedKey(this.objDataTransfer.strRandomKey).then(x => this.getcomputedKey(x).then(x => this.objDataTransfer.strComputedKey = x));
            }
        }
        else {
            alert('¡Registro completado!');
            this.objLocalStorageSession.isPendingRegistration = false;
            this.objLocalStorageSession.setLocalStorage();
            this.closeWindowPlugIn();
        }

    }
    // #endregion

    // #region métodos para el estado signUp

    showSignUpControl() {
        this.funcInitializer = this.initSignUpLogInControlEvents;
        this.loadControl([StatesGigya.LOAD], StatesGigya.LOGIN, true);
    }

    initSignUpLogInControlEvents() {
        let objHTMLLinks = document.getElementsByClassName('gigyaJFMLink');
        if (objHTMLLinks) {
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.removeEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLLinks).forEach(item => item.addEventListener('click', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }
        let objHTMLSubmit = document.getElementsByClassName('gigyaJFMform-detail');
        if (objHTMLSubmit) {
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.removeEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
            Array.prototype.slice.call(objHTMLSubmit).forEach(item => item.addEventListener('submit', LogicaSessionPlugInLogIn.preventDefaultCtrl, false));
        }

        document.getElementById('gigyaJFMsignUpOpen').removeEventListener('click', LogicaSessionPlugInLogIn.showSignUpLogInDialog, false);
        document.getElementById('gigyaJFMLogInOpen').removeEventListener('click', LogicaSessionPlugInLogIn.showSignUpLogInDialog, false);
        document.getElementById('gigyaJFMClosing').removeEventListener('click', this.closeWindowPlugIn, false);

        document.getElementById('gigyaJFMsignUpOpen').addEventListener('click', LogicaSessionPlugInLogIn.showSignUpLogInDialog, false);
        document.getElementById('gigyaJFMLogInOpen').addEventListener('click', LogicaSessionPlugInLogIn.showSignUpLogInDialog, false);
        document.getElementById('gigyaJFMClosing').addEventListener('click', this.closeWindowPlugIn, false);
        document.getElementById("gigyaJFMsignUpOpen").click();
    }


    initSignUpEvents() {
        this.objWidgetLogIn.objCtrlFields = {
            objemailText: document.getElementById('gigyaJFMemail'),
            objPasswordText: document.getElementById('gigyaJFMpassword'),
            objConfirmPsText: document.getElementById('gigyaJFMpasswordrepeat'),
            objNewsletterChk: document.getElementById('gigyaJFMCheck2'),
            objPrivacyNoticeChk: document.getElementById('gigyaJFMCheck1'),
            objerrFormSignUpSpan: document.getElementById('gigyaJFMerrFormSignUp')
        }

        this.objWidgetLogIn.objSendButton = document.getElementById('gigyaJFMBTNSignUp');

        this.objWidgetLogIn.objSocialGmailLink.objHTML = document.getElementById('gigyaJFMSocialGmail');
        this.objWidgetLogIn.objSocialFacebookLink.objHTML = document.getElementById('gigyaJFMSocialFacebook');
        this.objWidgetLogIn.objSocialTwitterLink.objHTML = document.getElementById('gigyaJFMSocialTwitter');

        this.objWidgetLogIn.objCtrlFields.objemailText.value = '';
        this.objWidgetLogIn.objCtrlFields.objPasswordText.value = '';
        this.objWidgetLogIn.objCtrlFields.objConfirmPsText.value = '';
        this.objWidgetLogIn.objCtrlFields.objNewsletterChk.checked = false;
        this.objWidgetLogIn.objCtrlFields.objPrivacyNoticeChk.checked = false;
        this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.value = '';

        this.objWidgetLogIn.objCtrlFields.objemailText.removeEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objCtrlFields.objConfirmPsText.removeEventListener('keypress', this.validateConfirmPasswordSignUp, false);
        this.objWidgetLogIn.objSendButton.removeEventListener('click', this.validateSignUpForm, false);
        this.objWidgetLogIn.objSocialGmailLink.objHTML.removeEventListener('click', this.goToSocialLink, false);
        this.objWidgetLogIn.objSocialFacebookLink.objHTML.removeEventListener('click', this.goToSocialLink, false);
        this.objWidgetLogIn.objSocialTwitterLink.objHTML.removeEventListener('click', this.goToSocialLink, false);

        this.objWidgetLogIn.objCtrlFields.objemailText.addEventListener('blur', this.lostFocusEmail, false);
        this.objWidgetLogIn.objCtrlFields.objConfirmPsText.addEventListener('keyup', this.validateConfirmPasswordSignUp, false);
        this.objWidgetLogIn.objSendButton.addEventListener('click', this.validateSignUpForm, false);
        this.objWidgetLogIn.objSocialGmailLink.objHTML.addEventListener('click', this.goToSocialLink, false);
        this.objWidgetLogIn.objSocialFacebookLink.objHTML.addEventListener('click', this.goToSocialLink, false);
        this.objWidgetLogIn.objSocialTwitterLink.objHTML.addEventListener('click', this.goToSocialLink, false);

        this.objAJAXService.callService(this, this.objDataTransfer, 'generateRandomKeyForLogIn', this.receiveRandomKey);
    }

    validateRequieredPasswordField(evt) {
        let strPassword = objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPasswordText.value;

        if (strPassword) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = true;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = '';
        }
        else {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = false;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Debe digitar el password';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPasswordText.focus();
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    validateConfirmPasswordSignUp(evt) {
        let strPassword = objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objPasswordText.value,
            strConfirmPS = objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objConfirmPsText.value;

        if (strPassword === strConfirmPS) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = true;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = '';
        }
        else {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = false;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'El password digitado no corresponde con el primero ya ingresado';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objConfirmPsText.focus();
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    validateSignUpForm(evt) {
        objLogicaSessionPlugInLogIn.lostFocusEmail();
        if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
            objLogicaSessionPlugInLogIn.validateRequieredPasswordField()
            if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
                objLogicaSessionPlugInLogIn.validateConfirmPasswordSignUp();
                if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
                    objLogicaSessionPlugInLogIn.validatePrivacyNoticeSignUp()
                    if (objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid) {
                        objLogicaSessionPlugInLogIn.objDataTransfer.strCaptchaToken = LogicaSessionPlugInLogIn.validateRecaptcha();
                        if (LogicaSessionPlugInLogIn.boolIsValidCatchap) {
                            objLogicaSessionPlugInLogIn.sendCredentialesToSignUp();
                        }
                        else {
                            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Debes verificar reCAPTCHA...';
                        }
                    }
                }
            }
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    validatePrivacyNoticeSignUp() {
        if (this.objWidgetLogIn.objCtrlFields.objPrivacyNoticeChk.checked) {
            this.objWidgetLogIn.boolValid = true;
            this.objWidgetLogIn.strMsgErr = '';
        }
        else {
            this.objWidgetLogIn.boolValid = false;
            this.objWidgetLogIn.strMsgErr = 'Debe de aceptar el aviso de privacidad';
            this.objWidgetLogIn.objCtrlFields.objPrivacyNoticeChk.focus();
        }
        this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = this.objWidgetLogIn.strMsgErr;
    }

    receiveRandomKey(strResponseText) {
        this.objDataTransfer.setDataFromJSON(strResponseText);

        if (this.objDataTransfer.strErrormessage != 'OK') {
            alert('Se presento un error que impide continuar - MSG: ' + this.objDataTransfer.strErrormessage);

            this.closeWindowPlugIn();
            // Cierre de la ventana sin continuar el flujo
            return;
        }
        this.getcomputedKey(this.objDataTransfer.strRandomKey).then(x => this.getcomputedKey(x).then(x => this.objDataTransfer.strComputedKey = x));

    }

    sendCredentialesToSignUp() {
        this.objDataTransfer.strEmail = this.objWidgetLogIn.objCtrlFields.objemailText.value;
        this.objDataTransfer.strPassword = this.objWidgetLogIn.objCtrlFields.objPasswordText.value;
        this.objDataTransfer.strProfile = JSON.stringify({ email: this.objDataTransfer.strEmail });
        this.objDataTransfer.strData = JSON.stringify({ subscribe: this.objWidgetLogIn.objCtrlFields.objNewsletterChk.checked, terms: true });
        this.objDataTransfer.strExtraProfileFields = '';
        this.objDataTransfer.strExtraProfileFieldsDescriptor = '';
        this.objDataTransfer.boolIsInvalidRequest = false;
        this.objDataTransfer.strSessionCookie = '';
        this.objDataTransfer.boolIsAccountPendingRegistration = false;
        this.objDataTransfer.boolIsSocialLogin = false;
        this.objDataTransfer.strErrormessage = '';
        this.objDataTransfer.strUID = '';
        this.objDataTransfer.strRegToken = '';
        this.objDataTransfer.boolIsCompletedOperation = false;

        this.objAJAXService.callService(this, this.objDataTransfer, 'sendCredentialesToSignUp', this.processSignUpLogInAceptance, true);
    }

    processSignUpLogInAceptance(strResponseText) {
        this.objDataTransfer.setDataFromJSON(strResponseText);

        if (this.objDataTransfer.strErrormessage != 'OK') {
            if (this.objDataTransfer.boolIsInvalidRequest) {
                alert('Se presento un error que impide continuar - MSG: ' + this.objDataTransfer.strErrormessage);
                // Cierre de la ventana sin continuar el flujo
                this.closeWindowPlugIn();
                return;
            }
            else {
                this.objWidgetLogIn.strMsgErr = this.objDataTransfer.strErrormessage;
                this.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = this.objWidgetLogIn.strMsgErr;
                grecaptcha.reset(this.objWidgetLogIn.strHTMLLOGIN.recaptchaIds.recaptcha1.widgetId);
                grecaptcha.reset(this.objWidgetLogIn.strHTMLLOGIN.recaptchaIds.recaptcha2.widgetId);
                this.getcomputedKey(this.objDataTransfer.strRandomKey).then(x => this.getcomputedKey(x).then(x => this.objDataTransfer.strComputedKey = x));
            }
        }
        else {
            this.objLocalStorageSession.strUID = this.objDataTransfer.strUID;
            this.objLocalStorageSession.dtLastAccess = new Date();
            this.objLocalStorageSession.strRegToken = this.objDataTransfer.strRegToken;
            this.objLocalStorageSession.intExpirationSessionDays = this.objDataTransfer.intExpirationSessionDays;
            this.objLocalStorageSession.isPendingRegistration = this.objDataTransfer.boolIsAccountPendingRegistration;
            this.objLocalStorageSession.setLocalStorage();

            alert('Solicitud procesada con exito');

            if (this.objLocalStorageSession.isPendingRegistration) {
                this.goToThankyouPage();
            }
            else {
                this.closeWindowPlugIn();
            }
        }
    }
    // #endregion

    // #region métodos para Social Login

    goToSocialLink(evt) {
        let strUrl = '';

        switch (evt.currentTarget.id) {
            case 'gigyaJFMSocialGmail':
                strUrl = objLogicaSessionPlugInLogIn.objWidgetLogIn.objSocialGmailLink.strUrl;
                break;
            case 'gigyaJFMSocialFacebook':
                strUrl = objLogicaSessionPlugInLogIn.objWidgetLogIn.objSocialFacebookLink.strUrl;
                break;
            case 'gigyaJFMSocialTwitter':
                strUrl = objLogicaSessionPlugInLogIn.objWidgetLogIn.objSocialTwitterLink.strUrl;
                break;
        }

        objLogicaSessionPlugInLogIn.objWindow = window.open(strUrl, "_blank", "location=no,height=600,width=750");

        if (objLogicaSessionPlugInLogIn.objWindow == null) {
            alert('Ventana Emergente Bloqueda')
        }
    }


    receivePostMessageFromWindow(evt) {
        let srtCommand = JSON.parse(evt.data);

        this.objWindow.close();

        if (srtCommand.MSG == "SocialLogin:completed") {
            if (srtCommand.ERROR == "" || srtCommand.ERROR == "pending_registration") {
                this.objLocalStorageSession.dtLastAccess = new Date();
                this.objLocalStorageSession.intExpirationSessionDays = 7;
                this.objLocalStorageSession.isPendingRegistration = false;
                this.objLocalStorageSession.strUID = '';
                this.objLocalStorageSession.strRegToken = srtCommand.ACCESSTOKEN;

                this.objLocalStorageSession.setLocalStorage();
                this.boolIsSocialLogin = true;
                this.closeWindowPlugIn();
            }
            else {
                alert('Hubo un error al tratar de ingresar por Red Social :' + srtCommand.ERRORDESCRIPTION);
            }
        }
        else {
            alert('Hubo un error al tratar de ingresar por Red Social : petición fuera de origen');
        }
    }

    // #endregion

    // #region metodos para el logout
    closeSession() {
        if (this.objDataTransfer.isPreviousLogIn(this.objLocalStorageSession)) {
            this.objAJAXService.callService(this, this.objDataTransfer, 'LogoutSession', null);
        }
        this.objLocalStorageSession.removeLocalStorage();
        this.closeWindowPlugIn();
    }

    // #endregion

    // #region metodos en comun

    lostFocusEmail(evt) {
        if (!/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objemailText.value)) {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = false;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = 'Email no tiene el formato correto';
            objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objemailText.focus();

        }
        else {
            objLogicaSessionPlugInLogIn.objWidgetLogIn.boolValid = true;
            objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr = '';
        }
        objLogicaSessionPlugInLogIn.objWidgetLogIn.objCtrlFields.objerrFormSignUpSpan.innerHTML = objLogicaSessionPlugInLogIn.objWidgetLogIn.strMsgErr;
    }

    closeWindowPlugIn(evt) {
        if (objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey) {
            objLogicaSessionPlugInLogIn.objAJAXService.callService(objLogicaSessionPlugInLogIn, objLogicaSessionPlugInLogIn.objDataTransfer, 'deleteRandomHex', null);
            objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey = '';
            objLogicaSessionPlugInLogIn.objDataTransfer.strComputedKey = '';
        }
        objLogicaSessionPlugInLogIn.showLoadControl(true);
    }


    async getcomputedKey(strMessage) {
        let msgUint8 = new TextEncoder().encode(strMessage);                           // encode as (utf-8) Uint8Array
        let hashBuffer = await crypto.subtle.digest('SHA-256', msgUint8);           // hash the message
        let hashArray = Array.from(new Uint8Array(hashBuffer));                     // convert buffer to byte array
        return hashArray.map(b => b.toString(16).padStart(2, '0')).join(''); // convert bytes to hex string
    }


    //#endregion


    // #region Carga del componente en el DOM
    loadControl(enumPreviousState, enumNextState, boolAppendToBody) {

        //if (!enumPreviousState.some(item => this.enumState == item)) {
        //    alert('operación invalida con el control')
        //    return;
        //}

        // Creamos objeto de comunicación para el evento en partitular
        this.objDataTransfer = new DataTransferCom();

        this.enumState = enumNextState;  // actualizamos el estado actual del sitio

        // ocultamos los demás controles
        let objCls = document.getElementsByClassName(this.strControlClasses);
        let objCtrl = null;

        if (objCls) {
            objCtrl = Array.prototype.slice.apply(objCls);
            objCtrl.forEach(item => {
                if (!item.classList.contains(this.strClassForHiding)) {
                    item.classList.add(this.strClassForHiding);
                }
                if (item.classList.contains(this.strClassForShowing)) {
                    item.classList.remove(this.strClassForShowing);
                }
            });
        }

        switch (enumNextState) {
            case 'LOAD':
                this.strControlLoading = this.objWidgetLogIn.strHTMLLOAD.strHTML;
                break;
            case 'LOGIN':
                this.strControlLoading = this.objWidgetLogIn.strHTMLLOGIN.strHTML;
                break;
            case 'CONTINUE':
                this.strControlLoading = this.objWidgetLogIn.strHTMLCONTINUE.strHTML;
                break;
            case 'COMPLETION':
                this.strControlLoading = this.objWidgetLogIn.strHTMLCOMPLETION.strHTML;
                break;
            case 'SIGNIN':
                this.strControlLoading = this.objWidgetLogIn.strHTMLSIGNIN.strHTML;
                break;
            case 'RESETPS':
                this.strControlLoading = this.objWidgetLogIn.strHTMLRESETPS.strHTML;
                break;
        }

        this.strClassAnchor = (boolAppendToBody) ? null : 'gigyaJFMButtonLogIn';

        if (this.strControlLoading) {
            this.loadingControl(null);
        }
        else {
            this.objAJAXService.callServiceToGetHTML(this, enumNextState, 'getHTMLLogin', this.loadingControl, (enumNextState != 'LOAD'));
        }
    }

    loadingControl(strResponseText) {

        let strControl = '';
        let objRecaptchaIds = null;

        if (strResponseText) {
            let objStatePlugIn = JSON.parse(strResponseText);
            strControl = objStatePlugIn.strHTML;
        }



        if (strControl) {
            switch (this.enumState) {
                case 'LOAD':
                    this.objWidgetLogIn.strHTMLLOAD.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLLOAD.recaptchaIds;
                    break;
                case 'LOGIN':
                    this.objWidgetLogIn.strHTMLLOGIN.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLLOGIN.recaptchaIds;
                    break;
                case 'CONTINUE':
                    this.objWidgetLogIn.strHTMLCONTINUE.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLCONTINUE.recaptchaIds;
                    break;
                case 'COMPLETION':
                    this.objWidgetLogIn.strHTMLCOMPLETION.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLCOMPLETION.recaptchaIds;
                    break;
                case 'SIGNIN':
                    this.objWidgetLogIn.strHTMLSIGNIN.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLSIGNIN.recaptchaIds;
                    break;
                case 'RESETPS':
                    this.objWidgetLogIn.strHTMLRESETPS.strHTML = strControl;
                    objRecaptchaIds = this.objWidgetLogIn.strHTMLRESETPS.recaptchaIds;
                    break;
            }

            let objHTML = (this.strClassAnchor === null) ? document.body : document.getElementsByClassName(this.strClassAnchor)[0];
            if (objHTML) {
                if (objHTML.classList.contains(this.strClassAnchor)) {
                    objHTML.innerHTML = '';
                }

                objHTML.insertAdjacentHTML('beforeend', strControl);
                for (let objRecaptchaItem in objRecaptchaIds) {
                    objRecaptchaIds[objRecaptchaItem].widgetId = grecaptcha.render(objRecaptchaIds[objRecaptchaItem].id, objRecaptchaIds[objRecaptchaItem].parameters);
                }
            }
        }

        for (let strProp in this.objWidgetLogIn) {
            if (typeof this.objWidgetLogIn[strProp] == "object" && typeof this.objWidgetLogIn[strProp].boolOpenCtrl !== 'undefined') {
                this.objWidgetLogIn[strProp].boolOpenCtrl = false;
            }
        }

        switch (this.enumState) {
            case 'LOAD':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLLOAD.id;
                this.objWidgetLogIn.strHTMLLOAD.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLLOAD.recaptchaIds;
                break;
            case 'LOGIN':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLLOGIN.id;
                this.objWidgetLogIn.strHTMLLOGIN.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLLOGIN.recaptchaIds;
                break;
            case 'CONTINUE':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLCONTINUE.id;
                this.objWidgetLogIn.strHTMLCONTINUE.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLCONTINUE.recaptchaIds;
                break;
            case 'COMPLETION':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLCOMPLETION.id;
                this.objWidgetLogIn.strHTMLCOMPLETION.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLCOMPLETION.recaptchaIds;
                break;
            case 'SIGNIN':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLSIGNIN.id;
                this.objWidgetLogIn.strHTMLSIGNIN.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLSIGNIN.recaptchaIds;
                break;
            case 'RESETPS':
                this.strIDCtrl = this.objWidgetLogIn.strHTMLRESETPS.id;
                this.objWidgetLogIn.strHTMLRESETPS.boolOpenCtrl = true;
                objRecaptchaIds = this.objWidgetLogIn.strHTMLRESETPS.recaptchaIds;
                break;
        }

        for (let objRecaptchaItem in objRecaptchaIds) {
            grecaptcha.reset(objRecaptchaIds[objRecaptchaItem].widgetId);
        }

        let objCtrl = document.getElementById(this.strIDCtrl);

        if (!objCtrl.classList.contains(this.strClassForShowing)) {
            objCtrl.classList.add(this.strClassForShowing);
        }

        this.funcInitializer();
    }

    // #endregion

    // #region Métodos staticos
    static goToSignUpControl(evt) {
        if (evt) {
            evt.preventDefault();
        }
        objLogicaSessionPlugInLogIn.showSignUpControl();
    }

    static goTocloseSession(evt) {
        evt.preventDefault();
        objLogicaSessionPlugInLogIn.closeSession();
    }

    static validateRecaptcha(token) {
        let objRecaptchaIds = null;
        let objResponse = null;
        LogicaSessionPlugInLogIn.boolIsValidCatchap = false;

        for (let strProp in objLogicaSessionPlugInLogIn.objWidgetLogIn) {
            if (typeof objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp] == "object" && typeof objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].boolOpenCtrl !== 'undefined') {
                if (objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].boolOpenCtrl) {
                    objRecaptchaIds = objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].recaptchaIds;
                }
            }
        }

        for (let objRecaptchaItem in objRecaptchaIds) {
            objResponse = grecaptcha.getResponse(objRecaptchaIds[objRecaptchaItem].widgetId);
            if (objResponse.length != 0) {
                LogicaSessionPlugInLogIn.boolIsValidCatchap = true;
                return objResponse;
            }
        }

        return '';
    }

    static preventDefaultCtrl(evt) {
        evt.preventDefault();
    }


    static showSignUpLogInDialog(evt) {
        let i,
            strEvento,
            objTabcontent,
            objTablinks,
            objRecaptchaIds = null;

        if (objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey) {
            objLogicaSessionPlugInLogIn.objAJAXService.callService(objLogicaSessionPlugInLogIn, objLogicaSessionPlugInLogIn.objDataTransfer, 'deleteRandomHex', null);
            objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey = '';
            objLogicaSessionPlugInLogIn.objDataTransfer.strComputedKey = '';
        }

        for (let strProp in objLogicaSessionPlugInLogIn.objWidgetLogIn) {
            if (typeof objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp] == "object" && typeof objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].boolOpenCtrl !== 'undefined') {
                if (objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].boolOpenCtrl) {
                    objRecaptchaIds = objLogicaSessionPlugInLogIn.objWidgetLogIn[strProp].recaptchaIds;
                }
            }
        }

        for (let objRecaptchaItem in objRecaptchaIds) {
            grecaptcha.reset(objRecaptchaIds[objRecaptchaItem].widgetId);
        }



        objTabcontent = document.querySelectorAll("#gigyaJFMLOGINCtrl .gigyaJFMtabcontent");
        for (i = 0; i < objTabcontent.length; i++) {
            objTabcontent[i].style.display = "none";
        }
        objTablinks = document.getElementsByClassName("gigyaJFMtablinks");
        for (i = 0; i < objTablinks.length; i++) {
            objTablinks[i].className = objTablinks[i].className.replace(" gigyaJFMactive", "");
        }
        evt.currentTarget.className += " gigyaJFMactive";

        if (evt.currentTarget.id === 'gigyaJFMsignUpOpen') {
            objLogicaSessionPlugInLogIn.enumState = StatesGigya.SIGNIN;
            objLogicaSessionPlugInLogIn.initSignUpEvents();
            strEvento = 'gigyaJFMsign-up';
        }
        else if (evt.currentTarget.id === 'gigyaJFMLogInOpen') {
            objLogicaSessionPlugInLogIn.enumState = StatesGigya.LOGINWITHEMAIL;
            objLogicaSessionPlugInLogIn.initLogInEvents();
            strEvento = 'gigyaJFMsign-in';
        }
        document.getElementById(strEvento).style.display = "block";

    }

    static goFromThankyouPageToCompletionPage() {
        objLogicaSessionPlugInLogIn.showCompletionPage();
    }

    static goFromLogInToForgotPassword() {
        if (objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey) {
            objLogicaSessionPlugInLogIn.objAJAXService.callService(objLogicaSessionPlugInLogIn, objLogicaSessionPlugInLogIn.objDataTransfer, 'deleteRandomHex', null);
            objLogicaSessionPlugInLogIn.objDataTransfer.strRandomKey = '';
            objLogicaSessionPlugInLogIn.objDataTransfer.strComputedKey = '';
        }
        objLogicaSessionPlugInLogIn.showForgotPaswordCtrl();
    }
    // #endregion

}