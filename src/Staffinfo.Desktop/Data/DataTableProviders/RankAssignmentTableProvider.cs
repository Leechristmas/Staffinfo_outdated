using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице RANK_ASSIGNMENT
    /// </summary>
    public class RankAssignmentTableProvider:IWritableDirectoryTableContract<RankAssignmentModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись о присвоении звания
        /// </summary>
        /// <param name="rankAssignmentModel">присвоение звания</param>
        /// <returns></returns>
        public RankAssignmentModel Save(RankAssignmentModel rankAssignmentModel)
        {
            if (rankAssignmentModel == null) throw new ArgumentNullException(nameof(rankAssignmentModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"ADD_RANK_ASSIGNMENT {rankAssignmentModel.EmployeeId}, '{rankAssignmentModel.Description}', '{rankAssignmentModel.AssignmentDate}', {rankAssignmentModel.PreviousRankId}, {rankAssignmentModel.NewRankId}, {rankAssignmentModel.OrderNumber};");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                rankAssignmentModel.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return rankAssignmentModel;
        }

        /// <summary>
        /// Возвращает присвоение звания по id
        /// </summary>
        /// <param name="id">id присвоения звания</param>
        /// <returns></returns>
        public RankAssignmentModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM RANK_ASSIGNMENT WHERE ID={id};");

            RankAssignmentModel rankAssignmentModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                rankAssignmentModel = new RankAssignmentModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                    AssignmentDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    PreviousRankId = int.Parse(sqlDataReader[4].ToString()),
                    NewRankId = int.Parse(sqlDataReader[5].ToString()),
                    OrderNumber = int.Parse(sqlDataReader[6].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return rankAssignmentModel;
        }

        /// <summary>
        /// Возвращает список всех присвоений званий
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RankAssignmentModel> Select()
        {
            var rankAssignmentList = new ObservableCollection<RankAssignmentModel>();

            var cmd = new SqlCommand("GET_RANK_ASSIGNMENT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var rankAssignmentModel = new RankAssignmentModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        AssignmentDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        PreviousRankId = int.Parse(sqlDataReader[4].ToString()),
                        NewRankId = int.Parse(sqlDataReader[5].ToString()),
                        OrderNumber = int.Parse(sqlDataReader[6].ToString())
                    };

                    rankAssignmentList.Add(rankAssignmentModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return rankAssignmentList;
        }

        /// <summary>
        /// Обновить запись о присвоении звания
        /// </summary>
        /// <param name="rankAssignmentModel">присвоение звания</param>
        /// <returns></returns>
        public bool Update(RankAssignmentModel rankAssignmentModel)
        {
            if (rankAssignmentModel == null) throw new ArgumentNullException(nameof(rankAssignmentModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE RANK_ASSIGNMENT SET EMPLOYEE_ID={rankAssignmentModel.EmployeeId}, DESCRIPTION='{rankAssignmentModel.Description}', ASSIGNMENT_DATE='{rankAssignmentModel.AssignmentDate}', PREV_RANK_ID={rankAssignmentModel.PreviousRankId}, NEW_RANK_ID={rankAssignmentModel.NewRankId}, ORDER_NUMBER={rankAssignmentModel.OrderNumber} WHERE ID={rankAssignmentModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM RANK_ASSIGNMENT WHERE ID = '{id}'");
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
        public ObservableCollection<RankAssignmentModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var rankAssignmentList = new ObservableCollection<RankAssignmentModel>();

            var cmd = new SqlCommand($"GET_RANK_ASSIGNMENT {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var rankAssignment = new RankAssignmentModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        AssignmentDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        PreviousRankId = int.Parse(sqlDataReader[4].ToString()),
                        NewRankId = int.Parse(sqlDataReader[5].ToString()),
                        OrderNumber = int.Parse(sqlDataReader[6].ToString())
                    };

                    rankAssignmentList.Add(rankAssignment);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return rankAssignmentList;
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