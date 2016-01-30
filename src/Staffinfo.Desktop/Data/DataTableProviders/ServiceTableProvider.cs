using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    public class ServiceTableProvider: ITableProvider, IDisposable
    {
        public string ErrorInfo { get; private set; }

        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel serviceModel)
        {
            if (serviceModel == null) throw new ArgumentNullException(nameof(serviceModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var service = serviceModel as ServiceModel;

            var cmd =
                new SqlCommand($@"INSERT INTO SERVICE VALUES('{service.ServiceTitle}'; SELECT MAX(ID) FROM SERVICE;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                service.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return service;
        }

        public BaseModel GetElementById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM SERVICE WHERE ID={id};");

            ServiceModel serviceModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                serviceModel = new ServiceModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    ServiceTitle = sqlDataReader[1].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return serviceModel;
        }

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var serviceList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM SERVICE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var serviceModel = new ServiceModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        ServiceTitle = sqlDataReader[1].ToString()
                    };

                    serviceList.Add(serviceModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return serviceList;
        }

        public bool Update(BaseModel service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service), Resources.DatabaseConnector_parameter_cannot_be_null);

            var serviceModel= service as ServiceModel;

            var cmd = new SqlCommand($@"UPDATE SERVICE SET SERVICE_TITLE='{serviceModel.ServiceTitle}' WHERE ID={serviceModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM SERVICE WHERE ID = '{id}'");
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
