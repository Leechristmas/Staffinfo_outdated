namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель учреждения образования
    /// </summary>
    public class EducationalInstitutionModel: BaseModel
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

        #endregion
    }
}
