namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель учреждения образования
    /// </summary>
    public class EducationalInstituitionModel
    {
        public EducationalInstituitionModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public string InstituitionTitle { get; set; }
        public string Description { get; set; }
        public string InstituitionType { get; set; }

        #endregion
    }
}
