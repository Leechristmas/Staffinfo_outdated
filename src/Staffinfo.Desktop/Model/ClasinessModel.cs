using System;
using System.ComponentModel;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель классности служащего
    /// </summary>
    public class ClasinessModel: BaseModel
    {
        #region Properties

        /// <summary>
        /// Id служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        [DisplayName("Номер приказа")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Дата подтверждения классности
        /// </summary>
        [DisplayName("Дата")]
        public DateTime ClasinessDate { get; set; }

        /// <summary>
        /// Уровень классности
        /// </summary>
        [DisplayName("Уровень классности")]
        public int ClasinessLevel { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Дополнительно")]
        public string Description { get; set; }

        #endregion
    }
}
