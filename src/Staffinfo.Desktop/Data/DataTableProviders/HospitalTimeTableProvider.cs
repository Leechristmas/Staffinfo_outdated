using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице HOSPITAL_TIME
    /// </summary>
    public class HospitalTimeTableProvider: IWritableDirectoryTableContract<HospitalTimeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись о больничном
        /// </summary>
        /// <param name="hospitalTime">больничный</param>
        /// <returns></returns>
        public HospitalTimeModel Save(HospitalTimeModel hospitalTime)
        {
            if (hospitalTime == null) throw new ArgumentNullException(nameof(hospitalTime), Resources.DatabaseConnector_parameter_cannot_be_null);

            

            var cmd =
                new SqlCommand($@"ADD_HOSPITAL_TIME @EMPLOYEE_ID, @DESCRIPTION, @START_DATE, @FINISH_DATE;");

            var employeeId = cmd.Parameters.Add("@EMPLOYEE_ID", SqlDbType.Int);
            employeeId.Value = hospitalTime.EmployeeId;

            var description = cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar);
            description.Value = hospitalTime.Description;

            var startDate = cmd.Parameters.Add("@START_DATE", SqlDbType.DateTime);
            startDate.Value = hospitalTime.StartDate;

            var finishDate = cmd.Parameters.Add("@FINISH_DATE", SqlDbType.DateTime);
            finishDate.Value = hospitalTime.FinishDate;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                hospitalTime.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return hospitalTime;
        }

        /// <summary>
        /// Возвращает больничный по id
        /// </summary>
        /// <param name="id">id больничного</param>
        /// <returns></returns>
        public HospitalTimeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM HOSPITAL_TIME WHERE ID={id};");

            HospitalTimeModel hospitalTimeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();
                
                hospitalTimeModel = new HospitalTimeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                    StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    FinishDate = DateTime.Parse(sqlDataReader[4].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return hospitalTimeModel;
        }

        /// <summary>
        /// Возвращает список больничных
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HospitalTimeModel> Select()
        {
            var hospitalTimeList = new ObservableCollection<HospitalTimeModel>();

            var cmd = new SqlCommand("GET_HOSPITAL_TIME");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var hospitalTimeModel = new HospitalTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString())
                    };

                    hospitalTimeList.Add(hospitalTimeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return hospitalTimeList;
        }

        /// <summary>
        /// Обновить запись о больничном
        /// </summary>
        /// <param name="hospitalTimeModel">больничный</param>
        /// <returns></returns>
        public bool Update(HospitalTimeModel hospitalTimeModel)
        {
            if (hospitalTimeModel == null) throw new ArgumentNullException(nameof(hospitalTimeModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE HOSPITAL_TIME SET EMPLOYEE_ID={hospitalTimeModel.EmployeeId}, DESCRIPTION='{hospitalTimeModel.Description}', START_HOSPITAL_TIME='{hospitalTimeModel.StartDate}', FINISH_HOSPITAL_TIME='{hospitalTimeModel.FinishDate}' WHERE ID={hospitalTimeModel.Id};");

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
        /// Удалить больничный по id
        /// </summary>
        /// <param name="id">id больничного</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM HOSPITAL_TIME WHERE ID = '{id}'");
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
        /// Возвращает список больничных по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<HospitalTimeModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var hospitalTimeList = new ObservableCollection<HospitalTimeModel>();

            var cmd = new SqlCommand($"GET_HOSPITAL_TIME {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var hospitalTime = new HospitalTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString())
                    };

                    hospitalTimeList.Add(hospitalTime);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return hospitalTimeList;
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