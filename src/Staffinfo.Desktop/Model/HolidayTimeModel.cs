using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель отпуска
    /// </summary>
    public class HolidayTimeModel
    {
        public HolidayTimeModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        #endregion
    }
}
