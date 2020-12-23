using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Plugins.Entities.BD;
using KCM.ServiciosInternet.Plugins.Entities;


namespace KCM.ServiciosInternet.Plugins.Business.BD
{
    class SessionBD : IDisposable
    {

        private SqlConnection objSqlConn;
        private SqlCommand objSqlCmd;

        private string _strConn;

        private string _strInsert = @"INSERT INTO [dbo].[gigyaJFMKeys]
                                       ([hexKey]
                                       ,[randomkeyVc]
                                       ,[fecAltaDt]
                                       ,[ASP.NET_SessionId])
                                 VALUES
                                       (@hexKey ,@randomkeyVc, @fecAltaDt, @SessionId)";

        private string _strSelect = @"SELECT  [hexKey]
                                              ,[randomkeyVc]
                                              ,[fecAltaDt]
                                          FROM [dbo].[gigyaJFMKeys]
                                        WHERE hexKey = @hexKey";

        private string _strSelectCount = @"SELECT COUNT(*)
                                          FROM [dbo].[gigyaJFMKeys]
                                        WHERE [ASP.NET_SessionId] = @SessionId";

        private string _strDelete = @"DELETE FROM [dbo].[gigyaJFMKeys] WHERE hexKey = @hexKey";

        private string _strDeleteKeys = @"DELETE FROM [dbo].[gigyaJFMKeys] WHERE [fecAltaDt] < @todayDt";

        private string strConn
        {
            get
            {
                if (_strConn == null)
                {
                    return clsBDConfigSession.strConnBD;
                }

                return _strConn;
            }
            set
            {
                this._strConn = value;
            }
        }

        public SessionBD() : this(null)
        {

        }

        public SessionBD(string strConnBD)
        {
            if (strConnBD == null)
            {
                strConnBD = this.strConn;
            }
            else
            {
                this.strConn = strConnBD;
            }
            this.objSqlConn = new SqlConnection(this.strConn);
            this.objSqlCmd = new SqlCommand();
            this.objSqlConn.Open();
            this.objSqlCmd.Connection = this.objSqlConn;
        }

        public bool insertKey(clsSessionState objSessionState, string strCookieSessionId)
        {
            objSqlCmd.CommandText = this._strInsert;
            objSqlCmd.CommandType = System.Data.CommandType.Text;
            objSqlCmd.Parameters.Clear();

            SqlParameter objSqlParam = new SqlParameter("@hexKey", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, objSessionState.objLogInObjects.strComputedKey);

            objSqlCmd.Parameters.Add(objSqlParam);

            objSqlParam = new SqlParameter("@randomkeyVc", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, objSessionState.objLogInObjects.strRandomKey);

            objSqlCmd.Parameters.Add(objSqlParam);

            objSqlParam = new SqlParameter("@fecAltaDt", System.Data.SqlDbType.DateTime, 0
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, objSessionState.objLogInObjects.dtRandomKeyDate);

            objSqlCmd.Parameters.Add(objSqlParam);

            objSqlParam = new SqlParameter("@SessionId", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, strCookieSessionId);

            objSqlCmd.Parameters.Add(objSqlParam);

            if (objSqlCmd.ExecuteNonQuery() > 0)
            {
                return true;
            }

            return false;
        }

        public bool deleteKey(clsSessionState objSessionState)
        {
            objSqlCmd.CommandText = this._strDelete;
            objSqlCmd.CommandType = System.Data.CommandType.Text;
            objSqlCmd.Parameters.Clear();

            SqlParameter objSqlParam = new SqlParameter("@hexKey", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, objSessionState.objLogInObjects.strComputedKey);

            objSqlCmd.Parameters.Add(objSqlParam);

            if (objSqlCmd.ExecuteNonQuery() > 0)
            {
                return true;
            }

            return false;
        }

        public bool isExistselectKey(clsSessionState objSessionState)
        {
            objSqlCmd.CommandText = this._strSelect;
            objSqlCmd.CommandType = System.Data.CommandType.Text;
            objSqlCmd.Parameters.Clear();

            SqlParameter objSqlParam = new SqlParameter("@hexKey", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, objSessionState.objLogInObjects.strComputedKey);

            objSqlCmd.Parameters.Add(objSqlParam);

            using (SqlDataReader objSqlRrd = objSqlCmd.ExecuteReader())
            {
                if (!objSqlRrd.HasRows)
                {
                    return false;
                }

                while (objSqlRrd.Read())
                {
                    objSessionState.objLogInObjects.strRandomKey = objSqlRrd.GetString(1);
                    objSessionState.objLogInObjects.dtRandomKeyDate = objSqlRrd.GetDateTime(2);
                }
            }

            return true;
        }

        public bool isReachMaxRequest(string strCookieSessionId)
        {
            int intMaxRequest = 0;
            objSqlCmd.CommandText = this._strSelectCount;
            objSqlCmd.CommandType = System.Data.CommandType.Text;
            objSqlCmd.Parameters.Clear();

            SqlParameter objSqlParam = new SqlParameter("@SessionId", System.Data.SqlDbType.NVarChar, 512
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, strCookieSessionId);

            objSqlCmd.Parameters.Add(objSqlParam);

            using (SqlDataReader objSqlRrd = objSqlCmd.ExecuteReader())
            {
                if (!objSqlRrd.HasRows)
                {
                    return false;
                }

                while (objSqlRrd.Read())
                {
                    intMaxRequest = objSqlRrd.GetInt32(0);
                }

            }
            if (intMaxRequest > 10)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal bool deleteExpiredKeys(int intExpirationKeyMins)
        {

            objSqlCmd.CommandText = this._strDeleteKeys;
            objSqlCmd.CommandType = System.Data.CommandType.Text;
            objSqlCmd.Parameters.Clear();

            DateTime dtToday = DateTime.Now.AddMinutes(-intExpirationKeyMins);

            SqlParameter objSqlParam = new SqlParameter("@todayDt", System.Data.SqlDbType.DateTime, 0
                , System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Default, dtToday);

            objSqlCmd.Parameters.Add(objSqlParam);

            if (objSqlCmd.ExecuteNonQuery() > 0)
            {
                return true;
            }

            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                    objSqlConn.Close();
                    objSqlConn = null;
                    objSqlCmd = null;
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~SessionBD() {
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
