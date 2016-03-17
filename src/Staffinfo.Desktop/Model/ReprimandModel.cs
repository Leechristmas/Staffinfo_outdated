using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Выговор
    /// </summary>
    public class ReprimandModel: BaseModel
    {
        
        #region Properties

        /// <summary>
        /// Код служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Сумма выговора
        /// </summary>
        public decimal ReprimandSum { get; set; }

        /// <summary>
        /// Дата вынесения выговора
        /// </summary>
        public DateTime ReprimandDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion
    
    }
}
