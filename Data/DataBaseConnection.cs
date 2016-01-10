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
        private const string _urlDataBase = "";

        private readonly SqlConnection _sqlConnection; 

        public DataBaseConnection()
        {
            _sqlConnection = new SqlConnection(_urlDataBase);
            _sqlConnection.Open();
        }        
    }
}
