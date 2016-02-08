using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeTableProvider: IDisposable, ITableProvider
    {
        public string ErrorInfo { get; private set; }

        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);

            var employeeModel = employee as EmployeeModel;

            var cmd =
                new SqlCommand($"INSERT INTO EMPLOYEE VALUES('{employeeModel.FirstName}', '{employeeModel.MiddleName}', '{employeeModel.LastName}'," + 
                $"'{employeeModel.PersonalNumber}', {employeeModel.PostId}, {employeeModel.RankId}, '{employeeModel.BornDate.Value}'," + 
                $"'{employeeModel.JobStartDate.Value}', '{employeeModel.Address}', '{employeeModel.Pasport}', '{employeeModel.MobilePhoneNumber}', '{employeeModel.HomePhoneNumber}', '{employeeModel.IsPensioner}'); "+
                "SELECT MAX(ID) FROM EMPLOYEE;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                employeeModel.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return employeeModel;
        }

        public BaseModel GetElementById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM EMPLOYEE WHERE ID={id};");

            EmployeeModel employeeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                employeeModel = new EmployeeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    FirstName = sqlDataReader["EMPLOYEE_FIRSTNAME"].ToString(),
                    LastName = sqlDataReader["EMPLOYEE_LASTNAME"].ToString(),
                    MiddleName = sqlDataReader["EMPLOYEE_MIDDLENAME"].ToString(),
                    PersonalNumber = sqlDataReader["PERSONAL_KEY"].ToString(),
                    PostId = Int64.Parse(sqlDataReader["POST_ID"].ToString()),
                    RankId = Int64.Parse(sqlDataReader["RANK_ID"].ToString()),
                    BornDate = DateTime.Parse(sqlDataReader["BORN_DATE"].ToString()),
                    JobStartDate = DateTime.Parse(sqlDataReader["JOB_START_DATE"].ToString()),
                    Address = sqlDataReader["ADDRESS"].ToString(),
                    Pasport = sqlDataReader["PASPORT"].ToString(),
                    MobilePhoneNumber = sqlDataReader["MOBILE_PHONE_NUMBER"].ToString(),
                    HomePhoneNumber = sqlDataReader["HOME_PHONE_NUMBER"].ToString(),
                    IsPensioner = bool.Parse(sqlDataReader["IS_PENSIONER"].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return employeeModel;
        }

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var employeeList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM EMPLOYEE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                

                while (sqlDataReader.Read())
                {
                    var employeeModel = new EmployeeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        FirstName = sqlDataReader["EMPLOYEE_FIRSTNAME"].ToString(),
                        LastName = sqlDataReader["EMPLOYEE_LASTNAME"].ToString(),
                        MiddleName = sqlDataReader["EMPLOYEE_MIDDLENAME"].ToString(),
                        PersonalNumber = sqlDataReader["PERSONAL_KEY"].ToString(),
                        PostId = Int64.Parse(sqlDataReader["POST_ID"].ToString()),
                        RankId = Int64.Parse(sqlDataReader["RANK_ID"].ToString()),
                        BornDate = DateTime.Parse(sqlDataReader["BORN_DATE"].ToString()),
                        JobStartDate = DateTime.Parse(sqlDataReader["JOB_START_DATE"].ToString()),
                        Address = sqlDataReader["ADDRESS"].ToString(),
                        Pasport = sqlDataReader["PASPORT"].ToString(),
                        MobilePhoneNumber = sqlDataReader["MOBILE_PHONE_NUMBER"].ToString(),
                        HomePhoneNumber = sqlDataReader["HOME_PHONE_NUMBER"].ToString(),
                        IsPensioner = bool.Parse(sqlDataReader["IS_PENSIONER"].ToString())
                    };

                    employeeList.Add(employeeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return employeeList;
        }

        public bool Update(BaseModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);

            var employeeModel = employee as EmployeeModel;

            var cmd = new SqlCommand($@"UPDATE EMPLOYEE SET EMPLOYEE_FIRSTNAME='{employeeModel.FirstName}', EMPLOYEE_MIDDLENAME='{employeeModel.MiddleName}'," + 
                $"EMPLOYEE_LASTNAME='{employeeModel.LastName}', PERSONAL_KEY='{employeeModel.PersonalNumber}', POST_ID={employeeModel.PostId}," +
                $"RANK_ID={employeeModel.RankId}, BORN_DATE='{employeeModel.BornDate.Value}', JOB_START_DATE='{employeeModel.JobStartDate.Value}'," +
                $"ADDRESS='{employeeModel.Address}', PASPORT='{employeeModel.Pasport}', MOBILE_PHONE_NUMBER='{employeeModel.MobilePhoneNumber}'," +
                $"HOME_PHONE_NUMBER='{employeeModel.HomePhoneNumber}', IS_PENSIONER='{employeeModel.IsPensioner}' " +
                $"WHERE ID={employeeModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM EMPLOYEE WHERE ID = '{id}'");
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
