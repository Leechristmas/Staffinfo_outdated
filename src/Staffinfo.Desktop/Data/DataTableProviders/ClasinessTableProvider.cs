using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей классности
    /// </summary>
    public class ClasinessTableProvider : ITableProvider, IDisposable
    {
        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel clasiness)
        {
            if (clasiness == null) throw new ArgumentNullException(nameof(clasiness), Resources.DatabaseConnector_parameter_cannot_be_null);

            var clasinessModel = clasiness as ClasinessModel;

            var cmd =
                new SqlCommand($@"INSERT INTO CLASINESS VALUES({clasinessModel.EmployeeId}, {clasinessModel.OrderNumber}, '{clasinessModel.ClasinessDate}', {clasinessModel.ClasinessLevel}, '{clasinessModel.Description}'); SELECT MAX(ID) FROM POST;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                clasinessModel.Id = Int64.Parse(sqlDataReader[0].ToString());
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

        public string ErrorInfo { get; set; }

        public BaseModel GetElementById(long? id)
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

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var clasinessList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM CLASINESS");

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

        public bool Update(BaseModel clasiness)
        {
            if (clasiness == null) throw new ArgumentNullException(nameof(clasiness), Resources.DatabaseConnector_parameter_cannot_be_null);

            var clasinessModel = clasiness as ClasinessModel;

            var cmd = new SqlCommand($@"UPDATE CLASINESS SET EMPLOYEE_ID={clasinessModel.EmployeeId}, ORDER_NUMBER={clasinessModel.OrderNumber}, CLASINESS_DATE='{clasinessModel.ClasinessDate}', CLASINESS_LEVEL={clasinessModel.ClasinessLevel}, DESCRIPTION='{clasinessModel.Description}' WHERE ID={clasinessModel.Id};");

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
