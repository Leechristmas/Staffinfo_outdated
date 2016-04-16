namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Служба
    /// </summary>
    public class ServiceModel: BaseModel
    {
        /// <summary>
        /// Название службы
        /// </summary>
        public string ServiceTitle { get; set; }

        /// <summary>
        /// Id группы служб
        /// </summary>
        public int GroupId { get; set; }
    }
}
