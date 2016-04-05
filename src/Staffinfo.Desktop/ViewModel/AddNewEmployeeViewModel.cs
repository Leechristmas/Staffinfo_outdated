using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.View;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для окна добавления служащего
    /// </summary>
    public class AddNewEmployeeViewModel : WindowViewModelBase, IDataErrorInfo
    {
        #region Constructor

        public AddNewEmployeeViewModel()
        {
            BornDate = DateTime.Now.AddYears(-18);  //минимальный возраст служащего - 18 лет
            JobStartDate = DateTime.Now;

            _serviceList = new ListViewModel<ServiceModel>(DataSingleton.Instance.ServiceList);
            _rankList = new ListViewModel<RankModel>(DataSingleton.Instance.RankList);
            _postList = new ListViewModel<PostModel>(DataSingleton.Instance.PostList);
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

        /// <summary>
        /// Фамилия
        /// </summary>
        private string _lastName;

        /// <summary>
        /// Имя
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Отчество
        /// </summary>
        private string _middleName;

        /// <summary>
        /// Дата рождения
        /// </summary>
        private DateTime _bornDate;

        /// <summary>
        /// Дата начала службы в МЧС
        /// </summary>
        private DateTime _jobStartDate;

        /// <summary>
        /// Город
        /// </summary>
        private string _city;

        /// <summary>
        /// Улица
        /// </summary>
        private string _street;

        /// <summary>
        /// Дом
        /// </summary>
        private string _house;

        /// <summary>
        /// Квартира
        /// </summary>
        private string _flat;

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        private string _mobilePhoneNumber;

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        private string _homePhoneNumber;

        /// <summary>
        /// Паспорт
        /// </summary>
        private PasportModel _pasport = new PasportModel();

        /// <summary>
        /// Выбранная должность
        /// </summary>
        private PostModel _selectedPost;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        private string _error = String.Empty;
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
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value == "" ? null : value;
                RaisePropertyChanged("LastName");
                var val = Validate;
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value == "" ? null : value;
                RaisePropertyChanged("FirstName");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value == "" ? null : value;
                RaisePropertyChanged("MiddleName");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BornDate
        {
            get { return _bornDate; }
            set
            {
                _bornDate = value;
                RaisePropertyChanged("BornDate");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Дата начала работы
        /// </summary>
        public DateTime JobStartDate
        {
            get { return _jobStartDate; }
            set
            {
                _jobStartDate = value;
                RaisePropertyChanged("JobStartDate");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Город
        /// </summary>
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged("City");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                RaisePropertyChanged("Street");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Дом
        /// </summary>
        public string House
        {
            get { return _house; }
            set
            {
                _house = value;
                RaisePropertyChanged("House");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat
        {
            get { return _flat; }
            set
            {
                _flat = value;
                RaisePropertyChanged("Flat");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Паспорт
        /// </summary>
        public PasportModel Pasport
        {
            get { return _pasport ?? (_pasport = new PasportModel()); }
            set
            {
                _pasport = value;
                RaisePropertyChanged("Pasport");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Выбранная серия паспорта
        /// </summary>
        public string SelectedPasportSeries
        {
            get { return Pasport.Series; }
            set
            {
                Pasport.Series = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Pasport");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string SelectedPasportNumber
        {
            get { return Pasport.Number; }
            set
            {
                Pasport.Number = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Pasport");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Организация, выдававшая паспорт
        /// </summary>
        public string SelectedPasportOrganization
        {
            get { return Pasport.OrganizationUnit; }
            set
            {
                Pasport.OrganizationUnit = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Pasport");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string MobilePhoneNumber
        {
            get { return _mobilePhoneNumber; }
            set
            {
                _mobilePhoneNumber = value;
                RaisePropertyChanged("MobilePhoneNumber");
            }
        }

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        public string HomePhoneNumber
        {
            get { return _homePhoneNumber; }
            set
            {
                _homePhoneNumber = value;
                RaisePropertyChanged("HomePhoneNumber");
            }
        }

        /// <summary>
        /// Выбранная служба
        /// </summary>
        public ServiceModel SelectedService
        {
            get { return ServiceList.SelectedItem; }
            set
            {
                ServiceList.SelectedItem = value;
                RaisePropertyChanged();
                RaisePropertyChanged("PostList");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Выбранное звание
        /// </summary>
        public RankModel SelectedRank
        {
            get { return RankList.SelectedItem; }
            set
            {
                RankList.SelectedItem = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RankList");
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Выбранная должность 
        /// P.S. кривая реализация т.к. нужна селекция должностей по выбранной службе()
        /// </summary>
        public PostModel SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                _selectedPost = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Validate");
                RaisePropertyChanged("AddNewEmployeeCommand");
            }
        }

        /// <summary>
        /// Звания
        /// </summary>
        public ListViewModel<RankModel> RankList => _rankList;

        /// <summary>
        /// Должности
        /// </summary>
        public ListViewModel<PostModel> PostList => new ListViewModel<PostModel>(_postList.ModelList.Where(post => post.ServiceId == SelectedService?.Id).ToList());

        /// <summary>
        /// Службы
        /// </summary>
        public ListViewModel<ServiceModel> ServiceList => _serviceList;

        #endregion

        #region Commands

        /// <summary>
        /// Добавить служащего TODO:validation
        /// </summary>
        private RelayCommand _addNewEmployeeCommand;
        public RelayCommand AddNewEmployeeCommand
            => _addNewEmployeeCommand ?? (_addNewEmployeeCommand = new RelayCommand(AddNewEmployeeExecute));

        private void AddNewEmployeeExecute()
        {
            //блокируем интерфейс
            ViewIsEnable = false;   

            //валидация
            if (!Validate)
            {
                ViewIsEnable = true;
                return;
            }
            
            //сохраняем паспорт
            using (var prvdr = new PasportTableProvider())
            {
                if ((Pasport = prvdr.Save(Pasport)) == null)
                {
                    MessageBox.Show("Не удалось сохранить паспортные данные! " + prvdr.ErrorInfo);
                    return;
                }
            }

            //сохраняем самого служащего
            using (var prvdr = new EmployeeTableProvider())
            {
                var post = SelectedPost;
                var rank = RankList.SelectedItem;

                var employee = new EmployeeModel
                {
                    LastName = LastName,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    PersonalNumber = PersonalNumber,
                    PostId = post.Id,
                    RankId = rank.Id,
                    BornDate = BornDate,
                    JobStartDate = JobStartDate,
                    City = City,
                    Street = Street,
                    House = House,
                    Flat = Flat,
                    PasportId = Pasport.Id,
                    MobilePhoneNumber = MobilePhoneNumber,
                    HomePhoneNumber = HomePhoneNumber
                };

                employee = prvdr.Save(employee);

                if (employee == null)
                {
                    using (var pasportPrvdr = new PasportTableProvider())
                    {
                        pasportPrvdr.DeleteById(Pasport.Id);
                    }
                    MessageBox.Show("Не удалось сохранить служащего!" + prvdr.ErrorInfo, "Ошибка!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                    
                else
                    DataSingleton.Instance.EmployeeList.Add(new EmployeeViewModel(employee));
            }
            
            //открываем интерфейс
            ViewIsEnable = true;

            //закрываем окно
            WindowsClosed = true;
        }

        #endregion

        #region IDataErrorInfo implementation

        /// <summary>
        /// Валидация ввода
        /// TODO: номера телефонов
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName))
                            error = "Не указано имя сотрудника";
                        if (FirstName?.Length > 20)
                            error = "Длина имени не должна превышать 20 символов";
                        break;
                    case "LastName":
                        if (string.IsNullOrEmpty(LastName))
                            error = "Не указана фамилия сотрудника";
                        if (LastName?.Length > 40)
                            error = "Длина фамилии не должна превышать 40 символов";
                        break;
                    case "MiddleName":
                        if (string.IsNullOrEmpty(MiddleName))
                            error = "Не указана фамилия сотрудника";
                        if (MiddleName?.Length > 40)
                            error = "Длина фамилии не должна превышать 40 символов";
                        break;
                    case "PersonalNumber":
                        if (string.IsNullOrEmpty(PersonalNumber))
                            error = "Не указан личный номер сотрудника";
                        if (PersonalNumber?.Length != 7)
                            error = "Длина личного номера - 7 символов";
                        break;
                    case "BornDate":
                        if (BornDate > DateTime.Now.AddYears(-18))
                            error = "Сотрудник слишком молод";
                        if (BornDate < DateTime.Now.AddYears(-65))
                            error = "Сотрудник староват";
                        break;
                    case "JobStartDate":
                        if (JobStartDate > DateTime.Now || JobStartDate < DateTime.Now.AddYears(-18))
                            error = "Дата указана некорректно";
                        break;
                    case "City":
                        if (string.IsNullOrEmpty(City))
                            error = "Не указан город проживания";
                        if (City?.Length > 40)
                            error = "Длина названия города не должна превышать 40 символов";
                        break;
                    case "Street":
                        if (string.IsNullOrEmpty(Street))
                            error = "Не указана улица проживания";
                        if (Street?.Length > 40)
                            error = "Длина названия улицы не должна превышать 40 символов";
                        break;
                    case "House":
                        if (string.IsNullOrEmpty(House))
                            error = "Не указан номер дома";
                        int a;
                        if (!Int32.TryParse(House, out a) || a <= 0)
                            error = "Номер дома указан неверно";
                        break;
                    case "SelectedPost":
                        if (SelectedPost == null)
                            error = "Не указана должность";
                        break;
                    case "SelectedService":
                        if (SelectedService == null)
                            error = "Не указана служба";
                        break;
                    case "SelectedRank":
                        if (SelectedRank == null)
                            error = "Не указано звание";
                        break;
                    case "SelectedPasportSeries":
                        if (SelectedPasportSeries == null)
                            error = "Не указана серия";
                        break;
                    case "SelectedPasportNumber":
                        if (string.IsNullOrEmpty(SelectedPasportNumber))
                            error = "Не указан номер паспорта сотрудника";
                        if (SelectedPasportNumber?.Length != 7)
                            error = "Длина номера паспорта - 7 символов";
                        break;
                    case "SelectedPasportOrganization":
                        if (string.IsNullOrEmpty(SelectedPasportOrganization))
                            error = "Не указана организация, выдавшая паспорт";
                        if (SelectedPasportOrganization?.Length > 100)
                            error = "Длина названия организации не должна превышать 100 символов";
                        break;
                }
                Error = error;
                return error;
            }
        }
        
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Валидация view (через индексатор, который возвращает текст ошибки)
        /// </summary>
        /// <returns></returns>
        protected override bool Validate
        {
            get
            {
                if (this[nameof(LastName)] != "") return false;
                if (this[nameof(FirstName)] != "") return false;
                if (this[nameof(MiddleName)] != "") return false;
                if (this[nameof(SelectedPost)] != "") return false;
                if (this[nameof(SelectedRank)] != "") return false;
                if (this[nameof(SelectedService)] != "") return false;
                if (this[nameof(BornDate)] != "") return false;
                if (this[nameof(JobStartDate)] != "") return false;
                if (this[nameof(PersonalNumber)] != "") return false;
                if (this[nameof(City)] != "") return false;
                if (this[nameof(Street)] != "") return false;
                if (this[nameof(House)] != "") return false;
                if (this[nameof(Flat)] != "") return false;
                if (this[nameof(SelectedPasportSeries)] != "") return false;
                if (this[nameof(SelectedPasportNumber)] != "") return false;
                return this[nameof(SelectedPasportOrganization)] == "";
            }
        }
        //this[nameof(LastName)] != "" &&
        //this[nameof(FirstName)] != "" &&
        //this[nameof(MiddleName)] != "" && 
        //this[nameof(SelectedPost)] != "" && 
        //this[nameof(SelectedRank)] != "" && 
        //this[nameof(SelectedService)] != "" && 
        //this[nameof(BornDate)] != "" && 
        //this[nameof(JobStartDate)] != "" && 
        //this[nameof(City)] != "" && 
        //this[nameof(Street)] != "" && 
        //this[nameof(House)] != "" && 
        //this[nameof(Flat)] != "" && 
        //this[nameof(SelectedPasportSeries)] != "" && 
        //this[nameof(SelectedPasportNumber)] != "" && 
        //this[nameof(SelectedPasportOrganization)] != "";
    }
}
