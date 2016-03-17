using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице VIOLATION
    /// </summary>
    public class ViolationTableProvider: IWritableDirectoryTableContract<ViolationModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить нарушение
        /// </summary>
        /// <param name="violation">нарушение</param>
        /// <returns></returns>
        public ViolationModel Save(ViolationModel violation)
        {
            if (violation == null) throw new ArgumentNullException(nameof(violation), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO VIOLATION VALUES({violation.EmployeeId}, '{violation.Description}', '{violation.ViolationDate}'); SELECT MAX(ID) FROM VIOLATION;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                violation.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return violation;
        }

        /// <summary>
        /// Возвращает нарушение по id
        /// </summary>
        /// <param name="id">id нарушения</param>
        /// <returns></returns>
        public ViolationModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM VIOLATION WHERE ID={id};");

            ViolationModel violationModelModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                violationModelModel = new ViolationModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                    ViolationDate = DateTime.Parse(sqlDataReader[3].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return violationModelModel;
        }

        /// <summary>
        /// Возвращает список нарушений
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ViolationModel> Select()
        {
            var violationList = new ObservableCollection<ViolationModel>();

            var cmd = new SqlCommand("SELECT * FROM VIOLATION");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var violation = new ViolationModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        ViolationDate = DateTime.Parse(sqlDataReader[3].ToString())
                    };

                    violationList.Add(violation);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return violationList;
        }

        /// <summary>
        /// Обновить запись о нарушении
        /// </summary>
        /// <param name="violation">нарушение</param>
        /// <returns></returns>
        public bool Update(ViolationModel violation)
        {
            if (violation == null) throw new ArgumentNullException(nameof(violation), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE VIOLATION SET VIOLATOR_ID={violation.EmployeeId}, DESCRIPTION='{violation.Description}', VIOLATION_DATE='{violation.ViolationDate}' WHERE ID={violation.Id};");

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
        /// Удалить нарушение по id
        /// </summary>
        /// <param name="id">id нарушения</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM VIOLATION WHERE ID = '{id}'");
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
        /// Возвращает список нарушений по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<ViolationModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var violationList = new ObservableCollection<ViolationModel>();

            var cmd = new SqlCommand($"SELECT * FROM VIOLATION WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var violationModel = new ViolationModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        ViolationDate = DateTime.Parse(sqlDataReader[3].ToString())
                    };

                    violationList.Add(violationModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return violationList;
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