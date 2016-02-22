using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.View;
using System;
using System.Net;
using System.Windows;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;
using Staffinfo.Desktop.Shared;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StartViewModel : WindowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public StartViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            try
            {
                DataSingleton.Instance.DataInitialize();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных!" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Fields
        /// <summary>
        /// Отчество
        /// </summary>
        private string _middleName;

        /// <summary>
        /// Имя
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Фамилия
        /// </summary>
        private string _lastName;
        
        /// <summary>
        /// Уровень доступа
        /// </summary>
        private int _accessLevel;

        /// <summary>
        /// Логин
        /// </summary>
        private string _login;

        /// <summary>
        /// Пароль
        /// </summary>
        private string _password;

        /// <summary>
        /// Выбранный таб
        /// </summary>
        private int _selectedTabIndex;

        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        private UserModel _user;

        /// <summary>
        /// Текущий режим
        /// </summary>
        private string _mode = Resources.AuthorizationMode;

        #endregion

        #region Properties

        /// <summary>
        /// Тип доступа
        /// </summary>
        public string AccessType => User?.AccessLevel == (int) AccessLevelType.Reader ? "reader: ":
            "admin: ";

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName => User?.LastName + ' ' + User?.FirstName + ' ' + User?.MiddleName;

        /// <summary>
        /// visibility для кнопки settings в топбаре
        /// </summary>
        public Visibility SettingVisibility
        {
            get
            {
                return User == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                if (value.Length > 20) throw new Exception("Слишком длинный логин");
                _login = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (value.Length > 20) throw new Exception("Слишком длинный пароль");
                _password = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текущий таб
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        private UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;

                RaisePropertyChanged();
                RaisePropertyChanged("SettingVisibility");
                RaisePropertyChanged("FullName");
                RaisePropertyChanged("AccessType");
            }
        }

        /// <summary>
        /// Текущий режим (авторизация/главное меню)
        /// </summary>
        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands
        
        /// <summary>
        /// Открыть окно пользователей
        /// </summary>
        #region ShowUsers command

        private RelayCommand _showUsers;

        public RelayCommand ShowUsers => _showUsers ?? (_showUsers = new RelayCommand(ShowUsersExecute));

        private void ShowUsersExecute()
        {
            var usersView = new UsersView() {DataContext = new UserViewModel(DataSingleton.Instance.User)}; //добавить data context
            usersView.ShowDialog();
        }

        #endregion

        /// <summary>
        /// Авторизация
        /// </summary>
        #region Authorization command
        private RelayCommand _authorization;

        public RelayCommand Authorization => _authorization ?? (_authorization = new RelayCommand(AuthorizationExecute));

        private void AuthorizationExecute()
        {
            using (var prvdr = new UserTableProvider())
            {
                try
                {
                    if ((User = prvdr.Check(Login, Password)) == null)
                        throw new Exception("Пользователь не найден");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка авторизации", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            DataSingleton.Instance.User = User;
            Mode = Resources.MainMode;
            SelectedTabIndex = 1;
        }

        #endregion

        /// <summary>
        /// Переход к окну, где отображаются все служащие
        /// </summary>
        #region GoToAllEmployeesView command
        private RelayCommand _goToAllEmployeesView;

        public RelayCommand GoToAllEmployeesView
            => _goToAllEmployeesView ?? (_goToAllEmployeesView = new RelayCommand(GoToAllEmployeesViewExecute));

        private void GoToAllEmployeesViewExecute()
        {
            var allEmployeesView = new AllEmployeesView();
            allEmployeesView.ShowDialog();
        }

        #endregion
        
        #endregion
        
    }
}