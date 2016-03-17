using System;
using System.Data.SqlClient;
using Staffinfo.Desktop.Properties;
using Staffinfo.Desktop.Shared;

namespace Staffinfo.Desktop.Data
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    public class DatabaseConnector: IDisposable
    {
        public DatabaseConnector()
        { }

        public DatabaseConnector(string urlDataBase)
        {
            _urlDataBase = urlDataBase;
            _sqlConnection = new SqlConnection(_urlDataBase);
            _sqlConnection.Open();
        }

        /// <summary>
        /// строка соединения
        /// </summary>
        private readonly string _urlDataBase;
        
        private readonly SqlConnection _sqlConnection;

        /// <summary>
        /// Выполняет запрос и возвращает результат в виде SqlDataReader
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_parameter_cannot_be_null);

            var sqlCommandToExecute = new SqlCommand(sqlCommand, _sqlConnection);

            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = sqlCommandToExecute.ExecuteReader();
            }
            catch(Exception ex)
            {
                throw new Exception(Resources.DatabaseConnector_query_has_some_errors + ex.Message);
            }

            return sqlDataReader;
        }

        /// <summary>
        /// Выполняет запрос и возвращает результат в виде SqlDataReader
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(SqlCommand sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_parameter_cannot_be_null);

            sqlCommand.Connection = _sqlConnection;

            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            catch(Exception ex)
            {
                throw new Exception(Resources.DatabaseConnector_query_has_some_errors + ex.Message);
            }

            return sqlDataReader;
        }

        /// <summary>
        /// Выполняет запрос без возврата результат
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        public void Execute(string sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_parameter_cannot_be_null);

            var sqlCommandToExecute = new SqlCommand(sqlCommand, _sqlConnection);

            try
            {
                sqlCommandToExecute.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(Resources.DatabaseConnector_query_has_some_errors + ex.Message);
            }
        }

        /// <summary>
        /// Выполняет запрос без возврата результата
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        public void Execute(SqlCommand sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_parameter_cannot_be_null);

            sqlCommand.Connection = _sqlConnection;
            
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(Resources.DatabaseConnector_query_has_some_errors + ex.Message);
            }
        }

        #region IDisposable implementation

        private bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || disposing)
                return;

            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }

            _disposed = true;
        }
        #endregion

    }
}