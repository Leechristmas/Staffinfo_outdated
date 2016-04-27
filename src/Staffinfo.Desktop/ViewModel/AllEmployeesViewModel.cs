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

            _posts = DataSingleton.Instance.PostList.Select(p => new PostViewModel(p)).ToList();
        }

        #region Fields

        private readonly List<PostViewModel> _posts;

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
        /// ВЫбранная служба (фильтрация)
        /// </summary>
        private ServiceModel _selectedService;

        /// <summary>
        /// Ширина бокового меню
        /// </summary>
        private int _menuWidth = 35;

        /// <summary>
        /// Текущий таб (фильтрация/раб. область)
        /// </summary>
        private int _actualTab;

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

        /// <summary>
        /// Текст ошибки (сортировка)
        /// </summary>
        private string _sortErrorText;

        /// <summary>
        /// Текст ошибки (филтрация)
        /// </summary>
        private string _filterErrorText;

        /// <summary>
        /// Нач. возраст (для фильтрации)
        /// </summary>
        private int? _startAge;

        /// <summary>
        /// Финальный возраст (для фильтрации)
        /// </summary>
        private int? _finishAge;

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
        /// Текст ошибки при неудачной фильтрации
        /// </summary>
        public string FilterErrorText
        {
            get { return _filterErrorText; }
            set
            {
                _filterErrorText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Начальный возраст (для фильтрации)
        /// </summary>
        public int? StartAge
        {
            get { return _startAge; }
            set
            {
                _startAge = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Финальный возраст (для фильтрации)
        /// </summary>
        public int? FinishAge
        {
            get { return _finishAge; }
            set
            {
                _finishAge = value;
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
            get { return _canDelete && IsAdmin; }
            set
            {
                _canDelete = value;
                RaisePropertyChanged(nameof(CanDelete));
                RaisePropertyChanged(nameof(RemoveEmployee));
            }
        }

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

                //применим филр
                ApplyFiltrationExecute();

                //селекция сотрудников согласно введенному тексту
                Employees = new ObservableCollection<EmployeeViewModel>(Employees.Where(e => e.LastName.ToLower().StartsWith(SearchText.ToLower())));
                

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
        /// Должности
        /// </summary>
        public List<PostViewModel> Posts
        {
            get { return _posts.Where(p => p.ServiceId == SelectedService?.Id).ToList(); }
        }

        /// <summary>
        /// Службы
        /// </summary>
        public List<ServiceModel> Services => DataSingleton.Instance.ServiceList;

        /// <summary>
        /// Выбранная служба
        /// </summary>
        public ServiceModel SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Posts));
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
            SetDefaultSortRequisites();
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
            CanDelete = Employees.Count > 0;
        }

        /// <summary>
        /// Удалить служащего
        /// </summary>
        private RelayCommand _removeEmployee;
        public RelayCommand RemoveEmployee
            => _removeEmployee ?? (_removeEmployee = new RelayCommand(RemoveEmployeeExecute));

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
            MenuTabIndex = 2;
            if (MenuWidth == 35) MenuWidth = 200;

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
        /// Применить фильтрацию
        /// </summary>
        private RelayCommand _applyFiltration;
        public RelayCommand ApplyFiltration
            => _applyFiltration ?? (_applyFiltration = new RelayCommand(ApplyFiltrationExecute));

        private void ApplyFiltrationExecute()
        {
            FilterErrorText = null;

            if (StartAge < 18 || StartAge > FinishAge || FinishAge > 60)
            {
                FilterErrorText = "Возраст указан неверно";
                return;
            } 

            var selectedRanks = Ranks.Where(r => r.IsSelected).ToList();
            var selectedPosts = Posts.Where(p => p.IsSelected).ToList();

            var result = (from item in DataSingleton.Instance.EmployeeList
                where ((!selectedRanks.Any() || selectedRanks.Any(r => r.RankId == item.Rank.Id)) &&
                       (!selectedPosts.Any() || selectedPosts.Any(p => p.PostId == item.Post.Id)) &&
                       (StartAge == null || item.Age >= StartAge) &&
                       (FinishAge == null || item.Age <= FinishAge))
                select item).ToList();

            Employees = new ObservableCollection<EmployeeViewModel>(result.Where(e => e.LastName.ToLower().StartsWith(SearchText.ToLower())));
            CanDelete = Employees.Count > 0;
        }

        /// <summary>
        /// Сбросить фильтрацию
        /// </summary>
        private RelayCommand _defaultFilter;
        public RelayCommand DefaultFilter => _defaultFilter ?? (_defaultFilter = new RelayCommand(DefaultFilterExecute));

        private void DefaultFilterExecute()
        {
            SetDefaultFilterRequisites();    
        }

        #endregion

        /// <summary>
        /// Устанавливает реквизиты сортировки по умолчанию
        /// </summary>
        public void SetDefaultSortRequisites()
        {
            SortErrorText = null;
            SelectedSortParameter = null;
            SelectedSortType = null;
        }

        /// <summary>
        /// Устанавливает реквизиты фильтрации по умолчанию
        /// </summary>
        public void SetDefaultFilterRequisites()
        {
            SelectedService = null;
            foreach (var rank in Ranks)
            {
                if(rank.IsSelected) rank.IsSelected = false;
            }
            foreach (var post in Posts)
            {
                if (post.IsSelected) post.IsSelected = false;
            }
            StartAge = null;
            FinishAge = null;
            FilterErrorText = null;
            Employees =
                new ObservableCollection<EmployeeViewModel>(
                    DataSingleton.Instance.EmployeeList.Where(e => e.LastName.ToLower().StartsWith(SearchText.ToLower())));
        }
    }
}
