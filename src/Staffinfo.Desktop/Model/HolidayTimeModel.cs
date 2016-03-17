using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель отпуска
    /// </summary>
    public class HolidayTimeModel: BaseModel
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
        /// Дата начала отпуска
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания отпуска
        /// </summary>
        public DateTime FinishDate { get; set; }

        #endregion
    }
}
