namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Модель звания
    /// </summary>
    public class RankModel: BaseModel
    {
        public RankModel()
        {
                
        }
                    
        public long Id { get;  set; }
        public string RankTitle { get;  set; }
    }
}
