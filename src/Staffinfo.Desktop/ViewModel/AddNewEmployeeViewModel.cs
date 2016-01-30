using System.Collections.Generic;
using System.Data;
using GalaSoft.MvvmLight;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class AddNewEmployeeViewModel: ViewModelBase
    {
        private readonly ListViewModel<RankModel> _rankList;
        private readonly ListViewModel<PostModel> _postList;
        private readonly ListViewModel<ServiceModel> _serviceList;

        private string _personalNumber;

        public AddNewEmployeeViewModel()
        {
            _rankList = new ListViewModel<RankModel>(DataSingleton.Instance.RankList);
            _postList = new ListViewModel<PostModel>(DataSingleton.Instance.PostList);
            _serviceList = new ListViewModel<ServiceModel>(DataSingleton.Instance.ServiceList);
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

        

        public ListViewModel<RankModel> RankList => _rankList;
        public ListViewModel<PostModel> PostList => _postList;
        public ListViewModel<ServiceModel> ServiceList => _serviceList;

    }
}
