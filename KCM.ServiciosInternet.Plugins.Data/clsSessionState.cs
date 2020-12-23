using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace KCM.ServiciosInternet.Plugins.Entities
{
    public class clsSessionState : IDisposable
    {
        #region propiedades

        public enum EventType
        {
            LogIn,
            CompleteRegistration,
            LogOut,
            ResetPassword,
            SignIn
        }


        public IPlugInLogInObjects objLogInObjects
        {
            get
            {
                if (_objLogInObjects == null)
                {
                    switch (enumEvent)
                    {
                        case EventType.LogIn:
                            _objLogInObjects = new clsLogin();
                            break;
                        case EventType.CompleteRegistration:
                            _objLogInObjects = new clsCompleteRegistration();
                            break;
                        case EventType.LogOut:
                            _objLogInObjects = new clsLogOut();
                            break;
                        case EventType.ResetPassword:
                            _objLogInObjects = new clsResetPassword();
                            break;
                        case EventType.SignIn:
                            _objLogInObjects = new clsSignIn();
                            break;
                        default:
                            throw new NullReferenceException();
                    }
                }
                return _objLogInObjects;
            }
        }

        public EventType enumEvent { get; set; }

        #endregion

        private IPlugInLogInObjects _objLogInObjects;

        private bool disposedValue = false; // Para detectar llamadas redundantes


        public clsSessionState(clsDataTransfer objData, EventType enumEvent, int intExpirationSessionDays = 7)
        {
            this.enumEvent = enumEvent;
            objData.intExpirationSessionDays = intExpirationSessionDays;

            objLogInObjects.receiveData(objData);

            objLogInObjects.strRandomKey = Guid.NewGuid().ToString();
            objLogInObjects.strComputedKey = getcomputedKey(getcomputedKey(objLogInObjects.strRandomKey));
            objLogInObjects.dtRandomKeyDate = DateTime.Now;
        }

        public clsSessionState(EventType enumEvento) : this(new clsDataTransfer(), enumEvento)
        {
        }

        public clsSessionState() : this(EventType.LogIn)
        {
        }

        public bool AreEqualComputedKeys(clsDataTransfer objData)
        {
            return this.objLogInObjects.strComputedKey.Equals(objData.strComputedKey);
        }

        public bool isExpiredKey(int intMinutes)
        {
            this.objLogInObjects.dtRandomKeyDate = this.objLogInObjects.dtRandomKeyDate.AddMinutes(intMinutes);
            return DateTime.Compare(DateTime.Now, this.objLogInObjects.dtRandomKeyDate) > 0;
        }

        private string getcomputedKey(string strRandomKey)
        {
            string strcomputedKey = null;

            using (SHA256 objSha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  

                byte[] bytesEnconde = objSha256Hash.ComputeHash(Encoding.UTF8.GetBytes(strRandomKey));

                // Convert byte array to a string   
                StringBuilder objstrb = new StringBuilder();

                for (int i = 0; i < bytesEnconde.Length; i++)
                {
                    objstrb.Append(bytesEnconde[i].ToString("x2"));
                }
                strcomputedKey = objstrb.ToString();
            }

            return strcomputedKey;
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _objLogInObjects = null;
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~SesionEstado() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
