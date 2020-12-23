using KCM.ServiciosInternet.Plugins.Business.BD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entity = KCM.ServiciosInternet.Plugins.Entities;
using System.Web.Script.Serialization;

namespace KCM.ServiciosInternet.Plugins.Business.Logica
{
    static class clsPlugInLogInServices
    {
        private static void updateIPlugInEventsLogIn(ref Entity.clsSessionState clsSessionState, Entity.clsDataTransfer objData, Entity.clsSessionState.EventType eventType)
        {

            if (clsSessionState == null)
            {
                clsSessionState = new Entity.clsSessionState(objData, eventType, Entity.clsConfigPlugIn.intExpirationSessionDays);
            }
            else
            {
                if (clsSessionState.enumEvent == eventType)
                {
                    clsSessionState.objLogInObjects.receiveData(objData);
                }
                else
                {
                    clsSessionState.Dispose();
                    clsSessionState = null;
                    clsSessionState = new Entity.clsSessionState(objData, eventType, Entity.clsConfigPlugIn.intExpirationSessionDays);
                }
            }
        }


        private static void receiveDataFromIPlugInEventsLogIn(Entity.clsSessionState objSessionState, ref Entity.clsDataTransfer objData)
        {
            objData = objSessionState.objLogInObjects.sendData();
        }

        internal static bool deleteRandomKey(ref Entity.clsDataTransfer objData)
        {
            bool boolIsDeleted = false;
            Entity.clsSessionState objSessionState = null;
            Entities.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.LogIn;
            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);
            objSessionState.objLogInObjects.strComputedKey = objData.strComputedKey;
            //registramos las llave en la BD
            using (SessionBD objBD = new SessionBD())
            {
                if (objBD.isExistselectKey(objSessionState))
                {
                    boolIsDeleted = objBD.deleteKey(objSessionState);
                }
            }

            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);
            return boolIsDeleted;
        }

        private static bool AreEqualComputedKeys(Entity.clsSessionState objSessionState)
        {
            using (SessionBD objBD = new SessionBD())
            {
                if (objBD.isExistselectKey(objSessionState))
                {
                    return true;
                }
                else
                {

                    objSessionState.objLogInObjects.boolIsCompletedOperation = false;
                    objSessionState.objLogInObjects.boolIsInvalidRequest = true;
                    objSessionState.objLogInObjects.strErrormessage = "Computed keys are not equal";
                    return false;
                }
            }
        }
        private static bool deleteKey(Entity.clsSessionState objSessionState)
        {
            using (SessionBD objBD = new SessionBD())
            {
                if (objBD.isExistselectKey(objSessionState))
                {
                    return objBD.deleteKey(objSessionState);
                }
            }
            return false;
        }


        private static bool isExpiredKey(Entity.clsSessionState objSessionState)
        {
            if (objSessionState.isExpiredKey(Entity.clsConfigPlugIn.intExpirationKeyMins))
            {
                objSessionState.objLogInObjects.boolIsCompletedOperation = false;
                objSessionState.objLogInObjects.boolIsInvalidRequest = true;
                objSessionState.objLogInObjects.strErrormessage = "Computed key is expired";
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isExpiredReCaptcha(Entity.clsSessionState objSessionState, bool boolIsLogIn)
        {
            string strCatchapSecret = string.Empty;  // almacena la llave secreta de recaptcha

            if (boolIsLogIn)
            {
                strCatchapSecret = Entity.clsConfigPlugIn.strReCAPTCHALogInFlowSecret;
            }
            else
            {
                strCatchapSecret = Entity.clsConfigPlugIn.strReCAPTCHARegisterFlowSecret;
            }
            string strWebAddr = "https://www.google.com/recaptcha/api/siteverify?secret=" + strCatchapSecret + "&response=" + objSessionState.objLogInObjects.strCaptchaToken;

            WebRequest objHttpWebRequest = WebRequest.Create(strWebAddr);
            HttpWebResponse objHttpResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();
            using (StreamReader objSdr = new StreamReader(objHttpResponse.GetResponseStream()))
            {
                JavaScriptSerializer objJSONSerializer = new JavaScriptSerializer();
                Dictionary<string, object> objResp = (Dictionary<string, object>)objJSONSerializer.DeserializeObject(objSdr.ReadToEnd());
                // todo: debes de poner registro de errores log
                if (!objResp["success"].ToString().Equals("True"))
                {
                    objSessionState.objLogInObjects.boolIsCompletedOperation = false;
                    objSessionState.objLogInObjects.boolIsInvalidRequest = true;
                    objSessionState.objLogInObjects.strErrormessage = "Debes verificar reCAPTCHA...";
                    return true;
                }
            }

            return false;
        }

        internal static Entity.clsSessionState generateRandomKey(ref Entity.clsDataTransfer objData, string strCookieSessionId, Entity.clsSessionState.EventType eventType)
        {
            Entity.clsSessionState objSessionState = null;
            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);

            //registramos las llave en la BD
            using (SessionBD objBD = new SessionBD())
            {
                objBD.deleteExpiredKeys(Entity.clsConfigPlugIn.intExpirationKeyMins * 3);
                if (objBD.isReachMaxRequest(strCookieSessionId))
                {
                    objSessionState.objLogInObjects.boolIsCompletedOperation = false;
                    objSessionState.objLogInObjects.boolIsInvalidRequest = true;
                    objSessionState.objLogInObjects.strErrormessage = "it has reached max request: wait for " + (Entity.clsConfigPlugIn.intExpirationKeyMins * 3) + " minutes";
                    objSessionState.objLogInObjects.strRandomKey = "";
                    objSessionState.objLogInObjects.strComputedKey = "";
                }
                else
                {
                    if (objBD.isExistselectKey(objSessionState))
                    {
                        objBD.deleteKey(objSessionState);
                    }
                    objBD.insertKey(objSessionState, strCookieSessionId);
                    objSessionState.objLogInObjects.strErrormessage = "OK";
                }
            }

            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);
            return objSessionState;
        }

        internal static bool sendCredentialesToLogIn(ref Entity.clsDataTransfer objData)
        {
            Entity.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.LogIn;
            Entity.clsSessionState objSessionState = null;
            bool boolIsLogIn = false;

            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);

            objSessionState.objLogInObjects.strComputedKey = objData.strComputedKey;

            if (!isExpiredReCaptcha(objSessionState,true))  
            {
                if (AreEqualComputedKeys(objSessionState))
                {
                    if (!isExpiredKey(objSessionState))
                    {
                        using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
                        {
                            boolIsLogIn = objGigyaAccounts.logIn(objSessionState.objLogInObjects as Entity.clsLogin);
                            if (objSessionState.objLogInObjects.strErrormessage == "OK" || objSessionState.objLogInObjects.boolIsInvalidRequest == true)
                            {
                                deleteKey(objSessionState);
                            }
                        }
                    }
                } 
            }
            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);

            return boolIsLogIn;
        }

        internal static Entity.clsSessionState getAccountInfo(ref Entity.clsDataTransfer objData, Entity.clsSessionState.EventType eventType)
        {
            Entity.clsSessionState objSessionState = null;
            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);

            using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
            {
                objGigyaAccounts.getAccountInfo(objSessionState.objLogInObjects as Entity.clsCompleteRegistration, !string.IsNullOrEmpty((objSessionState.objLogInObjects as Entity.clsCompleteRegistration).strUID));
            }

            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);
            return objSessionState;
        }

        internal static bool sendMissingFields(ref Entity.clsDataTransfer objData)
        {
            Entity.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.CompleteRegistration;
            Entity.clsSessionState objSessionState = null;

            bool boolIsUpdated = false;

            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);
            objSessionState.objLogInObjects.strComputedKey = objData.strComputedKey;

            if (!isExpiredReCaptcha(objSessionState,true))
            {
                if (AreEqualComputedKeys(objSessionState))
                {
                    if (!isExpiredKey(objSessionState))
                    {
                        using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
                        {
                            if (objGigyaAccounts.setAccountInfo(objSessionState.objLogInObjects as Entity.clsCompleteRegistration, string.IsNullOrEmpty((objSessionState.objLogInObjects as Entity.clsCompleteRegistration).strRegToken)))
                            {
                                boolIsUpdated = objGigyaAccounts.finalizeRegistration(objSessionState.objLogInObjects as Entity.clsCompleteRegistration);
                                if (objSessionState.objLogInObjects.strErrormessage == "OK" || objSessionState.objLogInObjects.boolIsInvalidRequest == true)
                                {
                                    deleteKey(objSessionState);
                                }
                            }
                        }
                    }
                } 
            }
            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);

            return boolIsUpdated;
        }

        internal static bool LogoutSession(ref Entity.clsDataTransfer objData)
        {
            Entity.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.LogOut;
            Entity.clsSessionState objSessionState = null;

            bool boolIsLogout = false;

            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);

            using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
            {
                boolIsLogout = objGigyaAccounts.logOut(objSessionState.objLogInObjects as Entity.clsLogOut);
            }
            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);

            return boolIsLogout;
        }


        internal static bool requestResetPassword(ref Entity.clsDataTransfer objData)
        {
            Entity.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.ResetPassword;
            Entity.clsSessionState objSessionState = null;

            bool boolIsEmailSended = false;

            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);
            objSessionState.objLogInObjects.strComputedKey = objData.strComputedKey;

            if (!isExpiredReCaptcha(objSessionState,true))
            {
                if (AreEqualComputedKeys(objSessionState))
                {
                    if (!isExpiredKey(objSessionState))
                    {
                        using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
                        {
                            boolIsEmailSended = objGigyaAccounts.resetPassword(objSessionState.objLogInObjects as Entity.clsResetPassword);
                            //if (objSessionState.objLogInObjects.strErrormessage == "OK" || objSessionState.objLogInObjects.boolIsInvalidRequest == true)
                            //{
                                deleteKey(objSessionState);
                            //}
                        }
                    }
                } 
            }
            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);

            return boolIsEmailSended;
        }

        internal static bool sendCredentialesToSignIn(ref Entity.clsDataTransfer objData)
        {

            Entity.clsSessionState.EventType eventType = Entity.clsSessionState.EventType.SignIn;
            Entity.clsSessionState objSessionState = null;
            bool boolIsLogIn = false;

            updateIPlugInEventsLogIn(ref objSessionState, objData, eventType);

            objSessionState.objLogInObjects.strComputedKey = objData.strComputedKey;

            if (!isExpiredReCaptcha(objSessionState,false))
            {
                if (AreEqualComputedKeys(objSessionState))
                {
                    if (!isExpiredKey(objSessionState))
                    {
                        using (clsGigyaAccounts objGigyaAccounts = new clsGigyaAccounts())
                        {
                            boolIsLogIn = objGigyaAccounts.register(objSessionState.objLogInObjects as Entity.clsSignIn);
                            if (objSessionState.objLogInObjects.strErrormessage == "OK" || objSessionState.objLogInObjects.boolIsInvalidRequest == true)
                            {
                                deleteKey(objSessionState);
                            }
                        }
                    }
                } 
            }
            receiveDataFromIPlugInEventsLogIn(objSessionState, ref objData);

            return boolIsLogIn;
        }
    }
}
