namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель должности
    /// </summary>
    public class PostModel: BaseModel
    {
        public PostModel()
        {
                
        }
        
        #region Properties

        public long ServiceId { get; set; }
        public string PostTitle { get; set; }

        #endregion

    }
}
