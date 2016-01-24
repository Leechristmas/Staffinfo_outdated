using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель классности служащего
    /// </summary>
    public class ClasinessModel
    {
        public ClasinessModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public ushort OrderNumber { get; set; }
        public DateTime ClasinessDate { get; set; }
        public byte ClasinessLevel { get; set; }
        public string Description { get; set; }

        #endregion
    }
}
