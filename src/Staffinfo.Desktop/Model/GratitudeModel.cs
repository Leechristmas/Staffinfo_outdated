using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель благодарности
    /// </summary>
    public class GratitudeModel
    {
        public GratitudeModel()
        {

        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Description { get; set; }

        public DateTime GratitudeDate { get; set; }
        #endregion

    }
}
