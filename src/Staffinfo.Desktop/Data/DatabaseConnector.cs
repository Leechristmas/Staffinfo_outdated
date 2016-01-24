using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.ViewModel;

namespace Staffinfo.Desktop.Data
{
    public class DatabaseConnector
    {
        private const string UrlDataBase = @"Data Source=DESKTOP-QV3R6NU\SQLEXPRESS; Initial Catalog = staffinfo; Integrated Security=SSPI;";
        private readonly SqlConnection _sqlConnection;

        public DatabaseConnector()
        {
            _sqlConnection = new SqlConnection(UrlDataBase);
            _sqlConnection.Open();
        }

        /// <summary>
        /// Возвращает список view models для служащих из БД
        /// </summary>
        public ObservableCollection<EmployeeViewModel> GetEmployeeViewModels()
        {
            var cmd = new SqlCommand("SELECT * FROM EMPLOYEE", _sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            var employeeViewModels = new ObservableCollection<EmployeeViewModel>();

            while (reader.Read())
            {
                //var employeeModel = new EmployeeModel(
                //    Int64.Parse(reader["ID"].ToString()),
                //    reader["EMPLOYEE_FIRSTNAME"].ToString(),
                //    reader["EMPLOYEE_MIDDLENAME"].ToString(),
                //    reader["EMPLOYEE_LASTNAME"].ToString(),
                //    reader["PERSONAL_KEY"].ToString(),
                //    Int64.Parse(reader["POST_ID"].ToString()),
                //    Int64.Parse(reader["RANK_ID"].ToString()),
                //    DateTime.Parse(reader["BORN_DATE"].ToString()),
                //    DateTime.Parse(reader["JOB_START_DATE"].ToString()),
                //    reader["ADDRESS"].ToString(),
                //    reader["PASPORT"].ToString(),
                //    reader["PHONE_NUMBER"].ToString());

                //employeeViewModels.Add(new EmployeeViewModel(employeeModel));
            }

            return employeeViewModels;
        }

    }
}