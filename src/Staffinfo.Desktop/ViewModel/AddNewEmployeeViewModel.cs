using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для окна добавления служащего
    /// </summary>
    public class AddNewEmployeeViewModel: WindowViewModelBase
    {
        #region Constructor

        public AddNewEmployeeViewModel()
        {
            _rankList = new ListViewModel<RankModel>(DataSingleton.Instance.RankList);
            _postList = new ListViewModel<PostModel>(DataSingleton.Instance.PostList);
            _serviceList = new ListViewModel<ServiceModel>(DataSingleton.Instance.ServiceList);
        }

        #endregion
        
        #region Fields
        /// <summary>
        /// Звания
        /// </summary>
        private readonly ListViewModel<RankModel> _rankList;

        /// <summary>
        /// Должности
        /// </summary>
        private readonly ListViewModel<PostModel> _postList;

        /// <summary>
        /// Службы
        /// </summary>
        private readonly ListViewModel<ServiceModel> _serviceList;

        /// <summary>
        /// Личный номер
        /// </summary>
        private string _personalNumber;

        #endregion

        #region Properties
        /// <summary>
        /// Личный номер
        /// </summary>
        public string PersonalNumber
        {
            get { return _personalNumber; }
            set
            {
                _personalNumber = value;
                RaisePropertyChanged("PersonalNumber");
            }
        }

        /// <summary>
        /// Звания
        /// </summary>
        public ListViewModel<RankModel> RankList => _rankList;

        /// <summary>
        /// Должности
        /// </summary>
        public ListViewModel<PostModel> PostList => _postList;

        /// <summary>
        /// Службы
        /// </summary>
        public ListViewModel<ServiceModel> ServiceList => _serviceList;

        #endregion

        #region Commands
        /// <summary>
        /// Закрыть окно
        /// </summary>
        private RelayCommand _closeWindowCommand;
        public RelayCommand CloseWindowCommand => _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindowExecute));

        private void CloseWindowExecute()
        {
            WindowsClosed = true;
        }

        #endregion
    }
}
