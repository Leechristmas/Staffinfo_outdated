using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице RELATIVE_TYPE
    /// </summary>
    public class RelativeTypeTableProvider: IReadOnlyTableContract<RelativeTypeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IReadOnlyTableContract implementation

        /// <summary>
        /// Возвращает тип родства по id
        /// </summary>
        /// <param name="id">id типа родства</param>
        /// <returns></returns>
        public RelativeTypeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM RELATIVE_TYPE WHERE ID={id};");

            RelativeTypeModel relativeTypeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                relativeTypeModel = new RelativeTypeModel
                {
                    Id = int.Parse(sqlDataReader[0].ToString()),
                    RelativeType = sqlDataReader[1].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return relativeTypeModel;
        }

        /// <summary>
        /// Возвращает все типы родства
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RelativeTypeModel> Select()
        {
            var relativeTypeList = new ObservableCollection<RelativeTypeModel>();

            var cmd = new SqlCommand("SELECT * FROM RELATIVE_TYPE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var relativeTypeModel = new RelativeTypeModel()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        RelativeType = sqlDataReader[1].ToString()
                    };

                    relativeTypeList.Add(relativeTypeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return relativeTypeList;
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