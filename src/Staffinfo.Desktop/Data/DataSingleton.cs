using System.Collections.Generic;
using System.Collections.ObjectModel;
using Staffinfo.Desktop.ViewModel;
using Staffinfo.Desktop.Data.DataTableProviders;
using System.Linq;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Data
{
    public class DataSingleton
    {
        private static DataSingleton _instance;
        private DatabaseConnector _databaseConnector;

        private DataSingleton()
        {
            _databaseConnector = new DatabaseConnector();
        }

        public static DataSingleton Instance => _instance ?? (_instance = new DataSingleton());

        public DatabaseConnector DatabaseConnector
        {
            get { return _databaseConnector; }
        }

        #region EmployeeList
        private ObservableCollection<EmployeeViewModel> _employeeList;

        public ObservableCollection<EmployeeViewModel> EmployeeList
        {
            get { return _employeeList ?? (_employeeList = new ObservableCollection<EmployeeViewModel>()); }
        }
        #endregion

        #region PostList

        private List<PostModel> _postList;

        public List<PostModel> PostList
        {
            get { return _postList ?? (_postList = new List<PostModel>()); }
        }

        #endregion

        #region RankList

        private List<RankModel> _rankList;

        public List<RankModel> RankList
        {
            get { return _rankList ?? (_rankList = new List<RankModel>()); }
        }

        #endregion

        #region ServiceList

        private List<ServiceModel> _serviceList;

        public List<ServiceModel> ServiceList
        {
            get { return _serviceList ?? (_serviceList = new List<ServiceModel>()); }
        }

        #endregion


        /// <summary>
        /// Подтягивает все данные из БД
        /// </summary>
        public void DataInitialize()
        {
            // Служащие
            using(var prvdr = new EmployeeTableProvider())
            {
                _instance._employeeList = new ObservableCollection<EmployeeViewModel>(prvdr.GetAllElements().Select(p => new EmployeeViewModel(p as EmployeeModel)));
            }

            //Должности
            using (var prvdr = new PostTableProvider())
            {
                _instance._postList = prvdr.GetAllElements().Select(p => p as PostModel).ToList();
            }

            //Звания
            using (var prvdr = new RankTableProvider())
            {
                _instance._rankList = prvdr.GetAllElements().Select(p => p as RankModel).ToList();
            }
            
            //Службы
            using (var prvdr = new ServiceTableProvider())
            {
                _instance._serviceList = prvdr.GetAllElements().Select(p => p as ServiceModel).ToList();
            } 
        }
    }
}
