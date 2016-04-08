using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Shared;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Базовый для view models, которые привязываются к окну
    /// </summary>
    public abstract class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// true - закрыть окно
        /// </summary>
        private bool _windowsClosed;
        
        /// <summary>
        /// блокировка интерфейса
        /// </summary>
        private bool _viewIsEnable = true;

        /// <summary>
        /// Уровень доступа
        /// </summary>
        private int _accessLevel;

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
                //WasChanged = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Является ли авторизованный пользователь администратором
        /// </summary>
        public bool IsAdmin => AccessLevel == (int)AccessLevelType.Admin;

        /// <summary>
        /// Уровень доступа
        /// </summary>
        public string AccessType => User?.AccessLevel == (int)AccessLevelType.Reader ? "reader: " :
            "admin: ";

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullUserName => User?.LastName + ' ' + User?.FirstName + ' ' + User?.MiddleName;

        /// <summary>
        /// Пользователь
        /// </summary>
        public UserModel User
        {
            get { return DataSingleton.Instance.User; }
            set
            {
                DataSingleton.Instance.User = value;

                RaisePropertyChanged();
                RaisePropertyChanged("SettingVisibility");
                RaisePropertyChanged("FullName");
                RaisePropertyChanged("AccessType");
            }
        }

        /// <summary>
        /// Окно закрыто
        /// </summary>
        public bool WindowsClosed
        {
            get { return _windowsClosed; }
            set
            {
                if (_windowsClosed == value)
                    return;
                _windowsClosed = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Валидация view
        /// </summary>
        /// <returns></returns>
        protected virtual bool Validate
        {
            get { return true; }
        }

        /// <summary>
        /// Блокировка интерфейса
        /// </summary>
        public bool ViewIsEnable
        {
            get { return _viewIsEnable; }
            set
            {
                _viewIsEnable = value;
                RaisePropertyChanged(nameof(ViewIsEnable));
            }
        }

        #region CloseCommand

        private RelayCommand _closeWindowCommand;

        /// <summary>
        /// Закрыть окно
        /// </summary>
        public RelayCommand CloseWindowCommand
            => _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindow));

        protected void CloseWindow()
        {
            WindowsClosed = true;   //закрываем окно
            WindowsClosed = false;  //разрешаем открывать окно снова
        }

        #endregion

        #region ShowSettings command

        /// <summary>
        /// Окно сеттингов
        /// </summary>
        private RelayCommand _showSettings;

        public RelayCommand ShowSettings => _showSettings ?? (_showSettings = new RelayCommand(ShowSettingsExecute));

        private void ShowSettingsExecute()
        {
            var settingsView = new SettingsView();
            settingsView.ShowDialog();
        }

        #endregion

    }
}