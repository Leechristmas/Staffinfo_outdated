using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Запись из таблицы лога
    /// </summary>
    public class DbLogRecord: BaseModel
    {
        /// <summary>
        /// Id сессии
        /// </summary>
        public long? SessionId { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Время проведения операции
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// Описание операции
        /// </summary>
        public string Description { get; set; }

    }
}