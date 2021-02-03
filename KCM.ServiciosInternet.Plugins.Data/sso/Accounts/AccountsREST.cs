/***********************************************************************************************
 *  Date    : January 2021
 *  Author  : Eng. Jorge Flores Miguel  KCUS/C84818
 *  Email   : jorgekain00@gmail.com
 *  Remarks : 
 ***********************************************************************************************/
namespace KCM.ServiciosInternet.Plugins.Data.sso.Accounts
{
    using KCM.ServiciosInternet.Plugins.Data.sso.Interfaces;
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    /// <summary>
    /// Constains all Single Sign Operations
    /// </summary>
    /// <remarks>
    /// <para>This class cannot be instantied by itselft</para>
    /// </remarks>
    public abstract class AccountsREST : IDisposable
    {
        /// <summary>
        /// Return Successful operation
        /// </summary>
        public const bool SUCCESSFUL = true;
        /// <summary>
        /// Return unSuccessful operation
        /// </summary>
        public const bool UNSUCESSFUL = false;


        public abstract bool requestResetPasswordEmail(ISingleSignOnData objData);

        public abstract bool updatePasswordWithToken(ISingleSignOnData objData);
                         
        public abstract bool logIn(ISingleSignOnData objData);
                         
        public abstract bool getAccountInfo(ISingleSignOnData objData, bool IsWithUID = true);
                         
        public abstract bool setAccountInfo(ISingleSignOnData objData, bool IsWithUID = true);
                         
        public abstract bool finalizeRegistration(ISingleSignOnData objData);
                         
        public abstract bool logOut(ISingleSignOnData objData);
                         
        public abstract bool signUp(ISingleSignOnData objData);



        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~AccountsREST() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
