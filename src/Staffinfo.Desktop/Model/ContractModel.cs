using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель контракта
    /// </summary>
    public class ContractModel: BaseModel
    {

        #region Properties
        
        /// <summary>
        /// Id служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Дата подписания контракта
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания контракта
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

    }
}
