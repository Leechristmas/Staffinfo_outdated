using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Staffinfo.Desktop.Data;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    public class AllEmployeesViewModel: ViewModelBase
    {
        public AllEmployeesViewModel()
        {
            EmployeeList = DataSingleton.Instance.EmployeeList;
        }

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get; set;
        }

        private RelayCommand _goToAddingNewEmployee;

        public RelayCommand GoToAddingNewEmployee => _goToAddingNewEmployee ?? (_goToAddingNewEmployee = new RelayCommand(GoToAddingNewEmployeeExecute));

        /// <summary>
        /// Открыть окно добавления служащего
        /// </summary>
        public void GoToAddingNewEmployeeExecute()
        {
            var addNewEmployeeView = new AddNewEmployeeView();
            addNewEmployeeView.DataContext = new AddNewEmployeeViewModel();
            addNewEmployeeView.ShowDialog();
        }
    }
}
