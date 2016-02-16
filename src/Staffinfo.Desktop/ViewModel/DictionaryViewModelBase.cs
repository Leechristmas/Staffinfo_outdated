using GalaSoft.MvvmLight.Command;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Базовый класс для view-моделей справочников
    /// </summary>
    public abstract class DictionaryViewModelBase: WindowViewModelBase
    {
        /// <summary>
        /// Название справочника
        /// </summary>
        protected string DirectoryTitle { get; private set; }

        #region Commands

        /// <summary>
        /// Закрыть окно
        /// </summary>
        protected RelayCommand _closeCommand;

        protected RelayCommand CloseCommand
            => _closeCommand ?? (_closeCommand = new RelayCommand(CloseCommandExecute));

        protected void CloseCommandExecute()
        {
            WindowsClosed = true;
        }

        #endregion


    }
}