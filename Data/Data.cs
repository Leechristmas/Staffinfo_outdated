using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Data
{
    public class Data
    {
        
        private Data()
        {

        }

        private static Data _instance;

        public static Data Instance
        {
            get { return _instance ?? (_instance = new Data()); }
        }

        private DataBaseConnection _dataBaseConnection;

        public DataBaseConnection DataBaseConnection
        {
            get { return _dataBaseConnection ?? (_dataBaseConnection = new DataBaseConnection()); }
        } 


    }
}
