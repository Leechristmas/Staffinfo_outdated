using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель процесса обучения
    /// </summary>
    public class EducationTimeModel: BaseModel
    {
        public EducationTimeModel()
        {
                
        }

        #region Properties
        
        public long EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public long SpecialityId { get; set; }
        public long InstitutionId { get; set; }
        public string Description { get; set; }

        #endregion

    }
}
