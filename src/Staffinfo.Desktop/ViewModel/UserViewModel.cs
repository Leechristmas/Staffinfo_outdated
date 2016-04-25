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
        /// Индекс выбранного пользователя
        /// </summary>
        private int? _selectedUserIndex;

        /// <summary>
        /// Текст ошибки при добавлении пользователя
        /// </summary>
        private string _addUserErrorText;
        
        /// <summary>
        /// Имя добавляемого пользователя
        /// </summary>
        private string _newUserFirstName = String.Empty;

        /// <summary>
        /// Фамилия добавляемого пользователя
        /// </summary>
        private string _newUserLastName = String.Empty;

        /// <summary>
        /// Отчество добавляемого пользователя
        /// </summary>
        private string _newUserMiddleName = String.Empty;

        /// <summary>
        /// Логин добавляемого пользователя
        /// </summary>
        private string _newUserLogin = String.Empty;

        /// <summary>
        /// Пароль добавляемого пользователя
        /// </summary>
        private string _newUserPassword = String.Empty;

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

        /// <summary>
        /// Старый пароль
        /// </summary>
        private string _oldPassword;

        /// <summary>
        /// Новый пароль
        /// </summary>
        private string _newPassword;

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        private string _confirmPassword;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        private string _errorText;
        #endregion

        #region Properties

        /// <summary>
        /// Индекс выбранного пользователя
        /// </summary>
        public int? SelectedUserIndex
        {
            get { return _selectedUserIndex; }
            set
            {
                _selectedUserIndex = value;
                RaisePropertyChanged();
            }
        }

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

        /// <summary>
        /// Индекс активного таба (пользователи/смена пароля/добавление пользователя)
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged(nameof(CanDelete));
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Старый пароль
        /// </summary>
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текст ошибки рпи добавлении пользователя
        /// </summary>
        public string AddUserErrorText
        {
            get { return _addUserErrorText; }
            set
            {
                _addUserErrorText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Enable для кнопки удаления
        /// </summary>
        public bool CanDelete => SelectedTabIndex == 1 && UserList.ModelCollection.Count > 0;

        /// <summary>
        /// Имя добавляемого пользователя
        /// </summary>
        public string NewUserFirstName
        {
            get { return _newUserFirstName; }
            set
            {
                _newUserFirstName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Фамилия добавляемого пользователя
        /// </summary>
        public string NewUserLastName
        {
            get { return _newUserLastName; }
            set
            {
                _newUserLastName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ОТчество добавляемого пользователя
        /// </summary>
        public string NewUserMiddleName
        {
            get { return _newUserMiddleName; }
            set
            {
                _newUserMiddleName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Логин добавляемого пользователя
        /// </summary>
        public string NewUserLogin
        {
            get { return _newUserLogin; }
            set
            {
                _newUserLogin = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Пароль добавляемого польтзователя
        /// </summary>
        public string NewUserPassword
        {
            get { return _newUserPassword; }
            set
            {
                _newUserPassword = value;
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
            NewPassword = String.Empty;
            OldPassword = String.Empty;
            ConfirmPassword = String.Empty;
            SelectedTabIndex = SelectedTabIndex == 0 ? 2 : 0;
        }

        #endregion

        #region ToMainTab command
        /// <summary>
        /// Перейти к изменению пароля
        /// </summary>
        private RelayCommand _toMainTab;
        public RelayCommand ToMainTab
            => _toMainTab ?? (_toMainTab = new RelayCommand(ToMainTabExecute));
        private void ToMainTabExecute()
        {
            SelectedTabIndex = 0;
            SetAddUserRequisitesDefault();
        }
        #endregion

        #region ToAddOnTab command
        /// <summary>
        /// Перейти к добавлению
        /// </summary>
        private RelayCommand _toAddTab;
        public RelayCommand ToAddTab
            => _toAddTab ?? (_toAddTab = new RelayCommand(ToAddTabExecute));
        private void ToAddTabExecute()
        {
            SelectedTabIndex = 3;
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

            using (var prvdr = new UserTableProvider())
            {
                var updatedUser = new UserModel
                {
                    Id = User.Id,
                    Login = Login,
                    Password = Password,
                    LastName = LastName,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    AccessLevel = AccessLevel
                };
                if (!prvdr.Update(updatedUser))
                {
                    MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                User = updatedUser;
            }
        }

        #endregion

        #region AcceptNewPassword command

        /// <summary>
        /// 
        /// </summary>
        private RelayCommand _acceptNewPassword;

        public RelayCommand AcceptNewPassword
            => _acceptNewPassword ?? (_acceptNewPassword = new RelayCommand(AcceptNewPasswordExecute));

        private void AcceptNewPasswordExecute()
        {
            ErrorText = null;

            if (String.CompareOrdinal(OldPassword, User.Password) != 0)
            {
                ErrorText = "Текущий пароль указан неверно";
                return;
            }
            if (String.CompareOrdinal(ConfirmPassword, NewPassword) != 0)
            {
                ErrorText = "Пароли не совпадают.";
                ConfirmPassword = String.Empty;
                return;
            }
            if (NewPassword.Length < 5)
            {
                ErrorText = "Не достаточно длинный пароль. Минимальное количество символов - 5";
                return;
            }
            if (NewPassword.Length > 15)
            {
                ErrorText = "Слишком длинный пароль. Максимальное количество символов - 15";
            }

            using (var prvdr = new UserTableProvider())
            {
                if (!prvdr.UpdatePassword(User.Id, NewPassword))
                {
                    MessageBox.Show("Не удалось обновить пароль!" + prvdr.ErrorInfo, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
            }
            User.Password = NewPassword;
            ToMainTabExecute();
        }

        #endregion

        #region AddUser command

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        private RelayCommand _addUser;

        public RelayCommand AddUser => _addUser ?? (_addUser = new RelayCommand(AddUserExecute));

        private void AddUserExecute()
        {
            AddUserErrorText = null;

            if (NewUserFirstName.Length < 3 || NewUserFirstName.Length > 15)
            {
                AddUserErrorText = "Имя указано некорректно.";
                return;
            }
            if (NewUserLastName.Length < 3 || NewUserLastName.Length > 20)
            {
                AddUserErrorText = "Фамилия указана некорректно.";
                return;
            }
            if (NewUserMiddleName.Length < 3 || NewUserMiddleName.Length > 15)
            {
                AddUserErrorText = "Отчество указано некорректно.";
                return;
            }
            if (NewUserLogin.Length < 5 || NewUserLogin.Length > 15)
            {
                AddUserErrorText = "Логин указан некорректно.";
                return;
            }
            if (NewUserPassword.Length < 5 || NewUserPassword.Length > 15)
            {
                AddUserErrorText = "Пароль указан некорректно.";
                return;
            }

            using (var prvdr = new UserTableProvider())
            {
                var user = prvdr.Save(new UserModel
                {
                    LastName = NewUserLastName,
                    FirstName = NewUserFirstName,
                    MiddleName = NewUserMiddleName,
                    Login = NewUserLogin,
                    Password = NewUserPassword,
                    AccessLevel = 0
                });
                if (user == null)
                {
                    MessageBox.Show("Не удалось добавить пользователя." + prvdr.ErrorInfo, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                UserList.ModelCollection.Add(new PartialUserViewModel(user));
            }
            SetAddUserRequisitesDefault();
            SelectedTabIndex = 1;
        }

        #endregion

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        private RelayCommand _removeUser;
        public RelayCommand RemoveUser => _removeUser ?? (_removeUser = new RelayCommand(RemoveUserExecute));

        private void RemoveUserExecute()
        {
            if (SelectedUserIndex == null)
            {
                MessageBox.Show("Пользователь не выбран.", "Ошибка", MessageBoxButton.OK,
                         MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            if (UserList.SelectedItem.Id == User.Id)
            {
                MessageBox.Show("Операция невозможна. Обратитесь к администритору базы данных.", "Ошибка", MessageBoxButton.OK,
                         MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            using (var prvdr = new UserTableProvider())
            {
                if (!prvdr.DeleteById(UserList.SelectedItem.Id))
                {
                    MessageBox.Show("Не удалось удалить пользователя." + prvdr.ErrorInfo, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                if (SelectedUserIndex != null) UserList.ModelCollection.RemoveAt(SelectedUserIndex.Value);
            }
        }

        /// <summary>
        /// Устанавливает значения реквизитов добавления пользователя по умолчанию
        /// </summary>
        private void SetAddUserRequisitesDefault()
        {
            NewUserFirstName = NewUserMiddleName = NewUserLastName = NewUserLogin = NewUserPassword = null;
        }

    }
}