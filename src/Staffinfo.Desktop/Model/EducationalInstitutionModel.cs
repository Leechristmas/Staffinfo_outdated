namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель учреждения образования
    /// </summary>
    public class EducationalInstitutionModel: BaseModel
    {
        public EducationalInstitutionModel()
        {
                
        }

        #region Properties
        
        public string InstituitionTitle { get; set; }
        public string Description { get; set; }
        public string InstituitionType { get; set; }

        #endregion
    }
}
