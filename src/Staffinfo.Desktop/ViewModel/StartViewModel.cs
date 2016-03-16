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
            try
            {
                //SelectedTabIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Îøèáêà ïðè çàãðóçêå äàííûõ!" + ex.Message, "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Fields
        /// <summary>
        /// Îò÷åñòâî
        /// </summary>
        private string _middleName;

        /// <summary>
        /// Èìÿ
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Ôàìèëèÿ
        /// </summary>
        private string _lastName;

        /// <summary>
        /// Óðîâåíü äîñòóïà
        /// </summary>
        private int _accessLevel;

        /// <summary>
        /// Ëîãèí
        /// </summary>
        private string _login;

        /// <summary>
        /// Ïàðîëü
        /// </summary>
        private string _password;

        /// <summary>
        /// Âûáðàííûé òàá
        /// </summary>
        private int _selectedTabIndex;

        /// <summary>
        /// Àâòîðèçîâàííûé ïîëüçîâàòåëü
        /// </summary>
        private UserModel _user;

        /// <summary>
        /// Òåêóùèé ðåæèì
        /// </summary>
        private string _mode = Resources.AuthorizationMode;

        /// <summary>
        /// Àêòèâíûé ñåðâåð
        /// </summary>
        private string _selectedServer;

        /// <summary>
        /// Ñïèñîê ñåðâåðîâ
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
                if (value.Length > 20) throw new Exception("Ñëèøêîì äëèííûé ëîãèí");
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
                if (value.Length > 20) throw new Exception("Ñëèøêîì äëèííûé ïàðîëü");
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
        /// Òåêóùèé ðåæèì (àâòîðèçàöèÿ/ãëàâíîå ìåíþ)
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
        /// Îáíîâèòü ñïèñîê ñåðâåðîâ
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
                    MessageBox.Show("Îøèáêà èíèöèàëèçàöèè ñåðâåðîâ." + ex.Message, "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            });
            SelectedTabIndex = 1;
        }

        #endregion

        #region WindowLoadedCommand

        /// <summary>
        /// Âûïîëíÿåòñÿ ñðàçó ïîñëå çàïóñêà ñòàðòîâîãî îêíà
        /// </summary>
        private RelayCommand _windowLoadedCommand;

        public RelayCommand WindowLoadedCommand
            => _windowLoadedCommand ?? (_windowLoadedCommand = new RelayCommand(WindowLoaded));

        /// <summary>
        /// Ïîäãðóæàåì ëîêàëüíûå ñåðâåðà (àñèíõðîííî)
        /// </summary>
        private async void WindowLoaded()
        {
            ServerNamesList = await Task.Run(() =>
                {
                    try
                    {
                        return DatabaseHelper.LoadServerInstances();
                    }
                    catch (FileNotFoundException fEx)
                    {
                        MessageBox.Show("Îøèáêà èíèöèàëèçàöèè ñåðâåðîâ." + fEx.Message, "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                });
            SelectedTabIndex = 1;
            if (ServerNamesList?.Count == 0)
            {
                MessageBox.Show(
                    "Ëîêàëüíûõ ñåðâåðîâ íå íàéäåíî. Ïðîâåðüòå, âêëþ÷åíà ëè ñëóæáà \"Îáîçðåâàòåëü SQL server\" è îáíîâèòå ñïèñîê ñåðâåðîâ",
                    "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        #endregion

        #region ShowUsers command

        private RelayCommand _showUsers;

        public RelayCommand ShowUsers => _showUsers ?? (_showUsers = new RelayCommand(ShowUsersExecute));
        
        /// <summary>
        /// Îòêðûòü îêíî ïîëüçîâàòåëåé
        /// </summary>
        private void ShowUsersExecute()
        {
            var usersView = new UsersView() { DataContext = new UserViewModel(DataSingleton.Instance.User) }; //äîáàâèòü data context
            usersView.ShowDialog();
        }

        #endregion
        
        #region Authorization command
        private RelayCommand _authorization;

        public RelayCommand Authorization => _authorization ?? (_authorization = new RelayCommand(AuthorizationExecute));

        /// <summary>
        /// Àâòîðèçàöèÿ
        /// </summary>
        private void AuthorizationExecute()
        {
            try
            {
                //Óâåäîìëÿåì, åñëè ñåðâåð íå âûáðàí
                if (SelectedServer == null) throw new Exception("Íå âûáðàí ñåðâåð.");
                try
                {
                    FindDatabase(); //èíèöèàëèçèðóåì äàííûå èç ÁÄ ñ èñïîëüçîâàíèåì óêàçàííîé connection string

                    DataSingleton.Instance.DataInitialize(ConnectionString);    //ïîäòÿãèâàåì ñëóæåáíûþ èíôîðìàöèþ
                    using (var prvdr = new UserTableProvider())
                    {
                        try
                        {
                            if ((User = prvdr.Check(Login, Password)) == null)  //àâòîðèçóåì ïîëüçîâàòåëÿ
                                throw new AuthorizationException("Ïîëüçîâàòåëü íå íàéäåí");

                            DataSingleton.Instance.User = User;
                            Mode = Resources.MainMode; //title äëÿ îêíà
                            SelectedTabIndex = 2;
                        }
                        catch (AuthorizationException aEx)
                        {
                            throw new AuthorizationException("Îøèáêà àâòîðèçàöèè. " + aEx.Message, aEx);
                        }
                    }
                }
                catch (DatabaseNotFoundException dbEx)  //åñëè íå íàøëè ÁÄ, ñîçäàåì
                {
                    var answer = MessageBox.Show("Áàçà äàííûõ ñëóæàùèõ íå íàéäåíà íà óêàçàííîì ñåðâåðå.\n\t\tÑîçäàòü åå?",
                        "Áàçà äàííûõ íå íàéäåíà", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (answer == MessageBoxResult.No) return;

                    try
                    {
                        DatabaseHelper.CreateDatabase("Data Source=" + SelectedServer + ';' +
                                                      "Integrated Security=SSPI;");
                        MessageBox.Show("Áàçà äàííûõ áûëà óñïåøíî ñîçäàíà.",
                            "Áàçà äàííûõ ñîçäàíà", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseWasNotCreatedException(
                            "Íå óäàëîñü ñîçäàòü áàçó äàííûõ íà ýòîì ñåðâåðå. Îøèáêà: " + e.Message +
                            "Ïîçâîíèòå Äèìîíó èëè ïîïðîáóéòå ïîçæå.", e);
                    }
                }
            }
            catch (DatabaseWasNotCreatedException dncEx)
            {
                MessageBox.Show(dncEx.Message,
                        "Íå óäàëîñü ñîçäàòü áàçó äàííûõ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (AuthorizationException aEx)
            {
                MessageBox.Show(aEx.Message, "Îøèáêà àâòîðèçàöèè", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Îøèáêà ïðè ïðîâåðêå ñåðâåðà íà íàëè÷èå íåîáõîäèìîé áàçû äàííûõ." + ex.Message,
                        "Îøèáêà", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Ïðîâåðêà íà ñóùåñòâîâàíèå ÁÄ
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
                if (reader[0].ToString() == "") throw new DatabaseNotFoundException("Áàçà äàííûõ ñ òàêèì èìåíåì íå ñóùåñòâóåò");
                reader.Close();
                sqlConnection.Close();
            }
        }

        #endregion
        
        #region GoToAllEmployeesView command
        private RelayCommand _goToAllEmployeesView;

        /// <summary>
        /// Ïåðåõîä ê îêíó, ãäå îòîáðàæàþòñÿ âñå ñëóæàùèå
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