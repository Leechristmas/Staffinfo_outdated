using System;
using System.IO;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для редактирования модели служащего
    /// </summary>
    public class EmployeeEditViewModel: WindowViewModelBase
    {
        #region Constructor

        public EmployeeEditViewModel()
        {
            _rankList = new ListViewModel<RankModel>(DataSingleton.Instance.RankList);
            _postList = new ListViewModel<PostModel>(DataSingleton.Instance.PostList);
            _serviceList = new ListViewModel<ServiceModel>(DataSingleton.Instance.ServiceList);
        }

        public EmployeeEditViewModel(EmployeeViewModel employeeViewModel) : this()
        {
            _rankList.SelectedItem = employeeViewModel.Rank;
            _postList.SelectedItem = employeeViewModel.Post;
            _serviceList.SelectedItem = employeeViewModel.Service;

            BornDate = employeeViewModel.BornDate;
            JobStartDate = employeeViewModel.JobStartDate;
            FirstName = employeeViewModel.FirstName;
            LastName = employeeViewModel.LastName;
            MiddleName = employeeViewModel.MiddleName;
            City = employeeViewModel.City;
            Street = employeeViewModel.Street;
            House = employeeViewModel.House;
            Flat = employeeViewModel.Flat;
            HomePhoneNumber = employeeViewModel.HomePhoneNumber;
            MobilePhoneNumber = employeeViewModel.MobilePhoneNumber;
            PasportNumber = employeeViewModel.PasportNumber;
            PasportOrganizationUnit = employeeViewModel.PasportOrganizationUnit;
            PasportSeries = employeeViewModel.PasportSeries;
            PersonalNumber = employeeViewModel.PersonalNumber;
            Photo = employeeViewModel.Photo;

            EmployeeViewModel = employeeViewModel;
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
        /// Фото служащего
        /// </summary>
        private BitmapImage _photo;

        /// <summary>
        /// Фото служащего
        /// </summary>
        public BitmapImage Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                RaisePropertyChanged("Photo");
            }
        }

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
        private DateTime? _bornDate;

        /// <summary>
        /// Дата начала службы в МЧС
        /// </summary>
        private DateTime? _jobStartDate;

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
        /// Серия паспорта
        /// </summary>
        private string _pasportSeries;

        /// <summary>
        /// Организация, выдавшая паспорт
        /// </summary>
        private string _pasportOrganizationUnit;

        /// <summary>
        /// Номер паспорта
        /// </summary>
        private string _pasportNumber;

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        private string _mobilePhoneNumber;

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        private string _homePhoneNumber;

        /// <summary>
        /// Были ли произведены изменения
        /// </summary>
        private bool _isChanged;
        #endregion

        #region Properties

        /// <summary>
        /// Флаг изменений
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }

        /// <summary>
        /// ViewModel служащего, выбранного для просмотра/редактирования
        /// </summary>
        public EmployeeViewModel EmployeeViewModel { get; set; }

        /// <summary>
        /// Личный номер
        /// </summary>
        public string PersonalNumber
        {
            get { return _personalNumber; }
            set
            {
                _personalNumber = value;
                IsChanged = true;
                RaisePropertyChanged("PersonalNumber");
            }
        }

        /// <summary>
        /// Полное имя служащего
        /// </summary>
        public string FullName
        {
            get { return LastName + ' ' + FirstName + ' ' + MiddleName; }
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
        public DateTime? BornDate
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
        public DateTime? JobStartDate
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
        /// Серия паспорта
        /// </summary>
        public string PasportSeries
        {
            get { return _pasportSeries; }
            set
            {
                _pasportSeries = value;
                RaisePropertyChanged("PasportSeries");
            }
        }

        /// <summary>
        /// Организация, выдавшая паспорт
        /// </summary>
        public string PasportOrganizationUnit
        {
            get { return _pasportOrganizationUnit; }
            set
            {
                _pasportOrganizationUnit = value;
                RaisePropertyChanged("PasportOrganizationUnit");
            }
        }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string PasportNumber
        {
            get { return _pasportNumber; }
            set
            {
                _pasportNumber = value;
                RaisePropertyChanged("PasportNumber");
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

        /// <summary>
        /// Принять изменения
        /// </summary>
        private RelayCommand _acceptChanges;
        public RelayCommand AcceptChanges => _acceptChanges ?? (_acceptChanges = new RelayCommand(AcceptChangesExecute));

        public void AcceptChangesExecute()
        {
            //Accept-code
        }

        /// <summary>
        /// Отменить изменения
        /// </summary>
        private RelayCommand _cleanOut;
        public RelayCommand CleanOut => _cleanOut ?? (_cleanOut = new RelayCommand(CleanOutExecute));

        public void CleanOutExecute()
        {
            _rankList.SelectedItem = EmployeeViewModel.Rank;
            _postList.SelectedItem = EmployeeViewModel.Post;
            _serviceList.SelectedItem = EmployeeViewModel.Service;

            BornDate = EmployeeViewModel.BornDate;
            JobStartDate = EmployeeViewModel.JobStartDate;
            FirstName = EmployeeViewModel.FirstName;
            LastName = EmployeeViewModel.LastName;
            MiddleName = EmployeeViewModel.MiddleName;
            City = EmployeeViewModel.City;
            Street = EmployeeViewModel.Street;
            House = EmployeeViewModel.House;
            Flat = EmployeeViewModel.Flat;
            HomePhoneNumber = EmployeeViewModel.HomePhoneNumber;
            MobilePhoneNumber = EmployeeViewModel.MobilePhoneNumber;
            PasportNumber = EmployeeViewModel.PasportNumber;
            PasportOrganizationUnit = EmployeeViewModel.PasportOrganizationUnit;
            PasportSeries = EmployeeViewModel.PasportSeries;
            PersonalNumber = EmployeeViewModel.PersonalNumber;
            Photo = EmployeeViewModel.Photo;

            EmployeeViewModel = EmployeeViewModel;
        }
        #endregion
    }
}