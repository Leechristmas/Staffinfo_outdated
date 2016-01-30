using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Базовый для view models, которые привязываются к окну
    /// </summary>
    public class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Если свойство меняется, закрывается окно
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
    }
}