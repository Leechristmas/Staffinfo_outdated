using System;

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
        public ushort OrderNumber { get; set; }

        /// <summary>
        /// Дата подтверждения классности
        /// </summary>
        public DateTime ClasinessDate { get; set; }

        /// <summary>
        /// Уровень классности
        /// </summary>
        public byte ClasinessLevel { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
