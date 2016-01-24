namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель должности
    /// </summary>
    public class PostModel
    {
        public PostModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public long ServiceId { get; set; }
        public string PostTitle { get; set; }

        #endregion

    }
}
