using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Staffinfo.Desktop.Data;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data.DataTableProviders;
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
            EmployeeList = new ObservableCollectionViewModel<EmployeeViewModel>(DataSingleton.Instance.EmployeeList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Служащие
        /// </summary>
        public ObservableCollectionViewModel<EmployeeViewModel> EmployeeList { get; set; }

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
            var addNewEmployeeView = new AddNewEmployeeView {DataContext = new AddNewEmployeeViewModel()};
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

        /// <summary>
        /// Удалить служащего
        /// </summary>
        private RelayCommand _removeEmployee;

        public RelayCommand RemoveEmployee
            => _removeEmployee ?? (_removeEmployee = new RelayCommand(RemoveEmployeeExecute));

        public void RemoveEmployeeExecute()
        {
            var remove = MessageBox.Show("Будет удалена вся ниформация о служащем. Вы уверены?", "Удаление", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);

            if (remove == MessageBoxResult.No) return;

            //var index = EmployeeList.SelectedIndex;
            var item = EmployeeList.SelectedItem;//ModelCollection[index];
            
            using (var prvdr = new EmployeeTableProvider())
            {
                if (!prvdr.DeleteById(item.Id))
                {
                    MessageBox.Show("Ошибка удаления!" + prvdr.ErrorInfo, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                EmployeeList.ModelCollection.Remove(item);
            }
        }
        #endregion
    }
}
