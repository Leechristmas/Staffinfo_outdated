using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель процесса обучения
    /// </summary>
    public class EducationTimeModel: BaseModel
    {

        #region Properties
        
        /// <summary>
        /// Id служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Дата начала обучения
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания обучения
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Код специальности
        /// </summary>
        public long SpecialityId { get; set; }

        /// <summary>
        /// Код учебного заведения
        /// </summary>
        public long InstitutionId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

    }
}
