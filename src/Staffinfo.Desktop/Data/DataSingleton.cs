using System.Collections.Generic;
using System.Collections.ObjectModel;
using Staffinfo.Desktop.ViewModel;
using Staffinfo.Desktop.Data.DataTableProviders;
using System.Linq;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Data
{
    /// <summary>
    /// Синглтон
    /// </summary>
    public class DataSingleton
    {
        private static DataSingleton _instance;

        private DataSingleton()
        {
            DatabaseConnector = new DatabaseConnector();
        }

        public static DataSingleton Instance => _instance ?? (_instance = new DataSingleton());

        public DatabaseConnector DatabaseConnector { get; }

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
        /// Классность
        /// </summary>
        private List<ClasinessModel> _clasinessList;

        /// <summary>
        /// Контракты
        /// </summary>
        private List<ContractModel> _contactList;

        /// <summary>
        /// Процесс обучения
        /// </summary>
        private List<EducationTimeModel> _educationTimeList;
        
        /// <summary>
        /// Учебные заведения
        /// </summary>
        private List<EducationalInstitutionModel> _educationalInstitutionList;

        /// <summary>
        /// Вынесения благодарности
        /// </summary>
        private List<GratitudeModel> _gratitudeList;

        /// <summary>
        /// Отпуска
        /// </summary>
        private List<HolidayTimeModel> _holidayTimeList;

        /// <summary>
        /// Больничные
        /// </summary>
        private List<HospitalTimeModel> _hospitalTimeList;

        /// <summary>
        /// Прохождение службы
        /// </summary>
        private List<MilitaryProcessModel> _militaryProcessList;

        /// <summary>
        /// Военная часть
        /// </summary>
        private List<MilitaryUnitModel> _militaryUnitList;

        /// <summary>
        /// Присвоения должностей
        /// </summary>
        private List<PostAssignmentModel> _postAssignmentList;

        /// <summary>
        /// Присвоения званий
        /// </summary>
        private List<RankAssignmentModel> _rankAssignmentList;

        /// <summary>
        /// Родственники
        /// </summary>
        private List<RelativeModel> _relativeList;

        /// <summary>
        /// Тип родства
        /// </summary>
        private List<RelativeTypeModel> _relativeTypeList;

        /// <summary>
        /// Выговоры
        /// </summary>
        private List<ReprimandModel> _reprimandList;

        /// <summary>
        /// Аттестация
        /// </summary>
        private List<SertificationModel> _sertificationList;

        /// <summary>
        /// Специальности
        /// </summary>
        private List<SpecialityModel> _specialityList;

        /// <summary>
        /// Нарушения
        /// </summary>
        private List<ViolationModel> _violationList;
        
        #endregion



        #region Properties

        /// <summary>
        /// Пользователь
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
        /// Классность
        /// </summary>
        public List<ClasinessModel> ClasinessList => _clasinessList ?? (_clasinessList = new List<ClasinessModel>());

        /// <summary>
        /// Контракты
        /// </summary>
        public List<ContractModel> ContractList => _contactList ?? (_contactList = new List<ContractModel>());

        /// <summary>
        /// Процесс обучения
        /// </summary>
        public List<EducationTimeModel> EducationTimeList
            => _educationTimeList ?? (_educationTimeList = new List<EducationTimeModel>());

        /// <summary>
        /// Учебные заведения
        /// </summary>
        public List<EducationalInstitutionModel> EducationalInstitutionList
            => _educationalInstitutionList ?? (_educationalInstitutionList = new List<EducationalInstitutionModel>());

        /// <summary>
        /// Вынесения благодарности
        /// </summary>
        public List<GratitudeModel> GratitudeList => _gratitudeList ?? (_gratitudeList = new List<GratitudeModel>());

        /// <summary>
        /// Отпуска
        /// </summary>
        public List<HolidayTimeModel> HolidayTimeList
            => _holidayTimeList ?? (_holidayTimeList = new List<HolidayTimeModel>());

        /// <summary>
        /// Больничные
        /// </summary>
        public List<HospitalTimeModel> HospitalTimeList
            => _hospitalTimeList ?? (_hospitalTimeList = new List<HospitalTimeModel>());

        /// <summary>
        /// Прохождение службы
        /// </summary>
        public List<MilitaryProcessModel> MilitaryProcessList
            => _militaryProcessList ?? (_militaryProcessList = new List<MilitaryProcessModel>());

        /// <summary>
        /// Военная часть
        /// </summary>
        public List<MilitaryUnitModel> MilitaryUnitList
            => _militaryUnitList ?? (_militaryUnitList = new List<MilitaryUnitModel>());

        /// <summary>
        /// Присвоения должностей
        /// </summary>
        public List<PostAssignmentModel> PostAssignmentList
            => _postAssignmentList ?? (_postAssignmentList = new List<PostAssignmentModel>());

        /// <summary>
        /// Присвоения званий
        /// </summary>
        public List<RankAssignmentModel> RankAssignmentList
            => _rankAssignmentList ?? (_rankAssignmentList = new List<RankAssignmentModel>());

        /// <summary>
        /// Родственники
        /// </summary>
        public List<RelativeModel> RelativeList
            => _relativeList ?? (_relativeList = new List<RelativeModel>());

        /// <summary>
        /// Типы родства
        /// </summary>
        public List<RelativeTypeModel> RelativeTypeList
            => _relativeTypeList ?? (_relativeTypeList = new List<RelativeTypeModel>());

        /// <summary>
        /// Выговоры
        /// </summary>
        public List<ReprimandModel> ReprimandList => _reprimandList ?? (_reprimandList = new List<ReprimandModel>());

        /// <summary>
        /// Аттестация
        /// </summary>
        public List<SertificationModel> SertificationList
            => _sertificationList ?? (_sertificationList = new List<SertificationModel>());

        /// <summary>
        /// Специальности
        /// </summary>
        public List<SpecialityModel> SpecialityList
            => _specialityList ?? (_specialityList = new List<SpecialityModel>());

        /// <summary>
        /// Нарушения
        /// </summary>
        public List<ViolationModel> ViolationList
            => _violationList ?? (_violationList = new List<ViolationModel>()); 
        
        #endregion

        /// <summary>
        /// Подтягивает все данные из БД
        /// </summary>
        public void DataInitialize()
        {
            // Служащие
            using(var prvdr = new EmployeeTableProvider())
            {
                Instance._employeeList = new ObservableCollection<EmployeeViewModel>(prvdr.Select().Select(p => new EmployeeViewModel(p)));
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

            //Классность
            using (var prvdr = new ClasinessTableProvider())
            {
                Instance._clasinessList = prvdr.Select().ToList();
            }

            //Контракты
            using (var prvdr = new ContractTableProvider())
            {
                Instance._contactList = prvdr.Select().ToList();
            }

            //Процессы обучения
            using (var prvdr = new EducationTimeTableProvider())
            {
                Instance._educationTimeList = prvdr.Select().ToList();
            }

            //Учебные заведения
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                Instance._educationalInstitutionList = prvdr.Select().ToList();
            }

            //Присвоения благодарностей
            using (var prvdr = new GratitudeTableProvider())
            {
                Instance._gratitudeList = prvdr.Select().ToList();
            }

            //Отпуска
            using (var prvdr = new HolidayTimeTableProvider())
            {
                Instance._holidayTimeList = prvdr.Select().ToList();
            }

            //Больничные
            using (var prvdr = new HospitalTimeTableProvider())
            {
                Instance._hospitalTimeList = prvdr.Select().ToList();
            }

            //Прохождения службы
            using (var prvdr = new MilitaryProcessTableProvider())
            {
                Instance._militaryProcessList = prvdr.Select().ToList();
            }
            
            //Военные части
            using (var prvdr = new MilitaryUnitTableProvider())
            {
                Instance._militaryUnitList = prvdr.Select().ToList();
            }

            //Присвоения должностей
            using (var prvdr = new PostAssignmentTableProvider())
            {
                Instance._postAssignmentList = prvdr.Select().ToList();
            }

            //Присвоения званий
            using (var prvdr = new RankAssignmentTableProvider())
            {
                Instance._rankAssignmentList = prvdr.Select().ToList();
            }

            //Родственники
            using (var prvdr = new RelativeTableProvider())
            {
                Instance._relativeList = prvdr.Select().ToList();
            }

            //Типы родства
            using (var prvdr = new RelativeTypeTableProvider())
            {
                Instance._relativeTypeList = prvdr.Select().ToList();
            }

            //Выговоры
            using (var prvdr = new ReprimandTableProvider())
            {
                Instance._reprimandList = prvdr.Select().ToList();
            }

            //Аттестация
            using (var prvdr = new SertificationTableProvider())
            {
                Instance._sertificationList = prvdr.Select().ToList();
            }

            //Специальности
            using (var prvdr = new SpecialityTableProvider())
            {
                Instance._specialityList = prvdr.Select().ToList();
            }

            //Нарушения
            using (var prvdr = new ViolationTableProvider())
            {
                Instance._violationList = prvdr.Select().ToList();
            }
        }
    }
}
