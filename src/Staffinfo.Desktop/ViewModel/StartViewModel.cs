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
                MessageBox.Show("������ ��� �������� ������!" + ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Fields
        /// <summary>
        /// ��������
        /// </summary>
        private string _middleName;

        /// <summary>
        /// ���
        /// </summary>
        private string _firstName;

        /// <summary>
        /// �������
        /// </summary>
        private string _lastName;

        /// <summary>
        /// ������� �������
        /// </summary>
        private int _accessLevel;

        /// <summary>
        /// �����
        /// </summary>
        private string _login;

        /// <summary>
        /// ������
        /// </summary>
        private string _password;

        /// <summary>
        /// ��������� ���
        /// </summary>
        private int _selectedTabIndex;

        /// <summary>
        /// �������������� ������������
        /// </summary>
        private UserModel _user;

        /// <summary>
        /// ������� �����
        /// </summary>
        private string _mode = Resources.AuthorizationMode;

        /// <summary>
        /// �������� ������
        /// </summary>
        private string _selectedServer;

        /// <summary>
        /// ������ ��������
        /// </summary>
        private List<string> _serverNamesList;

        #endregion

        #region Properties

        /// <summary>
        /// ������ ����������
        /// </summary>
        private string ConnectionString => "Data Source=" + SelectedServer + ';' +
                                           $"Initial Catalog={DataSingleton.Instance.DatabaseName}; Integrated Security=SSPI;";

        /// <summary>
        /// �������� ������
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
        /// ����� ��������
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
        /// ��� �������
        /// </summary>
        public string AccessType => User?.AccessLevel == (int)AccessLevelType.Reader ? "reader: " :
            "admin: ";

        /// <summary>
        /// ������ ��� ������������
        /// </summary>
        public string FullName => User?.LastName + ' ' + User?.FirstName + ' ' + User?.MiddleName;

        /// <summary>
        /// visibility ��� ������ settings � �������
        /// </summary>
        public Visibility SettingVisibility
        {
            get
            {
                return User == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                if (value.Length > 20) throw new Exception("������� ������� �����");
                _login = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (value.Length > 20) throw new Exception("������� ������� ������");
                _password = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ������� ���
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
        /// �������������� ������������
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
        /// ������� ����� (�����������/������� ����)
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
        /// �������� ������ ��������
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
                    MessageBox.Show("������ ������������� ��������." + ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            });
            SelectedTabIndex = 1;
        }

        #endregion

        #region WindowLoadedCommand

        /// <summary>
        /// ����������� ����� ����� ������� ���������� ����
        /// </summary>
        private RelayCommand _windowLoadedCommand;

        public RelayCommand WindowLoadedCommand
            => _windowLoadedCommand ?? (_windowLoadedCommand = new RelayCommand(WindowLoaded));

        /// <summary>
        /// ���������� ��������� ������� (����������)
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
                        MessageBox.Show("������ ������������� ��������." + fEx.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                });
            SelectedTabIndex = 1;
            if (ServerNamesList?.Count == 0)
            {
                MessageBox.Show(
                    "��������� �������� �� �������. ���������, �������� �� ������ \"������������ SQL server\".",
                    "������", MessageBoxButton.OK, MessageBoxImage.Error);
                WindowsClosed = true;
            }

            
        }

        #endregion

        #region ShowUsers command

        private RelayCommand _showUsers;

        public RelayCommand ShowUsers => _showUsers ?? (_showUsers = new RelayCommand(ShowUsersExecute));
        
        /// <summary>
        /// ������� ���� �������������
        /// </summary>
        private void ShowUsersExecute()
        {
            var usersView = new UsersView() { DataContext = new UserViewModel(DataSingleton.Instance.User) }; //�������� data context
            usersView.ShowDialog();
        }

        #endregion
        
        #region Authorization command
        private RelayCommand _authorization;

        public RelayCommand Authorization => _authorization ?? (_authorization = new RelayCommand(AuthorizationExecute));

        /// <summary>
        /// �����������
        /// </summary>
        private void AuthorizationExecute()
        {
            try
            {
                //����������, ���� ������ �� ������
                if (SelectedServer == null) throw new Exception("�� ������ ������.");
                try
                {
                    FindDatabase(); //�������������� ������ �� �� � �������������� ��������� connection string

                    DataSingleton.Instance.DataInitialize(ConnectionString);    //����������� ��������� ����������
                    using (var prvdr = new UserTableProvider())
                    {
                        try
                        {
                            if ((User = prvdr.Check(Login, Password)) == null)  //���������� ������������
                                throw new AuthorizationException("������������ �� ������");

                            DataSingleton.Instance.User = User;
                            Mode = Resources.MainMode; //title ��� ����
                            SelectedTabIndex = 2;
                        }
                        catch (AuthorizationException aEx)
                        {
                            throw new AuthorizationException("������ �����������. " + aEx.Message, aEx);
                        }
                    }
                }
                catch (DatabaseNotFoundException dbEx)  //���� �� ����� ��, �������
                {
                    var answer = MessageBox.Show("���� ������ �������� �� ������� �� ��������� �������.\n\t\t������� ��?",
                        "���� ������ �� �������", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (answer == MessageBoxResult.No) return;

                    try
                    {
                        DatabaseHelper.CreateDatabase("Data Source=" + SelectedServer + ';' +
                                                      "Integrated Security=SSPI;");
                        MessageBox.Show("���� ������ ���� ������� �������.",
                            "���� ������ �������", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseWasNotCreatedException(
                            "�� ������� ������� ���� ������ �� ���� �������. ������: " + e.Message +
                            "��������� ������ ��� ���������� �����.", e);
                    }
                }
            }
            catch (DatabaseWasNotCreatedException dncEx)
            {
                MessageBox.Show(dncEx.Message,
                        "�� ������� ������� ���� ������", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (AuthorizationException aEx)
            {
                MessageBox.Show(aEx.Message, "������ �����������", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ��� �������� ������� �� ������� ����������� ���� ������." + ex.Message,
                        "������", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// �������� �� ������������� ��
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
                if (reader[0].ToString() == "") throw new DatabaseNotFoundException("���� ������ � ����� ������ �� ����������");
                reader.Close();
                sqlConnection.Close();
            }
        }

        #endregion
        
        #region GoToAllEmployeesView command
        private RelayCommand _goToAllEmployeesView;

        /// <summary>
        /// ������� � ����, ��� ������������ ��� ��������
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