using System;
using System.Collections.ObjectModel;

namespace Staffinfo.Desktop.Data.DataTableContracts
{
    /// <summary>
    /// Компонент для работы со справочниками(чтение и запись)
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IWritableDirectoryTableContract<T>: IWritableTableContract<T>
    {
        /// <summary>
        /// Получить информацию из справочника по id служащего
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ObservableCollection<T> SelectByEmployeeId(long? id);
    }
}