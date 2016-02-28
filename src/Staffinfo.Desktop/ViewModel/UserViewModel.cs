using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Shared;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view-model для пользователя
    /// </summary>
    public class UserViewModel: WindowViewModelBase
    {
        #region Constructor

        public UserViewModel(UserModel user): this()
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            AccessLevel = user.AccessLevel;
            Login = user.Login;
            Password = user.Password;
        }

        public UserViewModel()
        {
            try
            {
                using (var prvdr = new UserTableProvider())
                {
                    UserList = new ObservableCollectionViewModel<PartialUserViewModel>(
                        new ObservableCollection<PartialUserViewModel>(prvdr
                            .Select()
                            .Select(user => new PartialUserViewModel(user))));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить пользователей" + ex.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            _wasChanged = false;

        }

        #endregion
        
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
        /// Был ли изменен пользователь
        /// </summary>
        private bool _wasChanged;

        /// <summary>
        /// Индекс активного таба
        /// </summary>
        private int _selectedTabIndex;
        #endregion

        #region Properties

        /// <summary>
        /// Был ли изменен пользователь
        /// </summary>
        public bool WasChanged
        {
            get { return _wasChanged; }
            set
            {
                _wasChanged = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Список пользователей
        /// </summary>
        public /*readonly*/ ObservableCollectionViewModel<PartialUserViewModel> UserList { get; set; }

        /// <summary>
        /// Является ли авторизованный пользователь администратором
        /// </summary>
        public bool IsAdmin => AccessLevel == (int) AccessLevelType.Admin;

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value.Trim();
                WasChanged = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value.Trim();
                WasChanged = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value.Trim();
                WasChanged = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Уровень доступа к БД
        /// </summary>
        public int AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                if (value < 0 || value > 1) throw new Exception("Неверный уровень доступа");
                _accessLevel = value;
                WasChanged = true;
                RaisePropertyChanged();
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
                WasChanged = true;
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
                WasChanged = true;
                RaisePropertyChanged();
            }
        }


        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ShowPasswordChanging command
        /// <summary>
        /// Перейти к изменению пароля
        /// </summary>
        public RelayCommand ShowPasswordChanging
            => _showPasswordChanging ?? (_showPasswordChanging = new RelayCommand(ShowPasswordChangingExecute));
        private RelayCommand _showPasswordChanging;
        private void ShowPasswordChangingExecute()
        {
            SelectedTabIndex = SelectedTabIndex == 0 ? 2 : 0;
        }

        #endregion

        #region SaveChanges command
        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public RelayCommand SaveChanges => _saveChanges ?? (_saveChanges = new RelayCommand(SaveChangesExecute));
        private RelayCommand _saveChanges;
        private void SaveChangesExecute()
        {
            //Нужно сделать проверку на наличие изменений: пришить к активации команды
            var answer = MessageBox.Show("Принять изменения?", "Изменить", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (answer == MessageBoxResult.No) return;

            DataSingleton.Instance.User.Login = Login;
            DataSingleton.Instance.User.AccessLevel = AccessLevel;
            DataSingleton.Instance.User.FirstName = FirstName;
            DataSingleton.Instance.User.LastName = LastName;
            DataSingleton.Instance.User.MiddleName = MiddleName;
            DataSingleton.Instance.User.Password = Password;
        }

        #endregion
    }
}