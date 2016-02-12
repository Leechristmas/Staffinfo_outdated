using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице SERTIFICATION
    /// </summary>
    public class SertificationTableProvider: IWritableDirectoryTableContract<SertificationModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить аттестацию
        /// </summary>
        /// <param name="sertification">аттестация</param>
        /// <returns></returns>
        public SertificationModel Save(SertificationModel sertification)
        {
            if (sertification == null) throw new ArgumentNullException(nameof(sertification), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO SERTIFICATION VALUES({sertification.EmployeeId}, '{sertification.SertificationDate}', '{sertification.Description}'); SELECT MAX(ID) FROM SERTIFICATION;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                sertification.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return sertification;
        }

        /// <summary>
        /// Возвращает аттестацию по id
        /// </summary>
        /// <param name="id">id аттестации</param>
        /// <returns></returns>
        public SertificationModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM SERTIFICATION WHERE ID={id};");

            SertificationModel sertificationModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                sertificationModel = new SertificationModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    SertificationDate = DateTime.Parse(sqlDataReader[2].ToString()),
                    Description = sqlDataReader[3].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return sertificationModel;
        }

        /// <summary>
        /// Возвращает список аттестаций
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SertificationModel> Select()
        {
            var sertificationList = new ObservableCollection<SertificationModel>();

            var cmd = new SqlCommand("SELECT * FROM SERTIFICATION");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var sertification = new SertificationModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        SertificationDate = DateTime.Parse(sqlDataReader[2].ToString()),
                        Description = sqlDataReader[4].ToString()
                    };

                    sertificationList.Add(sertification);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return sertificationList;
        }

        /// <summary>
        /// Обновить запись об аттестации
        /// </summary>
        /// <param name="sertification">аттестация</param>
        /// <returns></returns>
        public bool Update(SertificationModel sertification)
        {
            if (sertification == null) throw new ArgumentNullException(nameof(sertification), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE SERTIFICATION SET EMPLOYEE_ID={sertification.EmployeeId}, SERTIFICATION_DATE='{sertification.SertificationDate}', DESCRIPTION='{sertification.Description}' WHERE ID={sertification.Id};");

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
        /// Удалить аттестацию по id
        /// </summary>
        /// <param name="id">id аттестации</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM SERTIFICATION WHERE ID = '{id}'");
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
        /// Возвращает список аттестаций по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<SertificationModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var sertificationList = new ObservableCollection<SertificationModel>();

            var cmd = new SqlCommand($"SELECT * FROM SERTIFICATION WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var sertificationModel = new SertificationModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        SertificationDate = DateTime.Parse(sqlDataReader[2].ToString()),
                        Description = sqlDataReader[3].ToString()
                    };

                    sertificationList.Add(sertificationModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return sertificationList;
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