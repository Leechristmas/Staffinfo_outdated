using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Нарушение
    /// </summary>
    public class ViolationModel: BaseModel
    {
        #region Properties

        /// <summary>
        /// Код служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата нарушения
        /// </summary>
        public DateTime ViolationDate { get; set; }

        #endregion
    }
}
