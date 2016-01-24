using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Воинская служба
    /// </summary>
    public class MilitaryProcessModel
    {
        public MilitaryProcessModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Descripion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public long MilitaryUnitId { get; set; }

        #endregion

    }
}
