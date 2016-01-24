using System.Collections.ObjectModel;
using Staffinfo.Desktop.ViewModel;

namespace Staffinfo.Desktop.Data
{
    public class DataSingleton
    {
        private static DataSingleton _instance;
        private DatabaseConnector _databaseConnector;

        private DataSingleton()
        {
            _databaseConnector = new DatabaseConnector();
        }

        public static DataSingleton Instance => _instance ?? (_instance = new DataSingleton());

        public DatabaseConnector DatabaseConnector
        {
            get { return _databaseConnector; }
        }

        #region EmployeeList
        private ObservableCollection<EmployeeViewModel> _employeeList;

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get { return _employeeList ?? (_employeeList = new ObservableCollection<EmployeeViewModel>()); }

        }
        #endregion
        
    }
}
