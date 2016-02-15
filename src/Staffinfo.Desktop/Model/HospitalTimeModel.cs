using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель больничного
    /// </summary>
    public class HospitalTimeModel: BaseModel
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
        /// Дата открытия больничного
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания больничного
        /// </summary>
        public DateTime FinishDate { get; set; }

        #endregion
    }
}
