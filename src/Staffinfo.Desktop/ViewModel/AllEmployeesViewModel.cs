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
    public class AllEmployeesViewModel : WindowViewModelBase
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
        private int _menuWidth = 35;

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

        /// <summary>
        /// Текущий таб функционального меню
        /// </summary>
        private int _menuTabIndex;

        /// <summary>
        /// Параметр сортировки
        /// </summary>
        private string _selectedSortParameter;

        /// <summary>
        /// Тип сортировки
        /// </summary>
        private string _selectedSortType;

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
        /// Текущий таб функционального меню
        /// </summary>    
        public int MenuTabIndex
        {
            get { return _menuTabIndex; }
            set
            {
                _menuTabIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MenuHeader));
            }
        }

        /// <summary>
        /// Заголовок функционального меню
        /// </summary>
        public string MenuHeader
        {
            get
            {
                if (MenuTabIndex == 0) return "Меню";
                if (MenuTabIndex == 1) return "Сортировка";
                return "Фильтрация";
            }
        }

        /// <summary>
        /// Выбранный параметр сортировки
        /// </summary>
        public string SelectedSortParameter
        {
            get { return _selectedSortParameter; }
            set
            {
                _selectedSortParameter = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текст ошибки при неудачной сортировке
        /// </summary>
        public string SortErrorText
        {
            get { return _sortErrorText; }
            set
            {
                _sortErrorText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный тип сортировки
        /// </summary>
        public string SelectedSortType
        {
            get { return _selectedSortType; }
            set
            {
                _selectedSortType = value;
                RaisePropertyChanged();
            }
        }

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
        /// Перейти к сортировке
        /// </summary>
        private RelayCommand _goToSortView;
        public RelayCommand GoToSortView => _goToSortView ?? (_goToSortView = new RelayCommand(GoToSortViewExecute));

        private void GoToSortViewExecute()
        {
            MenuTabIndex = 1;
            if (MenuWidth == 35) MenuWidth = 200;
        }

        /// <summary>
        /// Перейти к функциональному боковому меню
        /// </summary>
        private RelayCommand _goToMainMenu;
        public RelayCommand GoToMainMenu => _goToMainMenu ?? (_goToMainMenu = new RelayCommand(GoToMainMenuExecute));

        private void GoToMainMenuExecute()
        {
            MenuTabIndex = 0;
        }

        /// <summary>
        /// Открыть окно добавления служащего
        /// </summary>
        private RelayCommand _goToAddingNewEmployee;
        public RelayCommand GoToAddingNewEmployee
            => _goToAddingNewEmployee ?? (_goToAddingNewEmployee = new RelayCommand(GoToAddingNewEmployeeExecute));

        private void GoToAddingNewEmployeeExecute()
        {
            var addNewEmployeeView = new AddNewEmployeeView { DataContext = new AddNewEmployeeViewModel() };
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
            var employeeView = new EmployeeView { DataContext = new EmployeeEditViewModel(SelectedEmployee) };
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
            MenuWidth = MenuWidth == 200 ? 35 : 200;
            if (MenuTabIndex != 0)
            {
                MenuTabIndex = 0;
                SetDefaultSortRequisites();
            }
        }

        /// <summary>
        /// Переходим к фильтрации
        /// </summary>
        private RelayCommand _toFilterView;
        public RelayCommand ToFilterView => _toFilterView ?? (_toFilterView = new RelayCommand(ToFilterViewExecute));

        private void ToFilterViewExecute()
        {
            
        }

        /// <summary>
        /// Применить сортировку
        /// </summary>
        private RelayCommand _applySort;
        public RelayCommand ApplySort => _applySort ?? (_applySort = new RelayCommand(ApplySortExecute));

        private void ApplySortExecute()
        {
            SortErrorText = null;

            if (SelectedSortParameter == null ||
                SelectedSortType == null)
            {
                SortErrorText = "Не все параметры указаны";
                return;
            }

            switch (SelectedSortParameter)
            {
                case "по званию":
                    Employees = SelectedSortType == "по возрастанию"
                        ? new ObservableCollection<EmployeeViewModel>(_employees.OrderBy(e => e.Rank.RankWeight))
                        : new ObservableCollection<EmployeeViewModel>(
                            _employees.OrderByDescending(e => e.Rank.RankWeight));
                    break;
                case "по возрасту":
                    Employees = SelectedSortType == "по возрастанию"
                        ? new ObservableCollection<EmployeeViewModel>(_employees.OrderBy(e => e.Age))
                        : new ObservableCollection<EmployeeViewModel>(
                            _employees.OrderByDescending(e => e.Age));
                    break;
                case "по фамилии":
                    Employees = SelectedSortType == "по возрастанию"
                        ? new ObservableCollection<EmployeeViewModel>(_employees.OrderBy(e => e.LastName))
                        : new ObservableCollection<EmployeeViewModel>(
                            _employees.OrderByDescending(e => e.LastName));
                    break;
            }

            //обнуляем поля сортировки
            SetDefaultSortRequisites();
            //переходим к функциональному меню
            MenuTabIndex = 0;
        }

        /// <summary>
        /// TODO
        /// </summary>
        private RelayCommand _applyFiltration;

        private string _sortErrorText;

        public RelayCommand ApplyFiltration
            => _applyFiltration ?? (_applyFiltration = new RelayCommand(ApplyFiltrationExecute));

        private void ApplyFiltrationExecute()
        {
            var t = Ranks.Where(p => p.IsSelected);
            var t2 = t.Count();
        }

        #endregion

        /// <summary>
        /// Устанавливает реквизиты сортировки по умолчанию
        /// </summary>
        public void SetDefaultSortRequisites()
        {
            SelectedSortParameter = null;
            SelectedSortType = null;
        }
    }
}
