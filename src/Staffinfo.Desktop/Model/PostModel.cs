namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель должности
    /// </summary>
    public class PostModel: BaseModel
    {
        
        #region Properties

        public long ServiceId { get; set; }
        public string PostTitle { get; set; }

        #endregion

    }
}
