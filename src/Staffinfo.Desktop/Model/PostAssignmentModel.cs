using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Присвоение должности
    /// </summary>
    public class PostAssignmentModel: BaseModel
    {
        #region Properties
        
        /// <summary>
        /// Код служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Код предыдущей должности
        /// </summary>
        public long PreviousPostId { get; set; }

        /// <summary>
        /// Код новой должности
        /// </summary>
        public long NewPostId { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата присвоения должности
        /// </summary>
        public DateTime AssignmentDate { get; set; }

        #endregion

    }
}
