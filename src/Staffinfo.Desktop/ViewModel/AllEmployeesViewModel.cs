using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Staffinfo.Desktop.Data;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;
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
            //инициализируем уровень доступа пользователя к view
            AccessLevel = DataSingleton.Instance.User.AccessLevel;

            //инициализируем список сотрудников
            Employees = DataSingleton.Instance.EmployeeList;

            CanDelete = Employees?.Count > 0;

            Ranks = DataSingleton.Instance.RankList.Select(p => new RankViewModel(p)).ToList();
        }
        
        #region Fields

        /// <summary>
        /// Служащие
        /// </summary>
        private ObservableCollection<EmployeeViewModel> _employees;

        /// <summary>
        /// текст поиска
        /// </summary>
        private string _searchText = String.Empty;

        /// <summary>
        /// выбранный служащий
        /// </summary>
        private EmployeeViewModel _selectedEmployee;

        /// <summary>
        /// Ширина бокового меню
        /// </summary>
        private int _menuWidth = 150;

        /// <summary>
        /// Текущий таб (фильтрация/раб. область)
        /// </summary>
        private int _actualTab;

        /// <summary>
        /// Enable кнопки фильтрации
        /// </summary>
        private Visibility _filtrationBtnVisibility;

        /// <summary>
        /// Выброанные звания
        /// </summary>
        private List<RankModel> _selectedRanks;

        /// <summary>
        /// Разрешить удаление
        /// </summary>
        private bool _canDelete;

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

        /// <summary>
        /// Текущий таб (фильтрация/раб. область)
        /// </summary>
        public int ActualTab
        {
            get { return _actualTab; }
            set
            {
                _actualTab = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Разрешить удаление
        /// </summary>
        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                _canDelete = value;
                RaisePropertyChanged(nameof(CanDelete));
                RaisePropertyChanged(nameof(RemoveEmployee));
            }
        }

        /// <summary>
        /// Enable фильтрации
        /// </summary>
        public Visibility FiltrationBtnVisibility
        {
            get { return _filtrationBtnVisibility; }
            set
            {
                _filtrationBtnVisibility = value;
                RaisePropertyChanged();
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
            get { return _employees; }
            set
            {
                _employees = value;
                RaisePropertyChanged("Employees");
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
            get { return _searchText; }
            set
            {
                //блокируем интерфейс
                ViewIsEnable = false;

                _searchText = value;
                RaisePropertyChanged("SearchText");

                //селекция сотрудников согласно введенному тексту
                Employees = new ObservableCollection<EmployeeViewModel>(DataSingleton.Instance.EmployeeList.Where(e => e.LastName.ToLower().StartsWith(SearchText.ToLower())));

                CanDelete = Employees.Count > 0;

                //открываем интерфейс
                ViewIsEnable = true;
            }
        }

        /// <summary>
        /// Звания
        /// </summary>
        public List<RankViewModel> Ranks { get; set; }

        /// <summary>
        /// Выбранные звания
        /// </summary>
        public List<RankModel> SelectedRanks
        {
            get { return _selectedRanks; }
            set
            {
                _selectedRanks = value;
                RaisePropertyChanged();
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
            //CanDelete = Employees.Count > 0;
        }
        
        /// <summary>
        /// Удалить служащего
        /// </summary>
        private RelayCommand _removeEmployee;
        public RelayCommand RemoveEmployee
            => _removeEmployee ?? (_removeEmployee = new RelayCommand(RemoveEmployeeExecute, () => CanDelete));

        private void RemoveEmployeeExecute()
        {
            ViewIsEnable = false;

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

            CanDelete = Employees.Count > 0;

            ViewIsEnable = true;

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

        /// <summary>
        /// Переходим к фильтрации и обратно
        /// </summary>
        private RelayCommand _toFilterView;
        public RelayCommand ToFilterView => _toFilterView ?? (_toFilterView = new RelayCommand(ToFilterViewExecute));

        private void ToFilterViewExecute()
        {
            if (ActualTab == 0)
            {
                ActualTab = 1;
                FiltrationBtnVisibility = Visibility.Collapsed;
            }
            else
            {
                ActualTab = 0;
                FiltrationBtnVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private RelayCommand _acceptFiltration;
        public RelayCommand AcceptFiltration
            => _acceptFiltration ?? (_acceptFiltration = new RelayCommand(AcceptFiltrationExecute));

        private void AcceptFiltrationExecute()
        {
            var t = Ranks.Where(p => p.IsSelected);
            var t2 = t.Count();
        }

        #endregion
    }
}
