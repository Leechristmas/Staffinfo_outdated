using System;
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
    public class AddNewEmployeeViewModel: WindowViewModelBase
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
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged("LastName");
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
                _firstName = value;
                RaisePropertyChanged("FirstName");
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
                _middleName = value;
                RaisePropertyChanged("MiddleName");
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
        /// Добавить служащего
        /// </summary>
        private RelayCommand _addNewEmployeeCommand;


        public RelayCommand AddNewEmployeeCommand
            => _addNewEmployeeCommand ?? (_addNewEmployeeCommand = new RelayCommand(AddNewEmployeeExecute));

        private void AddNewEmployeeExecute()
        {
            //сохраняем паспорт
            using (var prvdr = new PasportTableProvider())
            {
                Pasport = prvdr.Save(Pasport);
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
                    MessageBox.Show("Не удалось сохранить служащего!" + prvdr.ErrorInfo, "Ошибка!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                else
                    DataSingleton.Instance.EmployeeList.Add(new EmployeeViewModel(employee));
            }
            WindowsClosed = true;
        }
        
        #endregion
    }
}
