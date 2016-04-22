using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент для работы с таблицей SERVICE
    /// </summary>
    public class ServiceTableProvider: IReadOnlyTableContract<ServiceModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IReadOnlyTableContract implementation

        /// <summary>
        /// Возвращает службу по id
        /// </summary>
        /// <param name="id">id службы</param>
        /// <returns></returns>
        public ServiceModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"GET_SERVICE {id};");

            ServiceModel serviceModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                serviceModel = new ServiceModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    ServiceTitle = sqlDataReader[1].ToString(),
                    GroupId = Int32.Parse(sqlDataReader[2].ToString()),
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

        /// <summary>
        /// Возвращает все службы
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ServiceModel> Select()
        {
            var serviceList = new ObservableCollection<ServiceModel>();

            var cmd = new SqlCommand("GET_SERVICE");
            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var serviceModel = new ServiceModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        ServiceTitle = sqlDataReader[1].ToString(),
                        GroupId = Int32.Parse(sqlDataReader[2].ToString()),
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
            finally
            {
             sqlDataReader?.Close();   
            }
            return serviceList;
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
