using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице HOOLIDAY_TIME
    /// </summary>
    public class HolidayTimeTableProvider: IWritableDirectoryTableContract<HolidayTimeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись об отпуске
        /// </summary>
        /// <param name="holidayTime">отпуск</param>
        /// <returns></returns>
        public HolidayTimeModel Save(HolidayTimeModel holidayTime)
        {
            if (holidayTime == null) throw new ArgumentNullException(nameof(holidayTime), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO HOLIDAY_TIME VALUES({holidayTime.EmployeeId}, '{holidayTime.Description}', '{holidayTime.StartDate}', '{holidayTime.FinishDate}'); SELECT MAX(ID) FROM HOLIDAY_TIME;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                holidayTime.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return holidayTime;
        }

        /// <summary>
        /// Возвращает отпуск по id
        /// </summary>
        /// <param name="id">id отпуска</param>
        /// <returns></returns>
        public HolidayTimeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM HOLIDAY_TIME WHERE ID={id};");

            HolidayTimeModel holidayTimeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                holidayTimeModel = new HolidayTimeModel
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

            return holidayTimeModel;
        }

        /// <summary>
        /// Возвращает список отпусков
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HolidayTimeModel> Select()
        {
            var holidayTimeList = new ObservableCollection<HolidayTimeModel>();

            var cmd = new SqlCommand("SELECT * FROM HOLIDAY_TIME");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var holidayTimeModel = new HolidayTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString())
                    };

                    holidayTimeList.Add(holidayTimeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return holidayTimeList;
        }

        /// <summary>
        /// Обновить запись об отпуске
        /// </summary>
        /// <param name="holidayTimeModel">отпуск</param>
        /// <returns></returns>
        public bool Update(HolidayTimeModel holidayTimeModel)
        {
            if (holidayTimeModel == null) throw new ArgumentNullException(nameof(holidayTimeModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE HOLIDAY_TIME SET EMPLOYEE_ID={holidayTimeModel.EmployeeId}, DESCRIPTION='{holidayTimeModel.Description}', START_HOLIDAY_DATE='{holidayTimeModel.StartDate}', FINISH_HOLIDAY_DATE='{holidayTimeModel.FinishDate}' WHERE ID={holidayTimeModel.Id};");

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
        /// Удалить отпуск по id
        /// </summary>
        /// <param name="id">id отпуска</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM HOLIDAY_TIME WHERE ID = '{id}'");
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
        /// Возвращает список отпусков по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<HolidayTimeModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var holidayTimeList = new ObservableCollection<HolidayTimeModel>();

            var cmd = new SqlCommand($"SELECT * FROM HOLIDAY_TIME WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var holidayTime = new HolidayTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        StartDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[4].ToString())
                    };

                    holidayTimeList.Add(holidayTime);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return holidayTimeList;
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