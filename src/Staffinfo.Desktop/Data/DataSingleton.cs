using System.Collections.ObjectModel;
using Staffinfo.Desktop.ViewModel;

namespace Staffinfo.Desktop.Data
{
    public class DataSingleton
    {
        private static DataSingleton _instance;

        private DataSingleton()
        {

        }

        public static DataSingleton Instance => _instance ?? (_instance = new DataSingleton());

        #region EmployeeList
        private ObservableCollection<EmployeeViewModel> _employeeList;

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get { return _employeeList ?? (_employeeList = new ObservableCollection<EmployeeViewModel>()); }

        }
        #endregion
        
    }
}
