using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Sql;
using Staffinfo.Desktop.ViewModel;
using Staffinfo.Desktop.Data.DataTableProviders;
using System.Linq;
using System.Threading.Tasks;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Shared;

namespace Staffinfo.Desktop.Data
{
    /// <summary>
    /// Синглтон
    /// </summary>
    public class DataSingleton
    {
        public DataSingleton()
        { }

        private static DataSingleton _instance;

        public static DataSingleton Instance => _instance ?? (_instance = new DataSingleton());

        public DatabaseConnector DatabaseConnector
        {
            get { return _databaseConnector; }
            set
            {
                if (_databaseConnector == null)
                    _databaseConnector = value;
            }
        }

        /// <summary>
        /// Файл логирования чтения из БД
        /// </summary>
        public static string DataLogFile = "DataLog.log";
        
        /// <summary>
        /// Файл логирования
        /// </summary>
        public static string AppLogFile = "AppLog.log";

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public static string DatabaseName => "STAFFINFO_TESTS";
        #region Fields

        /// <summary>
        /// Служащие
        /// </summary>
        private ObservableCollection<EmployeeViewModel> _employeeList;

        /// <summary>
        /// Должности
        /// </summary>
        private List<PostModel> _postList;

        /// <summary>
        /// Звания
        /// </summary>
        private List<RankModel> _rankList;

        /// <summary>
        /// Службы
        /// </summary>
        private List<ServiceModel> _serviceList;
        
        /// <summary>
        /// Учебные заведения
        /// </summary>
        private ObservableCollection<EducationalInstitutionModel> _educationalInstitutionList;

        /// <summary>
        /// Военная часть
        /// </summary>
        private ObservableCollection<MilitaryUnitModel> _militaryUnitList;
        
        /// <summary>
        /// Специальности
        /// </summary>
        private ObservableCollection<SpecialityModel> _specialityList;

        private DatabaseConnector _databaseConnector;

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает имя файла, хранящего список экземпляров локального сервера
        /// </summary>
        public string ServersFile => "servers.txt";
        
        /// <summary>
        /// Активный пользователь
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Служащий
        /// </summary>
        public ObservableCollection<EmployeeViewModel> EmployeeList => _employeeList ?? (_employeeList = new ObservableCollection<EmployeeViewModel>());

        /// <summary>
        /// Должности
        /// </summary>
        public List<PostModel> PostList => _postList ?? (_postList = new List<PostModel>());

        /// <summary>
        /// Звания
        /// </summary>
        public List<RankModel> RankList => _rankList ?? (_rankList = new List<RankModel>());

        /// <summary>
        /// Службы
        /// </summary>
        public List<ServiceModel> ServiceList => _serviceList ?? (_serviceList = new List<ServiceModel>());

        /// <summary>
        /// Учебные заведения
        /// </summary>
        public ObservableCollection<EducationalInstitutionModel> EducationalInstitutionList
            => _educationalInstitutionList ?? (_educationalInstitutionList = new ObservableCollection<EducationalInstitutionModel>());

        /// <summary>
        /// Военная часть
        /// </summary>
        public ObservableCollection<MilitaryUnitModel> MilitaryUnitList
            => _militaryUnitList ?? (_militaryUnitList = new ObservableCollection<MilitaryUnitModel>());
        
        /// <summary>
        /// Специальности
        /// </summary>
        public ObservableCollection<SpecialityModel> SpecialityList
            => _specialityList ?? (_specialityList = new ObservableCollection<SpecialityModel>());
        #endregion

        #region Methods

        /// <summary>
        /// Подтягивает из БД служебную информацию
        /// </summary>
        public void DataInitialize()
        {
            // Служащие
            using (var prvdr = new EmployeeTableProvider())
            {
                Instance._employeeList =
                    new ObservableCollection<EmployeeViewModel>(prvdr.Select().Select(p => new EmployeeViewModel(p)));
            }

            //Должности
            using (var prvdr = new PostTableProvider())
            {
                Instance._postList = prvdr.Select().ToList();
            }

            //Звания
            using (var prvdr = new RankTableProvider())
            {
                Instance._rankList = prvdr.Select().ToList();
            }

            //Службы
            using (var prvdr = new ServiceTableProvider())
            {
                Instance._serviceList = prvdr.Select().ToList();
            }

            //Учебные заведения
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                Instance._educationalInstitutionList = prvdr.Select();
            }

            //Воинские части
            using (var prvdr = new MilitaryUnitTableProvider())
            {
                Instance._militaryUnitList = prvdr.Select();
            }

            //Специальности
            using (var prvdr = new SpecialityTableProvider())
            {
                Instance._specialityList = prvdr.Select();
            }
        }

        #endregion

    }
}
