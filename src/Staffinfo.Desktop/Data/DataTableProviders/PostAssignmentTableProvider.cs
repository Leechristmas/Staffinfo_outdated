using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице POST_ASSIGNMENT
    /// </summary>
    public class PostAssignmentTableProvider: IWritableDirectoryTableContract<PostAssignmentModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись о присвоении звания
        /// </summary>
        /// <param name="postAssignmentModel">присвоение звания</param>
        /// <returns></returns>
        public PostAssignmentModel Save(PostAssignmentModel postAssignmentModel)
        {
            if (postAssignmentModel == null) throw new ArgumentNullException(nameof(postAssignmentModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO POST_ASSIGNMENT VALUES({postAssignmentModel.EmployeeId}, '{postAssignmentModel.Description}', '{postAssignmentModel.AssignmentDate}', {postAssignmentModel.PreviousPostId}, {postAssignmentModel.NewPostId}, {postAssignmentModel.OrderNumber}); SELECT MAX(ID) FROM POST_ASSIGNMENT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                postAssignmentModel.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return postAssignmentModel;
        }

        /// <summary>
        /// Возвращает присвоение звания по id
        /// </summary>
        /// <param name="id">id присвоения звания</param>
        /// <returns></returns>
        public PostAssignmentModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM POST_ASSIGNMENT WHERE ID={id};");

            PostAssignmentModel postAssignmentModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                postAssignmentModel = new PostAssignmentModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return postAssignmentModel;
        }

        /// <summary>
        /// Возвращает список всех присвоений званий
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PostAssignmentModel> Select()
        {
            var postAssignmentList = new ObservableCollection<PostAssignmentModel>();

            var cmd = new SqlCommand("SELECT * FROM POST_ASSIGNMENT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var postAssignmentModel = new PostAssignmentModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        AssignmentDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        PreviousPostId = int.Parse(sqlDataReader[4].ToString()),
                        NewPostId = int.Parse(sqlDataReader[5].ToString()),
                        OrderNumber = int.Parse(sqlDataReader[6].ToString())
                    };

                    postAssignmentList.Add(postAssignmentModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return postAssignmentList;
        }

        /// <summary>
        /// Обновить запись о присвоении звания
        /// </summary>
        /// <param name="postAssignmentModel">присвоение звания</param>
        /// <returns></returns>
        public bool Update(PostAssignmentModel postAssignmentModel)
        {
            if (postAssignmentModel == null) throw new ArgumentNullException(nameof(postAssignmentModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE POST_ASSIGNMENT SET EMPLOYEE_ID={postAssignmentModel.EmployeeId}, DESCRIPTION='{postAssignmentModel.Description}', ASSIGNMENT_DATE='{postAssignmentModel.AssignmentDate}', PREV_POST_ID={postAssignmentModel.PreviousPostId}, NEW_POST_ID={postAssignmentModel.NewPostId}, ORDER_NUMBER={postAssignmentModel.OrderNumber} WHERE ID={postAssignmentModel.Id};");

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
        /// Удалить присвоение звания по id
        /// </summary>
        /// <param name="id">id присвоения звания</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM POST_ASSIGNMENT WHERE ID = '{id}'");
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
        /// Возвращает список присвоений званий по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<PostAssignmentModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var postAssignmentList = new ObservableCollection<PostAssignmentModel>();

            var cmd = new SqlCommand($"SELECT * FROM POST_ASSIGNMENT WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var postAssignment = new PostAssignmentModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        AssignmentDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        PreviousPostId = int.Parse(sqlDataReader[4].ToString()),
                        NewPostId = int.Parse(sqlDataReader[5].ToString()),
                        OrderNumber = int.Parse(sqlDataReader[6].ToString())
                    };

                    postAssignmentList.Add(postAssignment);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return postAssignmentList;
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