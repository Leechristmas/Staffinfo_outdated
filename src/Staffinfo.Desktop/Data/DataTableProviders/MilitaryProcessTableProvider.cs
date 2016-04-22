using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице MILITARY_PROCESS
    /// </summary>
    public class MilitaryProcessTableProvider: IWritableDirectoryTableContract<MilitaryProcessModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись о прохождении службы
        /// </summary>
        /// <param name="militaryProcess">прохождение службы</param>
        /// <returns></returns>
        public MilitaryProcessModel Save(MilitaryProcessModel militaryProcess)
        {
            if (militaryProcess == null) throw new ArgumentNullException(nameof(militaryProcess), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"ADD_MILITARY_PROCESS {militaryProcess.EmployeeId}, '{militaryProcess.Description}', '{militaryProcess.StartDate}', '{militaryProcess.FinishDate}', {militaryProcess.MilitaryUnitId};");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                militaryProcess.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return militaryProcess;
        }

        /// <summary>
        /// Возвращает прохождение службы по id
        /// </summary>
        /// <param name="id">id прохождения службы</param>
        /// <returns></returns>
        public MilitaryProcessModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM MILITARY_PROCESS WHERE ID={id};");

            MilitaryProcessModel militaryProcessModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                militaryProcessModel = new MilitaryProcessModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                    StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    FinishDate = DateTime.Parse(sqlDataReader[4].ToString()),
                    MilitaryUnitId = int.Parse(sqlDataReader[5].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return militaryProcessModel;
        }

        /// <summary>
        /// Возвращает список всех прохождений службы
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MilitaryProcessModel> Select()
        {
            var militaryProcessList = new ObservableCollection<MilitaryProcessModel>();

            var cmd = new SqlCommand("GET_MILITARY_PROCESS");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var militaryProcessModel = new MilitaryProcessModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString()),
                        MilitaryUnitId = int.Parse(sqlDataReader[5].ToString())
                    };

                    militaryProcessList.Add(militaryProcessModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return militaryProcessList;
        }

        /// <summary>
        /// Обновить запись о прохождении службы
        /// </summary>
        /// <param name="militaryProcessModel">прохождение службы</param>
        /// <returns></returns>
        public bool Update(MilitaryProcessModel militaryProcessModel)
        {
            if (militaryProcessModel == null) throw new ArgumentNullException(nameof(militaryProcessModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE MILITARY_PROCESS SET EMPLOYEE_ID={militaryProcessModel.EmployeeId}, DESCRIPTION='{militaryProcessModel.Description}', START_DATE='{militaryProcessModel.StartDate}', FINISH_DATE='{militaryProcessModel.FinishDate}', MILITARY_UNIT_ID = {militaryProcessModel.MilitaryUnitId} WHERE ID={militaryProcessModel.Id};");

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
        /// Удалить прохождение службы по id
        /// </summary>
        /// <param name="id">id прохождения службы</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM MILITARY_PROCESS WHERE ID = '{id}'");
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
        /// Возвращает список прохождений службы по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<MilitaryProcessModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var militaryProcessList = new ObservableCollection<MilitaryProcessModel>();

            var cmd = new SqlCommand($"GET_MILITARY_PROCESS {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var militaryProcess = new MilitaryProcessModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString()),
                        MilitaryUnitId = int.Parse(sqlDataReader[5].ToString())
                    };

                    militaryProcessList.Add(militaryProcess);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return militaryProcessList;
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