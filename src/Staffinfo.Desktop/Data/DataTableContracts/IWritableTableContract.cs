using System.Collections.ObjectModel;

namespace Staffinfo.Desktop.Data.DataTableContracts
{
    /// <summary>
    /// Компонент обращений к таблице (чтение и запись)
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IWritableTableContract<T>
    {
        /// <summary>
        /// Текст ошибки
        /// </summary>
        string ErrorInfo { get; set; }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="obj">элемент для добавления</param>
        /// <returns></returns>
        T Save(T obj);

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

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="obj">обновляемый элемент</param>
        /// <returns></returns>
        bool Update(T obj);

        /// <summary>
        /// Удалить элемент по id
        /// </summary>
        /// <param name="id">id элемента</param>
        bool DeleteById(long? id);
    }
}