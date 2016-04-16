using System;
using System.ComponentModel;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Аттестация
    /// </summary>
    public class SertificationModel: BaseModel
    {
        #region Properties

        /// <summary>
        /// Код служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Дата аттестации
        /// </summary>
        public DateTime SertificationDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
