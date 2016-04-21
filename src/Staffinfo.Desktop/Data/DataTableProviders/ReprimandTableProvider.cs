using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице REPRIMAND
    /// </summary>
    public class ReprimandTableProvider: IReadOnlyTableContract<ReprimandModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить выговор
        /// </summary>
        /// <param name="reprimand">выговор</param>
        /// <returns></returns>
        public ReprimandModel Save(ReprimandModel reprimand)
        {
            if (reprimand == null) throw new ArgumentNullException(nameof(reprimand), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"ADD_REPRIMAND {reprimand.EmployeeId}, {reprimand.ReprimandSum}, '{reprimand.ReprimandDate}', '{reprimand.Description}';");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                reprimand.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return reprimand;
        }

        /// <summary>
        /// Возвращает выговор по id
        /// </summary>
        /// <param name="id">id выговора</param>
        /// <returns></returns>
        public ReprimandModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM REPRIMAND WHERE ID={id};");

            ReprimandModel reprimandModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                reprimandModel = new ReprimandModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    ReprimandSum = decimal.Parse(sqlDataReader[2].ToString()),
                    ReprimandDate = DateTime.Parse(sqlDataReader[3].ToString()),
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

            return reprimandModel;
        }

        /// <summary>
        /// Возвращает список выговоров
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ReprimandModel> Select()
        {
            var reprimandList = new ObservableCollection<ReprimandModel>();

            var cmd = new SqlCommand("GET_REPRIMAND");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var reprimand = new ReprimandModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        ReprimandSum = decimal.Parse(sqlDataReader[2].ToString()),
                        ReprimandDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        Description = sqlDataReader[4].ToString()
                    };

                    reprimandList.Add(reprimand);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return reprimandList;
        }

        /// <summary>
        /// Обновить запись о выговоре
        /// </summary>
        /// <param name="reprimand">Контракт</param>
        /// <returns></returns>
        public bool Update(ReprimandModel reprimand)
        {
            if (reprimand == null) throw new ArgumentNullException(nameof(reprimand), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE REPRIMAND SET EMPLOYEE_ID={reprimand.EmployeeId}, SUM_OF_REPRIMAND={reprimand.ReprimandSum}, REPRIMAND_DATE='{reprimand.ReprimandDate}', DESCRIPTION='{reprimand.Description}' WHERE ID={reprimand.Id};");

            try
            {
                DataSingleton.Instance.DatabaseConnector.Execute(cmd);
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удалить выговор по id
        /// </summary>
        /// <param name="id">id выговора</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM REPRIMAND WHERE ID = '{id}'");
            try
            {
                DataSingleton.Instance.DatabaseConnector.Execute(cmd);
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает список выговоров по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<ReprimandModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var reprimandList = new ObservableCollection<ReprimandModel>();

            var cmd = new SqlCommand($"GET_REPRIMAND {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var reprimandModel = new ReprimandModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        ReprimandSum = decimal.Parse(sqlDataReader[2].ToString()),
                        ReprimandDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        Description = sqlDataReader[4].ToString()
                    };

                    reprimandList.Add(reprimandModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return reprimandList;
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