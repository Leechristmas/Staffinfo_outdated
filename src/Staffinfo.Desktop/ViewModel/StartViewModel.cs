using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
        /// Активный таб
        /// </summary>
        private int _selectedTabIndex;

        /// <summary>
        /// Активный пользователь
        /// </summary>
        private UserModel _user;

        /// <summary>
        /// Режим
        /// </summary>
        private string _mode = Resources.AuthorizationMode;

        /// <summary>
        /// Активный сервер
        /// </summary>
        private string _selectedServer;

        /// <summary>
        /// Список серверов
        /// </summary>
        private List<string> _serverNamesList;

        #endregion

        #region Properties

        /// <summary>
        /// Строка подключения
        /// </summary>
        private string ConnectionString => "Data Source=" + SelectedServer + ';' +
                                           $"Initial Catalog={DataSingleton.Instance.DatabaseName}; Integrated Security=SSPI;";

        /// <summary>
        /// Активный сервер
        /// </summary>
        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ConnectionString");
            }
        }

        /// <summary>
        /// Список имен серверов
        /// </summary>
        public List<string> ServerNamesList
        {
            get { return _serverNamesList; }
            set
            {
                _serverNamesList = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Уровень доступа
        /// </summary>
        public string AccessType => User?.AccessLevel == (int)AccessLevelType.Reader ? "reader: " :
            "admin: ";

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName => User?.LastName + ' ' + User?.FirstName + ' ' + User?.MiddleName;

        /// <summary>
        /// visibility для кнопки открытия сеттингов
        /// </summary>
        public Visibility SettingVisibility => User == null ? Visibility.Collapsed : Visibility.Visible;

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
        /// Активный таб
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
        /// Пользователь
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
        /// Режим (Авторизация/главное меню)
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

        #region RefreshServerListCommand

        private RelayCommand _refreshServerListCommand;

        public RelayCommand RefreshServerListCommand
            =>
                _refreshServerListCommand ??
                (_refreshServerListCommand = new RelayCommand(RefreshServerListCommandExecute));

        /// <summary>
        /// Обновляет список серверов
        /// </summary>
        private async void RefreshServerListCommandExecute()
        {
            SelectedTabIndex = 0;
            ServerNamesList = await Task.Run(() =>
            {
                try
                {
                    DatabaseHelper.SaveServerInstancesIntoFile();
                    return DatabaseHelper.LoadServerInstances();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка инициализации серверов." + ex.Message, "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            });
            SelectedTabIndex = 1;
        }

        #endregion

        #region WindowLoadedCommand

        /// <summary>
        /// Выполняется сразу после загрузки
        /// </summary>
        private RelayCommand _windowLoadedCommand;

        public RelayCommand WindowLoadedCommand
            => _windowLoadedCommand ?? (_windowLoadedCommand = new RelayCommand(WindowLoadedExecute));

        private async void WindowLoadedExecute()
        {
            ServerNamesList = await Task.Run(() =>
                {
                    try
                    {
                        return DatabaseHelper.LoadServerInstances();
                    }
                    catch (FileNotFoundException fEx)
                    {
                        MessageBox.Show("Ошибка:" + fEx.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                });
            SelectedTabIndex = 1;
            if (ServerNamesList?.Count == 0)
            {
                MessageBox.Show(
                    "На компьютере не найдены экземпляры SQL Server. Включите службу \"Обозреватель SQL server\" и попытайтесь снова",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ShowUsers command

        private RelayCommand _showUsers;

        public RelayCommand ShowUsers => _showUsers ?? (_showUsers = new RelayCommand(ShowUsersExecute));
        
        /// <summary>
        /// Открыть users-view
        /// </summary>
        private void ShowUsersExecute()
        {
            var usersView = new UsersView() { DataContext = new UserViewModel(DataSingleton.Instance.User) };
            usersView.ShowDialog();
        }

        #endregion
        
        #region Authorization command
        private RelayCommand _authorization;

        public RelayCommand Authorization => _authorization ?? (_authorization = new RelayCommand(AuthorizationExecute));

        /// <summary>
        /// Авторизация
        /// </summary>
        private void AuthorizationExecute()
        {
            try
            {
                //Проверка, выбран ли сервер
                if (SelectedServer == null) throw new Exception("Сервер не выбран. ");
                try
                {
                    FindDatabase(); //ищем базу данных на указанном сервере

                    DataSingleton.Instance.DataInitialize(ConnectionString);    //инициализируем служебные данные
                    using (var prvdr = new UserTableProvider())
                    {
                        try
                        {
                            if ((User = prvdr.Check(Login, Password)) == null)  //авторизация
                                throw new AuthorizationException("Неверный логин или пароль.");

                            DataSingleton.Instance.User = User;
                            Mode = Resources.MainMode; //title for main-view
                            SelectedTabIndex = 2;
                        }
                        catch (AuthorizationException aEx)
                        {
                            throw new AuthorizationException("Ошибка авторизации. " + aEx.Message, aEx);
                        }
                    }
                }
                catch (DatabaseNotFoundException dbEx)  //База данных не найдена
                {
                    var answer = MessageBox.Show("База данных не найдена на выбранном сервере.\n\t\tСоздать?",
                        "База данных не найдена", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (answer == MessageBoxResult.No) return;

                    try
                    {
                        DatabaseHelper.CreateDatabase("Data Source=" + SelectedServer + ';' +
                                                      "Integrated Security=SSPI;");
                        MessageBox.Show("База данных создана. ",
                            "База данных создана", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseWasNotCreatedException(
                            "Не удалось создать базу данных. Ошибка: " + e.Message, e);
                    }
                }
            }
            catch (DatabaseWasNotCreatedException dncEx)
            {
                MessageBox.Show(dncEx.Message,
                        "Не удалось создать базу данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (AuthorizationException aEx)
            {
                MessageBox.Show(aEx.Message, "Ошибка авторизации", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Login = "";
                Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения. Обновите сервер и попробуйте снова." + ex.Message,
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Login = "";
                Password = "";
            }
        }

        /// <summary>
        /// Инициализирует список всех экземпляров SQL Server.
        /// </summary>
        private void FindDatabase()
        {
            using (var sqlConnection = new SqlConnection("Data Source=" + SelectedServer + ';' +
                "Integrated Security=SSPI;"))
            {
                sqlConnection.Open();
                var cmd = new SqlCommand("SELECT DB_ID('staffinfo_tests')", sqlConnection);
                var reader = cmd.ExecuteReader();
                reader.Read();
                if (reader[0].ToString() == "") throw new DatabaseNotFoundException("Серверов не найдено.");
                reader.Close();
                sqlConnection.Close();
            }
        }

        #endregion
        
        #region GoToAllEmployeesView command
        private RelayCommand _goToAllEmployeesView;

        /// <summary>
        /// Перейти к окну отображения сотрудников
        /// </summary>
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