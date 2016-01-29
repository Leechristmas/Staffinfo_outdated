using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель контракта
    /// </summary>
    public class ContractModel: BaseModel
    {
        public ContractModel()
        {
                
        }

        #region Properties
        
        public long EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }

        #endregion

    }
}
