namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель звания
    /// </summary>
    public class RankModel: BaseModel
    {
        /// <summary>
        /// Звание
        /// </summary>
        public string RankTitle { get;  set; }

        /// <summary>
        /// "Вес" звания
        /// </summary>
        public int RankWeight { get; set; }

        /// <summary>
        /// Период выслуги звания (в годах)
        /// </summary>
        public double Period { get; set; }
    }
}
