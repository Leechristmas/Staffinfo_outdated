namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Отчет
    /// </summary>
    public class Report: BaseModel
    {
        /// <summary>
        /// Название отчета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public string Description { get; set; }
    }
}