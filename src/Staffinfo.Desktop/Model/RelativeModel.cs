using System;

namespace Staffinfo.Desktop.Model
{

    /// <summary>
    /// Родственник
    /// </summary>
    public class RelativeModel
    {
        public RelativeModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long RelationTypeId { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BornDate { get; set; }
        public string Description { get; set; }

        #endregion


    }
}
