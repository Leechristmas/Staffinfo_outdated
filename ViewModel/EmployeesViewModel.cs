using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Staffinfo.Model;

namespace Staffinfo.ViewModel
{
    public class EmployeesViewModel: ViewModelBase
    {
        public EmployeesViewModel()
        {
            var data = Data.Data.Instance.DataBaseConnection;
            data.GetEmployee();
            EmployeeList = Data.Data.Instance.EmployeeList;
        }

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get; set;
        }

    }
}
