namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель должности
    /// </summary>
    public class PostModel: BaseModel
    {
        
        #region Properties
        /// <summary>
        /// Id службы
        /// </summary>
        public long ServiceId { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string PostTitle { get; set; }

        /// <summary>
        /// "Вес" должности
        /// </summary>
        public int PostWeight { get; set; }

        #endregion

    }
}
