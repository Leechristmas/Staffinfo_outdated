using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    public class EducationTimeTableProvider: ITableProvider, IDisposable
    {
        public string ErrorInfo { get; set; }
     
        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel eductionTime)
        {
            if (eductionTime == null) throw new ArgumentNullException(nameof(eductionTime), Resources.DatabaseConnector_parameter_cannot_be_null);

            var educationTimeModel = eductionTime as EducationTimeModel;

            var cmd =
                new SqlCommand($@"INSERT INTO EDUCATION_TIME VALUES({educationTimeModel.EmployeeId}, '{educationTimeModel.StartDate}', '{educationTimeModel.FinishDate}', {educationTimeModel.SpecialityId}, {educationTimeModel.InstitutionId}, '{educationTimeModel.Description}'); SELECT MAX(ID) FROM EDUCATION_TIME;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                educationTimeModel.Id = Int64.Parse(sqlDataReader[0].ToString());
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
        
        public BaseModel GetElementById(long? id)
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

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var educationTimeList = new ObservableCollection<BaseModel>();

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

        public bool Update(BaseModel educationTime)
        {
            if (educationTime == null) throw new ArgumentNullException(nameof(educationTime), Resources.DatabaseConnector_parameter_cannot_be_null);

            var educationTimeModel = educationTime as EducationTimeModel;

            var cmd = new SqlCommand($@"UPDATE EDUCATION_TIME SET EMPLOYEE_ID={educationTimeModel.EmployeeId}, START_DATE='{educationTimeModel.StartDate}', FINISH_DATE='{educationTimeModel.FinishDate}', SPECIALITY_ID={educationTimeModel.SpecialityId}, INSTITUTION_ID={educationTimeModel.InstitutionId}, DESCRIPTION='{educationTimeModel.Description}' WHERE ID={educationTimeModel.Id};");

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
