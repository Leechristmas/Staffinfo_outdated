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
        public string DirectoryTitle { get; private set; }
    }
}