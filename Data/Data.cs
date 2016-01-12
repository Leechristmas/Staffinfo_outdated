using Staffinfo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staffinfo.ViewModel;

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

        private ObservableCollection<EmployeeViewModel> _employeeList;

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get { return _employeeList ?? (_employeeList = new ObservableCollection<EmployeeViewModel>()); }
        }
    }
}
