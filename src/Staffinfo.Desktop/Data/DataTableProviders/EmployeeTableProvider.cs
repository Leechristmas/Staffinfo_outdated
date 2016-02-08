using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент для доступа к таблице Employee
    /// </summary>
    public class EmployeeTableProvider: IDisposable, IWritableTableContract<EmployeeModel>
    {
        public string ErrorInfo { get; set; }

        #region IWritableTableContract implementation

        /// <summary>
        /// Сохраняет служащего в БД
        /// </summary>
        /// <param name="employee">Служащий</param>
        /// <returns></returns>
        public EmployeeModel Save(EmployeeModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($"INSERT INTO EMPLOYEE VALUES('{employee.FirstName}', '{employee.MiddleName}', '{employee.LastName}'," + 
                $"'{employee.PersonalNumber}', {employee.PostId}, {employee.RankId}, '{employee.BornDate.Value}'," + 
                $"'{employee.JobStartDate.Value}', '{employee.Address}', '{employee.Pasport}', '{employee.MobilePhoneNumber}', '{employee.HomePhoneNumber}', '{employee.IsPensioner}'); "+
                "SELECT MAX(ID) FROM EMPLOYEE;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                employee.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return employee;
        }

        /// <summary>
        /// Возвращает служащего по id
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public EmployeeModel Select(long? id)
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

        /// <summary>
        /// Возвращает всех служащих
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EmployeeModel> Select()
        {
            var employeeList = new ObservableCollection<EmployeeModel>();

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

        /// <summary>
        /// Обновляет служащего
        /// </summary>
        /// <param name="employee">Служащий</param>
        /// <returns></returns>
        public bool Update(EmployeeModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE EMPLOYEE SET EMPLOYEE_FIRSTNAME='{employee.FirstName}', EMPLOYEE_MIDDLENAME='{employee.MiddleName}'," + 
                $"EMPLOYEE_LASTNAME='{employee.LastName}', PERSONAL_KEY='{employee.PersonalNumber}', POST_ID={employee.PostId}," +
                $"RANK_ID={employee.RankId}, BORN_DATE='{employee.BornDate.Value}', JOB_START_DATE='{employee.JobStartDate.Value}'," +
                $"ADDRESS='{employee.Address}', PASPORT='{employee.Pasport}', MOBILE_PHONE_NUMBER='{employee.MobilePhoneNumber}'," +
                $"HOME_PHONE_NUMBER='{employee.HomePhoneNumber}', IS_PENSIONER='{employee.IsPensioner}' " +
                $"WHERE ID={employee.Id};");

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
        /// Удалить служащего по id
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
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
