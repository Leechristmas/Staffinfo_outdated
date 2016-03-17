namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Воинская часть
    /// </summary>
    public class MilitaryUnitModel: BaseModel
    {

        #region Properties
        
        /// <summary>
        /// Название(номер) части
        /// </summary>
        public string MilitaryName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

    }
}
