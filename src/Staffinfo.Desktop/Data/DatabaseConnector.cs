using System;
using System.Data.SqlClient;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    public class DatabaseConnector: IDisposable
    {
        /// <summary>
        /// строка соединения
        /// </summary>
        private const string UrlDataBase = @"Data Source=DESKTOP-QV3R6NU\SQLEXPRESS; Initial Catalog = staffinfo; Integrated Security=SSPI;";

        private readonly SqlConnection _sqlConnection;

        public DatabaseConnector()
        {
            _sqlConnection = new SqlConnection(UrlDataBase);
            _sqlConnection.Open();
        }

        /// <summary>
        /// Выполняет запрос и возвращает результат в виде SqlDataReader
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_ExecuteReader_parameter_cannot_be_null_);

            var sqlCommandToExecute = new SqlCommand(sqlCommand, _sqlConnection);

            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = sqlCommandToExecute.ExecuteReader();
            }
            catch
            {
                throw new Exception(Resources.DatabaseConnector_ExecuteReader_QueryError);
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
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_ExecuteReader_parameter_cannot_be_null_);

            sqlCommand.Connection = _sqlConnection;

            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            catch
            {
                throw new Exception(Resources.DatabaseConnector_ExecuteReader_QueryError);
            }

            return sqlDataReader;
        }

        /// <summary>
        /// Выполняет запрос без возврата результат
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        public void Execute(string sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_ExecuteReader_parameter_cannot_be_null_);

            var sqlCommandToExecute = new SqlCommand(sqlCommand, _sqlConnection);

            try
            {
                sqlCommandToExecute.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception(Resources.DatabaseConnector_ExecuteReader_QueryError);
            }
        }

        /// <summary>
        /// Выполняет запрос без возврата результата
        /// </summary>
        /// <param name="sqlCommand">SQL-запрос</param>
        public void Execute(SqlCommand sqlCommand)
        {
            if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand), Resources.DatabaseConnector_ExecuteReader_parameter_cannot_be_null_);

            sqlCommand.Connection = _sqlConnection;

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception(Resources.DatabaseConnector_ExecuteReader_QueryError);
            }
        }

        /// <summary>
        /// Возвращает список view models для служащих из БД
        /// </summary>
        //public ObservableCollection<EmployeeViewModel> GetEmployeeViewModels()
        //{
        //    var cmd = new SqlCommand("SELECT * FROM EMPLOYEE", _sqlConnection);

        //    SqlDataReader reader = cmd.ExecuteReader();

        //    var employeeViewModels = new ObservableCollection<EmployeeViewModel>();

        //    while (reader.Read())
        //    {
        //        //var employeeModel = new EmployeeModel(
        //        //    Int64.Parse(reader["ID"].ToString()),
        //        //    reader["EMPLOYEE_FIRSTNAME"].ToString(),
        //        //    reader["EMPLOYEE_MIDDLENAME"].ToString(),
        //        //    reader["EMPLOYEE_LASTNAME"].ToString(),
        //        //    reader["PERSONAL_KEY"].ToString(),
        //        //    Int64.Parse(reader["POST_ID"].ToString()),
        //        //    Int64.Parse(reader["RANK_ID"].ToString()),
        //        //    DateTime.Parse(reader["BORN_DATE"].ToString()),
        //        //    DateTime.Parse(reader["JOB_START_DATE"].ToString()),
        //        //    reader["ADDRESS"].ToString(),
        //        //    reader["PASPORT"].ToString(),
        //        //    reader["PHONE_NUMBER"].ToString());

        //        //employeeViewModels.Add(new EmployeeViewModel(employeeModel));
        //    }

        //    return employeeViewModels;
        //}

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