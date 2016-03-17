using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице EducationTime
    /// </summary>
    public class EducationTimeTableProvider: IWritableDirectoryTableContract<EducationTimeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить обучение в БД
        /// </summary>
        /// <param name="eductionTime">процесс обучения</param>
        /// <returns></returns>
        public EducationTimeModel Save(EducationTimeModel eductionTime)
        {
            if (eductionTime == null) throw new ArgumentNullException(nameof(eductionTime), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($@"INSERT INTO EDUCATION_TIME VALUES({eductionTime.EmployeeId}, '{eductionTime.StartDate}', '{eductionTime.FinishDate}', {eductionTime.SpecialityId}, {eductionTime.InstitutionId}, '{eductionTime.Description}'); SELECT MAX(ID) FROM EDUCATION_TIME;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                eductionTime.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return eductionTime;
        }
        
        /// <summary>
        /// Возвращает процесс обучения по id
        /// </summary>
        /// <param name="id">id процесса обучения</param>
        /// <returns></returns>
        public EducationTimeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM EDUCATION_TIME WHERE ID={id};");

            EducationTimeModel educationTimeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                educationTimeModel = new EducationTimeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    StartDate = DateTime.Parse(sqlDataReader[2].ToString()),
                    FinishDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    SpecialityId = Int64.Parse(sqlDataReader[4].ToString()),
                    InstitutionId = Int64.Parse(sqlDataReader[5].ToString()),
                    Description = sqlDataReader[6].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return educationTimeModel;
        }

        /// <summary>
        /// Возвращает все процессы обучения
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EducationTimeModel> Select()
        {
            var educationTimeList = new ObservableCollection<EducationTimeModel>();

            var cmd = new SqlCommand("SELECT * FROM EDUCATION_TIME");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var educationTimeModel = new EducationTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        StartDate = DateTime.Parse(sqlDataReader[2].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        SpecialityId = Int64.Parse(sqlDataReader[4].ToString()),
                        InstitutionId = Int64.Parse(sqlDataReader[5].ToString()),
                        Description = sqlDataReader[6].ToString()
                    };

                    educationTimeList.Add(educationTimeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return educationTimeList;
        }

        /// <summary>
        /// Обновляет процесс обучения
        /// </summary>
        /// <param name="educationTime">процесс обучения</param>
        /// <returns></returns>
        public bool Update(EducationTimeModel educationTime)
        {
            if (educationTime == null) throw new ArgumentNullException(nameof(educationTime), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE EDUCATION_TIME SET EMPLOYEE_ID={educationTime.EmployeeId}, START_DATE='{educationTime.StartDate}', FINISH_DATE='{educationTime.FinishDate}', SPECIALITY_ID={educationTime.SpecialityId}, INSTITUTION_ID={educationTime.InstitutionId}, DESCRIPTION='{educationTime.Description}' WHERE ID={educationTime.Id};");

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
        /// Удалить процесс обучения по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM EDUCATION_TIME WHERE ID = '{id}'");
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
        /// Возвращает список периодов обучения по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<EducationTimeModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var educationTimeList = new ObservableCollection<EducationTimeModel>();

            var cmd = new SqlCommand($"SELECT * FROM EDUCATION_TIME WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var educationTimeModel = new EducationTimeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        StartDate = DateTime.Parse(sqlDataReader[2].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        SpecialityId = Int64.Parse(sqlDataReader[4].ToString()),
                        InstitutionId = Int64.Parse(sqlDataReader[5].ToString()),
                        Description = sqlDataReader[6].ToString()
                    };

                    educationTimeList.Add(educationTimeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return educationTimeList;
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
