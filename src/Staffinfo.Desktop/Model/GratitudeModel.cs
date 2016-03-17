using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель благодарности
    /// </summary>
    public class GratitudeModel: BaseModel
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
        /// Дата вынесения благодарности
        /// </summary>
        public DateTime GratitudeDate { get; set; }

        #endregion

    }
}
