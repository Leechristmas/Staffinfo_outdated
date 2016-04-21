using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class RankViewModel
    {
        public RankViewModel(RankModel rankModel)
        {
            RankTitle = rankModel.RankTitle;
        } 

        public string RankTitle { get; set; }
        public bool IsSelected { get; set; }
    }
}