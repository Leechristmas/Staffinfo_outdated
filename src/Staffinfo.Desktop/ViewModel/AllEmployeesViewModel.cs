using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    public class AllEmployeesViewModel: ViewModelBase
    {
        public AllEmployeesViewModel()
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
