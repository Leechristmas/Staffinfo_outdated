using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице Operations_LOG - таблица логов
    /// </summary>
    public class LogTableProvider : IReadOnlyTableContract<DbLogRecord>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IReadOnlyTableContract implementation

        /// <summary>
        /// Возвращает запись из лога бд по id
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns></returns>
        public DbLogRecord Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM OPERATION_TYPE WHERE ID={id};");

            DbLogRecord dbLogRecord = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                dbLogRecord = new DbLogRecord
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    SessionId = Int64.Parse(sqlDataReader[1].ToString()),
                    OperationType = sqlDataReader[2].ToString(),
                    TableName = sqlDataReader[3].ToString(),
                    OperationTime = DateTime.Parse(sqlDataReader[4].ToString()),
                    Description = sqlDataReader[5].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return dbLogRecord;
        }

        /// <summary>
        /// Возвращает все записи из таблицы логов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DbLogRecord> Select()
        {
            ErrorInfo = null;

            var dbLogRecords = new ObservableCollection<DbLogRecord>();

            var cmd = new SqlCommand("SELECT * FROM OPERATIONS_LOG");

            var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

            while (sqlDataReader.Read())
            {
                try
                {
                    var dbLogRecord = new DbLogRecord()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        SessionId = Int64.Parse(sqlDataReader[1].ToString()),
                        OperationType = sqlDataReader[2].ToString(),
                        TableName = sqlDataReader[3].ToString(),
                        OperationTime = DateTime.Parse(sqlDataReader[4].ToString()),
                        Description = sqlDataReader[5].ToString()
                    };

                    dbLogRecords.Add(dbLogRecord);
                }
                catch (Exception ex)
                {
                    ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;//TODO: писать в log-файл
                }
            }
            sqlDataReader.Close();

            return dbLogRecords;
        }

        /// <summary>
        /// Возвращает все записи из таблицы логов начиная с указанной даты
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DbLogRecord> Select(DateTime startDate)
        {
            ErrorInfo = null;

            var dbLogRecords = new ObservableCollection<DbLogRecord>();

            var cmd = new SqlCommand($"SELECT * FROM OPERATIONS_LOG WHERE OPERATION_TIME >= '{startDate}'");

            var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

            while (sqlDataReader.Read())
            {
                try
                {
                    var dbLogRecord = new DbLogRecord()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        SessionId = Int64.Parse(sqlDataReader[1].ToString()),
                        OperationType = sqlDataReader[2].ToString(),
                        TableName = sqlDataReader[3].ToString(),
                        OperationTime = DateTime.Parse(sqlDataReader[4].ToString()),
                        Description = sqlDataReader[5].ToString()
                    };

                    dbLogRecords.Add(dbLogRecord);
                }
                catch (Exception ex)
                {
                    ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;//TODO: писать в log-файл
                }
            }
            sqlDataReader.Close();

            return dbLogRecords;
        }

        /// <summary>
        /// Возвращает последнюю добавленную запись
        /// </summary>
        /// <returns></returns>
        public DbLogRecord GetLast()
        {
            var cmd = new SqlCommand($@"SELECT * FROM OPERATION_TYPE WHERE ID=(SELECT MAX(ID) FROM OPERATION_TYPE);");

            DbLogRecord dbLogRecord = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                dbLogRecord = new DbLogRecord
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    OperationType = sqlDataReader[1].ToString(),
                    TableName = sqlDataReader[2].ToString(),
                    OperationTime = DateTime.Parse(sqlDataReader[3].ToString()),
                    Description = sqlDataReader[4].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return dbLogRecord;
        }
        #endregion

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

            _disposed = true;
        }

        #endregion

    }
}