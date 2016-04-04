using System;
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
        public AllEmployeesViewModel()
        {
            AccessLevel = DataSingleton.Instance.User.AccessLevel;
        }

        #region Fields

        private ObservableCollectionViewModel<EmployeeViewModel> _employeeList =
            new ObservableCollectionViewModel<EmployeeViewModel>(DataSingleton.Instance.EmployeeList);

        /// <summary>
        /// текст поиска
        /// </summary>
        private string _searchText = String.Empty;

        /// <summary>
        /// выбранные служащий
        /// </summary>
        private EmployeeViewModel _selectedEmployee;

        /// <summary>
        /// Ширина бокового меню
        /// </summary>
        private int _menuWidth = 150;

        ///// <summary>
        ///// Ширина рабочей области
        ///// </summary>
        //private int _dataWidth;

        ///// <summary>
        ///// Ширина окна
        ///// </summary>
        //private int _windowWidth;


        #endregion

        #region Properties

        /// <summary>
        /// Ширина бокового меню
        /// </summary>
        public int MenuWidth
        {
            get { return _menuWidth; }
            set
            {
                _menuWidth = value;
                RaisePropertyChanged(nameof(MenuWidth));
            }
        }

        ///// <summary>
        ///// Ширина бокового меню
        ///// </summary>
        //public int DataWidth
        //{
        //    get { return _dataWidth; }
        //    set
        //    {
        //        _dataWidth = value;
        //        RaisePropertyChanged(nameof(DataWidth));
        //    }
        //}

        ///// <summary>
        ///// Ширина бокового меню
        ///// </summary>
        //public int WindowWidth
        //{
        //    get { return _windowWidth; }
        //    set
        //    {
        //        _windowWidth = value;
        //        RaisePropertyChanged(nameof(WindowWidth));
        //    }
        //}

        /// <summary>
        /// Служащие
        /// </summary>
        public ObservableCollection<EmployeeViewModel> Employees
        {
            get
            {
                return new ObservableCollection<EmployeeViewModel>(DataSingleton.Instance.EmployeeList.Where(e => e.LastName.ToLower().StartsWith(SearchText)));
            }
        }

        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public EmployeeViewModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value; 
                RaisePropertyChanged(nameof(SelectedEmployee));
            }
        }

        /// <summary>
        /// Текст из строки поиска
        /// </summary>
        public string SearchText
        {
            get { return _searchText.ToLower(); }
            set
            {
                ViewIsEnable = false;
                _searchText = value;
                
                RaisePropertyChanged("SearchText");
                RaisePropertyChanged("Employees");
                ViewIsEnable = true;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Открыть окно добавления служащего
        /// </summary>
        private RelayCommand _goToAddingNewEmployee;
        public RelayCommand GoToAddingNewEmployee
            => _goToAddingNewEmployee ?? (_goToAddingNewEmployee = new RelayCommand(GoToAddingNewEmployeeExecute));

        private void GoToAddingNewEmployeeExecute()
        {
            var addNewEmployeeView = new AddNewEmployeeView {DataContext = new AddNewEmployeeViewModel()};
            addNewEmployeeView.ShowDialog();
        }
        
        /// <summary>
        /// Удалить служащего
        /// </summary>
        private RelayCommand _removeEmployee;
        public RelayCommand RemoveEmployee
            => _removeEmployee ?? (_removeEmployee = new RelayCommand(RemoveEmployeeExecute));

        private void RemoveEmployeeExecute()
        {
            var item = SelectedEmployee;
            if (item == null)
            {
                MessageBox.Show("Запись не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.OK);
                return;
            }

            var remove = MessageBox.Show("Будет удалена вся ниформация о служащем. Вы уверены?", "Удаление", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);

            if (remove == MessageBoxResult.No) return;
            
            using (var prvdr = new EmployeeTableProvider())
            {
                if (!prvdr.DeleteById(item.Id))
                {
                    MessageBox.Show("Ошибка удаления!" + prvdr.ErrorInfo, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Employees.Remove(item);
            }
        }

        /// <summary>
        /// Открыть окно с информацией по выбранному служащему
        /// </summary>
        private RelayCommand _showEmployee;
        public RelayCommand ShowEmployee => _showEmployee ?? (_showEmployee = new RelayCommand(ShowEmployeeExecute));

        private void ShowEmployeeExecute()
        {
            var employeeView = new EmployeeView {DataContext = new EmployeeEditViewModel(SelectedEmployee) };
            employeeView.ShowDialog();
        }

        /// <summary>
        /// Свернуть/развернуть ширину бокового меню
        /// </summary>
        private RelayCommand _toggleMenuWidth;
        public RelayCommand ToggleMenuWidth
            => _toggleMenuWidth ?? (_toggleMenuWidth = new RelayCommand(ToggleMenuWidthExecute));

        private void ToggleMenuWidthExecute()
        {
            MenuWidth = MenuWidth == 150 ? 35 : 150;
        }

        #endregion
    }
}
