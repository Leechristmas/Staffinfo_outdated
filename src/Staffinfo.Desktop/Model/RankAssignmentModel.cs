using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Присвоение звания
    /// </summary>
    public class RankAssignmentModel
    {
        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long PreviousRankId { get; set; }
        public long NewRankId { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public DateTime AssignmentDate { get; set; }

        #endregion

    }
}
