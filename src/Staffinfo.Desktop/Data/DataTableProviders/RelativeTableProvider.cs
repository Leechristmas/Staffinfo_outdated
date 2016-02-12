using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице RELATIVE
    /// </summary>
    public class RelativeTableProvider: IWritableDirectoryTableContract<RelativeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить родственника
        /// </summary>
        /// <param name="relativeModel">родственник</param>
        /// <returns></returns>
        public RelativeModel Save(RelativeModel relativeModel)
        {
            if (relativeModel == null) throw new ArgumentNullException(nameof(relativeModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO RELATIVE VALUES({relativeModel.EmployeeId}, {relativeModel.RelationTypeId},'{relativeModel.FirstName}', '{relativeModel.MiddleName}', '{relativeModel.LastName}', '{relativeModel.BornDate}', '{relativeModel.Description}'); SELECT MAX(ID) FROM RELATIVE;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                relativeModel.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return relativeModel;
        }

        /// <summary>
        /// Возвращает родственника по id
        /// </summary>
        /// <param name="id">id родственника</param>
        /// <returns></returns>
        public RelativeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM RELATIVE WHERE ID={id};");

            RelativeModel relativeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                relativeModel = new RelativeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    RelationTypeId = int.Parse(sqlDataReader[2].ToString()),
                    FirstName = sqlDataReader[3].ToString(),
                    MiddleName = sqlDataReader[4].ToString(),
                    LastName = sqlDataReader[5].ToString(),
                    BornDate = DateTime.Parse(sqlDataReader[6].ToString()),
                    Description = sqlDataReader[7].ToString(),
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return relativeModel;
        }

        /// <summary>
        /// Возвращает список родственников всех служащих
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RelativeModel> Select()
        {
            var relativeList = new ObservableCollection<RelativeModel>();

            var cmd = new SqlCommand("SELECT * FROM RELATIVE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var relativeModel = new RelativeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        RelationTypeId = int.Parse(sqlDataReader[2].ToString()),
                        FirstName = sqlDataReader[3].ToString(),
                        MiddleName = sqlDataReader[4].ToString(),
                        LastName = sqlDataReader[5].ToString(),
                        BornDate = DateTime.Parse(sqlDataReader[6].ToString()),
                        Description = sqlDataReader[7].ToString(),

                    };

                    relativeList.Add(relativeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return relativeList;
        }

        /// <summary>
        /// Обновить запись о родственнике
        /// </summary>
        /// <param name="relativeModel">родственник</param>
        /// <returns></returns>
        public bool Update(RelativeModel relativeModel)
        {
            if (relativeModel == null) throw new ArgumentNullException(nameof(relativeModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($"UPDATE RELATIVE SET EMPLOYEE_ID={relativeModel.EmployeeId}, RELATION_TYPE_ID={relativeModel.RelationTypeId}, " +
                                     $"FIRST_NAME='{relativeModel.FirstName}', MIDDLE_NAME='{relativeModel.MiddleName}', LAST_NAME='{relativeModel.LastName}', " +
                                     $"BORN_DATE='{relativeModel.BornDate}', DESCRIPTION='{relativeModel.Description}' WHERE ID={relativeModel.Id};");

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
        /// Удалить родственника по id
        /// </summary>
        /// <param name="id">id родственника</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM RELATIVE WHERE ID = '{id}'");
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
        /// Возвращает родственников по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<RelativeModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var relativeList = new ObservableCollection<RelativeModel>();

            var cmd = new SqlCommand($"SELECT * FROM RELATIVE WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var relative = new RelativeModel
                    {
                        Id = int.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = int.Parse(sqlDataReader[1].ToString()),
                        RelationTypeId = int.Parse(sqlDataReader[2].ToString()),
                        FirstName = sqlDataReader[3].ToString(),
                        MiddleName = sqlDataReader[4].ToString(),
                        LastName = sqlDataReader[5].ToString(),
                        BornDate = DateTime.Parse(sqlDataReader[6].ToString()),
                        Description = sqlDataReader[7].ToString(),
                    };

                    relativeList.Add(relative);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return relativeList;
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