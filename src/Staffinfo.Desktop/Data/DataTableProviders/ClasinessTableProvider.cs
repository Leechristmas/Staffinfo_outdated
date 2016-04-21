using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей классности
    /// </summary>
    public class ClasinessTableProvider : IWritableDirectoryTableContract<ClasinessModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить запись в БД
        /// </summary>
        /// <param name="clasiness">Классность</param>
        /// <returns></returns>
        public ClasinessModel Save(ClasinessModel clasiness)
        {
            if (clasiness == null) throw new ArgumentNullException(nameof(clasiness), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($@"ADD_CLASINESS {clasiness.EmployeeId}, {clasiness.OrderNumber}, '{clasiness.ClasinessDate}', {clasiness.ClasinessLevel}, '{clasiness.Description}';");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                clasiness.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return clasiness;
        }

        /// <summary>
        /// Возвращает классность по id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ClasinessModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM CLASINESS WHERE ID={id};");

            ClasinessModel clasinessModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                clasinessModel = new ClasinessModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    OrderNumber = UInt16.Parse(sqlDataReader[2].ToString()),
                    ClasinessDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    ClasinessLevel = Byte.Parse(sqlDataReader[4].ToString()),
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

            return clasinessModel;
        }

        /// <summary>
        /// Возвращает записи о классности
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ClasinessModel> Select()
        {
            var clasinessList = new ObservableCollection<ClasinessModel>();

            var cmd = new SqlCommand("GET_CLASINESS");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var clasinessModel = new ClasinessModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        OrderNumber = UInt16.Parse(sqlDataReader[2].ToString()),
                        ClasinessDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        ClasinessLevel = Byte.Parse(sqlDataReader[4].ToString()),
                        Description = sqlDataReader[5].ToString()
                    };

                    clasinessList.Add(clasinessModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return clasinessList;
        }

        /// <summary>
        /// Обновляет запись классности
        /// </summary>
        /// <param name="clasiness">Классность</param>
        /// <returns></returns>
        public bool Update(ClasinessModel clasiness)
        {
            if (clasiness == null) throw new ArgumentNullException(nameof(clasiness), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE CLASINESS SET EMPLOYEE_ID={clasiness.EmployeeId}, ORDER_NUMBER={clasiness.OrderNumber}, CLASINESS_DATE='{clasiness.ClasinessDate}', CLASINESS_LEVEL={clasiness.ClasinessLevel}, DESCRIPTION='{clasiness.Description}' WHERE ID={clasiness.Id};");

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
        /// Удалить запись классности по id
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM CLASINESS WHERE ID = '{id}'");
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
        /// Возвращает список подтверждений классности по id служащего
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ObservableCollection<ClasinessModel> SelectByEmployeeId(long? id)
        {
            var clasinessList = new ObservableCollection<ClasinessModel>();

            var cmd = new SqlCommand($"GET_CLASINESS {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var clasinessModel = new ClasinessModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        OrderNumber = UInt16.Parse(sqlDataReader[2].ToString()),
                        ClasinessDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        ClasinessLevel = Byte.Parse(sqlDataReader[4].ToString()),
                        Description = sqlDataReader[5].ToString()
                    };

                    clasinessList.Add(clasinessModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return clasinessList;
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
