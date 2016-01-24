using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Выговор
    /// </summary>
    public class ReprimandModel
    {
        public ReprimandModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public decimal ReprimandSum { get; set; }
        public DateTime ReprimandDate { get; set; }
        public string Description { get; set; }

        #endregion
    
    }
}
