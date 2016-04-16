using System;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель учреждения образования
    /// </summary>
    public class EducationalInstitutionModel: BaseModel, IComparable
    {

        #region Properties
        
        /// <summary>
        /// Название учреждения образования
        /// </summary>
        public string InstituitionTitle { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип учебного заведения(ВУЗ,СУЗ и т.д.)
        /// </summary>
        public string InstituitionType { get; set; }

        /// <summary>
        /// Название учебного заведения для view
        /// </summary>
        public string FullName => InstituitionTitle + " - " + InstituitionType;

        #endregion

        public int CompareTo(object obj)
        {
            var educationInstitution = obj as EducationalInstitutionModel;

            if (educationInstitution == null)
                throw new ArgumentException("Передан объект неподходящего типа");

            return String.CompareOrdinal(InstituitionTitle, educationInstitution.InstituitionTitle);
        }
    }
}
