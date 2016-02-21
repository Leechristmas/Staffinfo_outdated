using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.View;
using System;
using System.Net;
using System.Windows;
using Staffinfo.Desktop.Model;

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
                MessageBox.Show("������ ��� �������� ������!" + ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Fields

        private string _middleName;
        private string _firstName;
        private string _lastName;
        private int _accessLevel;
        private string _login;
        private string _password;

        #endregion

        #region Properties

        public Visibility SettingVisibility
        {
            get
            {
                return User == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ������� ������������
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ������� ������� � ��
        /// </summary>
        public int AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                if (value < 0 || value > 1) throw new Exception("�������� ������� �������");
                _accessLevel = value;
                RaisePropertyChanged();
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
            }
        }

        #endregion

        #region Commands

        #region ShowUsers command

        /// <summary>
        /// ������� ���� �������������
        /// </summary>
        private RelayCommand _showUsers;

        public RelayCommand ShowUsers => _showUsers ?? (_showUsers = new RelayCommand(ShowUsersExecute));

        private void ShowUsersExecute()
        {
            var usersView = new UsersView(); //�������� data context
            usersView.ShowDialog();
        }

        #endregion

        /// <summary>
        /// �����������
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
                        throw new Exception("������������ �� ������");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������ �����������", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            DataSingleton.Instance.User = User;
            SelectedTabIndex = 1;
        }

        #endregion

        /// <summary>
        /// ������� � ����, ��� ������������ ��� ��������
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

        /// <summary>
        ///  ������� �������� ����
        /// </summary>
        #region CloseCommand

        private RelayCommand _closeWindowCommand;

        private int _selectedTabIndex;
        private UserModel _user;

        public RelayCommand CloseWindowCommand
            => _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindow));

        private void CloseWindow()
        {
            WindowsClosed = true;
        }

        #endregion

        #endregion
        
    }
}