using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Воинская служба
    /// </summary>
    public class MilitaryProcessModel: BaseModel
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
        /// Дата начала несения службы
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания службы
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Код воинской части
        /// </summary>
        public long MilitaryUnitId { get; set; }

        #endregion

    }
}
