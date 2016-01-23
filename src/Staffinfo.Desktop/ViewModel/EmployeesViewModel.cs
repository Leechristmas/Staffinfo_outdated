using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    public class EmployeesViewModel: ViewModelBase
    {
        public EmployeesViewModel()
        {
            //var data = Data.Data.Instance.DataBaseConnection;
            //data.GetEmployee();
            //EmployeeList = Data.Data.Instance.EmployeeList;
        }

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get; set;
        }

    }
}
