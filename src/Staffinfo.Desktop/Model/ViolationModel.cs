using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Нарушение
    /// </summary>
    public class ViolationModel
    {
        public ViolationModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime ViolationDate { get; set; }

        #endregion
    }
}
