using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей должностей
    /// </summary>
    public class PostTableProvider: IReadOnlyTableContract<PostModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IReadOnlyTableContract implementation

        /// <summary>
        /// Возвращает должность по id
        /// </summary>
        /// <param name="id">id должности</param>
        /// <returns></returns>
        public PostModel Select(long? id)
        {
            if(!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"GET_POST {id};");

            PostModel postModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                postModel = new PostModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    PostTitle = sqlDataReader[1].ToString(),
                    ServiceId = Int64.Parse(sqlDataReader[2].ToString()),
                    PostWeight = Int32.Parse(sqlDataReader[3].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return postModel;
        }

        /// <summary>
        /// Возвращает все должности
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PostModel> Select()
        {
            var postList = new ObservableCollection<PostModel>();

            var cmd = new SqlCommand("GET_POST");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var postModel = new PostModel()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        PostTitle = sqlDataReader[1].ToString(),
                        ServiceId = Int64.Parse(sqlDataReader[2].ToString()),
                        PostWeight = Int32.Parse(sqlDataReader[3].ToString())
                    };

                    postList.Add(postModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return postList;
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
