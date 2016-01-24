using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель больничного
    /// </summary>
    public class HospitalTimeModel
    {
        public HospitalTimeModel()
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
