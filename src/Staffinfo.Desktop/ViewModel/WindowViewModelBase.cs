using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Shared;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Базовый для view models, которые привязываются к окну
    /// </summary>
    public class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// true - закрыть окно
        /// </summary>
        private bool _windowsClosed;

        public bool WindowsClosed
        {
            get { return _windowsClosed; }
            set
            {
                if (_windowsClosed == value)
                    return;
                _windowsClosed = value;
                RaisePropertyChanged("WindowsClosed");
            }
        }

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

        #region CloseCommand

        private RelayCommand _closeWindowCommand;

        /// <summary>
        /// Закрыть окно
        /// </summary>
        public RelayCommand CloseWindowCommand
            => _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindow));

        private void CloseWindow()
        {
            WindowsClosed = true;
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