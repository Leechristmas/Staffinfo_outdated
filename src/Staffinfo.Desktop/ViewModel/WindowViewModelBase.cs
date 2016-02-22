using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        #region CloseCommand

        private RelayCommand _closeWindowCommand;

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