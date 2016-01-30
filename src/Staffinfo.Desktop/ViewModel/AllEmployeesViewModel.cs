using System.Collections.ObjectModel;
using Staffinfo.Desktop.Data;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для окна со списком служащих
    /// </summary>
    public class AllEmployeesViewModel: WindowViewModelBase
    {
        #region Constructors

        public AllEmployeesViewModel()
        {
            EmployeeList = DataSingleton.Instance.EmployeeList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Служащие
        /// </summary>
        public ObservableCollection<EmployeeViewModel> EmployeeList { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Открыть окно добавления служащего
        /// </summary>
        private RelayCommand _goToAddingNewEmployee;
        public RelayCommand GoToAddingNewEmployee
            => _goToAddingNewEmployee ?? (_goToAddingNewEmployee = new RelayCommand(GoToAddingNewEmployeeExecute));

        public void GoToAddingNewEmployeeExecute()
        {
            var addNewEmployeeView = new AddNewEmployeeView();
            addNewEmployeeView.DataContext = new AddNewEmployeeViewModel();
            addNewEmployeeView.ShowDialog();
        }

        /// <summary>
        /// Закрыть окно
        /// </summary>
        private RelayCommand _closeWindowCommand;

        public RelayCommand CloseWindowCommand
            => _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindowCommandExecute));

        public void CloseWindowCommandExecute()
        {
            WindowsClosed = true;
        }
        #endregion
    }
}
