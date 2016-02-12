using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Присвоение должности
    /// </summary>
    public class PostAssignmentModel
    {
        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long PreviousPostId { get; set; }
        public long NewPostId { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public DateTime AssignmentDate { get; set; }

        #endregion

    }
}
