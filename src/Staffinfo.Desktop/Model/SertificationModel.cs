using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Аттестация
    /// </summary>
    public class SertificationModel
    {
        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime SertificationDate { get; set; }
        public string Description { get; set; }

        #endregion
    }
}
