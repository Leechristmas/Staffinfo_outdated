using System.Collections.ObjectModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Интерфейс для классов работы с БД
    /// </summary>
    public interface ITableProvider
    {
        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="obj">элемент для добавления</param>
        /// <returns></returns>
        BaseModel AddNewElement(BaseModel obj);

        /// <summary>
        /// Вернуть элемент по id
        /// </summary>
        /// <param name="id">id элемента</param>
        /// <returns></returns>
        BaseModel GetElementById(long? id);

        /// <summary>
        /// Вернуть все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        ObservableCollection<BaseModel> GetAllElements();

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="obj">обновляемый элемент</param>
        /// <returns></returns>
        bool Update(BaseModel obj);

        /// <summary>
        /// Удалить элемент по id
        /// </summary>
        /// <param name="id">id элемента</param>
        bool DeleteById(long? id);
    }
}
