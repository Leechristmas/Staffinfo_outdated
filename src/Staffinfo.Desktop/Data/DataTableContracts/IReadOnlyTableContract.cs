using System.Collections.ObjectModel;

namespace Staffinfo.Desktop.Data.DataTableContracts
{
    /// <summary>
    /// Компонент для обращений к базе (только чтение)
    /// </summary>
    public interface IReadOnlyTableContract<T>
    {
        /// <summary>
        /// Текст ошибки
        /// </summary>
        string ErrorInfo { get; set; }

        /// <summary>
        /// Вернуть элемент по id
        /// </summary>
        /// <param name="id">id элемента</param>
        /// <returns></returns>
        T Select(long? id);

        /// <summary>
        /// Вернуть все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        ObservableCollection<T> Select();
    }
}