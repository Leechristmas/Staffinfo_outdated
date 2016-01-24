namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Воинская часть
    /// </summary>
    public class MilitaryUnitModel
    {
        public MilitaryUnitModel()
        {
                
        }

        #region Properties

        public long Id { get; set; }
        public string MilitaryName { get; set; }
        public string Description { get; set; }

        #endregion

    }
}
