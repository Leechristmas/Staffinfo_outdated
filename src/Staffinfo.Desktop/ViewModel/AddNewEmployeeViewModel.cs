using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Staffinfo.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class AddNewEmployeeViewModel: ViewModelBase
    {
        private readonly ListViewModel<RankModel> _rankList;
        private string _personalNumber;

        public AddNewEmployeeViewModel()
        {
            _rankList = new ListViewModel<RankModel>(new List<RankModel>
            {
                new RankModel {Id = 1, RankTitle = "Сержант"},
                new RankModel {Id = 2, RankTitle = "Лейтенант"},
                new RankModel {Id = 3, RankTitle = "Капитан"}
            });
        }

        public string PersonalNumber
        {
            get { return _personalNumber; }
            set
            {
                _personalNumber = value;
                RaisePropertyChanged("PersonalNumber");
            }
        }

        public ListViewModel<RankModel> RankList
        {
            get { return _rankList; }
        } 
    }
}
