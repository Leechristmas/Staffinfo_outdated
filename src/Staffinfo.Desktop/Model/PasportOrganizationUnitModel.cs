namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Паспортный стол
    /// </summary>
    public class PasportOrganizationUnitModel: BaseModel
    {
        #region Properties

        /// <summary>
        /// Название организации
        /// </summary>
        public string OrganizationUnitName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        #endregion

    }
}