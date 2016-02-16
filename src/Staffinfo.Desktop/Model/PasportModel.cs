namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Паспортные данные
    /// </summary>
    public class PasportModel: BaseModel
    {
        #region Properties

        /// <summary>
        /// Id паспортного стола
        /// </summary>
        public long OrganizationUnitId { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string Series { get; set; }

        #endregion

    }
}