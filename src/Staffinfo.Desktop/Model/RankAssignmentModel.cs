using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Присвоение звания
    /// </summary>
    public class RankAssignmentModel: BaseModel
    {
        #region Properties
        
        /// <summary>
        /// Код должности
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Код предыдущего звания
        /// </summary>
        public long PreviousRankId { get; set; }

        /// <summary>
        /// Код нового звания
        /// </summary>
        public long NewRankId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Дата присвоения звания
        /// </summary>
        public DateTime AssignmentDate { get; set; }

        #endregion

    }
}
