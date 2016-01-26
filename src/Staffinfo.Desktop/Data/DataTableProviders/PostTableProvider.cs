using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей должностей
    /// </summary>
    public class PostTableProvider: ITableProvider, IDisposable
    {
        public string ErrorInfo { get; private set; }

        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel postModel)
        {
            if(postModel == null) throw new ArgumentNullException(nameof(postModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var post = postModel as PostModel;

            var cmd = 
                new SqlCommand($@"INSERT INTO POST VALUES('{post.PostTitle}',{post.ServiceId}); SELECT MAX(ID) FROM POST;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                post.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return post;
        }

        public BaseModel GetElementById(long? id)
        {
            if(!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM POST WHERE ID={id};");

            PostModel postModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                postModel = new PostModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    PostTitle = sqlDataReader[1].ToString(),
                    ServiceId = Int64.Parse(sqlDataReader[2].ToString())
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

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var postList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM POST");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var postModel = new PostModel()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        PostTitle = sqlDataReader[1].ToString(),
                        ServiceId = Int64.Parse(sqlDataReader[2].ToString())
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

        public bool Update(BaseModel post)
        {
            if(post == null) throw new ArgumentNullException(nameof(post), Resources.DatabaseConnector_parameter_cannot_be_null);

            var postModel = post as PostModel;

            var cmd = new SqlCommand($@"UPDATE POST SET POST_TITLE='{postModel.PostTitle}', SERVICE_ID={postModel.ServiceId} WHERE ID={postModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM POST WHERE ID = '{id}'");
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
