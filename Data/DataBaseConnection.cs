using Staffinfo.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Data
{
    public class DataBaseConnection
    {
        private const string _urlDataBase = @"Data Source=DESKTOP-QV3R6NU\SQLEXPRESS; Initial Catalog = staffinfo; Integrated Security=SSPI;";
        private readonly SqlConnection _sqlConnection; 

        public DataBaseConnection()
        {
            _sqlConnection = new SqlConnection(_urlDataBase);
            _sqlConnection.Open();
        }        

        /// <summary>
        /// Возвращает набор полей, описывающих служащего (типо модели служащего=))
        /// </summary>
        public void GetEmployee()
        {
            var cmd = new SqlCommand("SELECT * FROM TB_EMPLOYEE", _sqlConnection);
            
            SqlDataReader reader = cmd.ExecuteReader();
                        
            while (reader.Read())
            {
                ((List<EmployeeModel>)Data.Instance.EmployeeList).Add(new EmployeeModel(
                    Int64.Parse(reader["ID"].ToString()),
                    reader["EMPLOYEE_FIRSTNAME"].ToString(),
                    reader["EMPLOYEE_MIDDLENAME"].ToString(),
                    reader["EMPLOYEE_LASTNAME"].ToString(),
                    reader["PERSONAL_KEY"].ToString(),
                    Int64.Parse(reader["POST_ID"].ToString()),
                    Int64.Parse(reader["RANK_ID"].ToString()),
                    DateTime.Parse(reader["BORN_DATE"].ToString()),
                    DateTime.Parse(reader["JOB_START_DATE"].ToString()),
                    reader["ADDRESS"].ToString(),
                    reader["PASPORT"].ToString(),
                    reader["PHONE_NUMBER"].ToString()));
            }
        }

    }
}
