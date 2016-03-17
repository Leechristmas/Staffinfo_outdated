using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Helpers;
using Staffinfo.Desktop.Model;


namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для редактирования модели служащего
    /// </summary>
    public class EmployeeEditViewModel : WindowViewModelBase
    {
        #region Constructor

        public EmployeeEditViewModel()
        {
            //подтягиваем список званий
            _rankList = new ListViewModel<RankModel>(DataSingleton.Instance.RankList);
            
            //список должностей
            _postList = new ListViewModel<PostModel>(DataSingleton.Instance.PostList);

            //список служб
            _serviceList = new ListViewModel<ServiceModel>(DataSingleton.Instance.ServiceList);

            //Определяем уровень доступа пользователя, вошедвшего в систему
            AccessLevel = DataSingleton.Instance.User.AccessLevel;
        }

        public EmployeeEditViewModel(EmployeeViewModel employeeViewModel) : this()
        {
            EmployeeViewModel = employeeViewModel;

            _rankList.SelectedItem = employeeViewModel.Rank;
            //_postList.SelectedItem = employeeViewModel.Post;
            SelectedPost = employeeViewModel.Post;
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
            Pasport = GetPasport(employeeViewModel.PasportId);
            PersonalNumber = employeeViewModel.PersonalNumber;
            Photo = employeeViewModel.Photo;

        }
        #endregion

        #region Fields

        /// <summary>
        /// Индекс активного листа справочника
        /// </summary>
        private int _selectedIndex = -1;

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
        /// Тип даныых, которые будут отображаться в grid'e
        /// </summary>
        private List<string> _informationModeList;
        
        /// <summary>
        /// Фото служащего
        /// </summary>
        private BitmapImage _photo;

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
        private PasportModel _pasport;

        /// <summary>
        /// Были ли произведены изменения
        /// </summary>
        private bool _wasChanged;

        /// <summary>
        /// Коллекция справочников
        /// </summary>
        private CatalogObservableCollectionsList _catalogList;

        /// <summary>
        /// Индекс выбранного "таба"
        /// </summary>
        private int _selectedTabIndex;

        /// <summary>
        /// Выбранная должность
        /// </summary>
        private PostModel _selectedPost;
        #endregion

        #region Properties

        /// <summary>
        /// Индекс выбранного "таба"
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TabsToogleTitle");
            }
        }

        /// <summary>
        /// Флаг изменений
        /// </summary>
        public bool WasChanged
        {
            get { return _wasChanged; }
            set
            {
                _wasChanged = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ViewModel служащего, выбранного для просмотра/редактирования
        /// </summary>
        private EmployeeViewModel EmployeeViewModel { get; set; }

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

                WasChanged = (_personalNumber != EmployeeViewModel.PersonalNumber);
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

                WasChanged = (_lastName != EmployeeViewModel.LastName);
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

                WasChanged = (_firstName != EmployeeViewModel.FirstName);
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

                WasChanged = (_middleName != EmployeeViewModel.MiddleName);
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

                WasChanged = (_bornDate != EmployeeViewModel.BornDate);
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

                WasChanged = (_jobStartDate != EmployeeViewModel.JobStartDate);
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

                WasChanged = (_city != EmployeeViewModel.City);
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

                WasChanged = (_street != EmployeeViewModel.Street);
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

                WasChanged = (_house != EmployeeViewModel.House);
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

                WasChanged = (_flat != EmployeeViewModel.Flat);
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

                WasChanged = (_mobilePhoneNumber != EmployeeViewModel.MobilePhoneNumber);
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

                WasChanged = _homePhoneNumber != EmployeeViewModel.HomePhoneNumber;
            }
        }

        /// <summary>
        /// Фото служащего
        /// </summary>
        public BitmapImage Photo
        {
            get
            {
                return _photo ??
                     (new BitmapImage(
                             new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                 "Resources/Images/empty_avatar_100x100.jpg"))));
            }
            set
            {
                _photo = value;
                RaisePropertyChanged("Photo");

                WasChanged = !BitmapImageHelper.ImageCompare(_photo, EmployeeViewModel.Photo);
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

        /// <summary>
        /// Content кнопки-переключателя между табами
        /// </summary>
        public string TabsToogleTitle => SelectedTabIndex == 0 ? "Добавить" : "Назад";

        /// <summary>
        /// Индекс активного справочника
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedItem");
            }
        }

        /// <summary>
        /// Аттестация
        /// </summary>
        public ObservableCollection<SertificationModel> Sertifications { get; set; }

        /// <summary>
        /// Больничные
        /// </summary>
        public ObservableCollection<HospitalTimeModel> HospitalTimes { get; set; }

        /// <summary>
        /// Благодарности
        /// </summary>
        public ObservableCollection<GratitudeModel> Gratitudes { get; set; }

        /// <summary>
        /// Выговоры
        /// </summary>
        public ObservableCollection<ReprimandModel> Reprimands { get; set; }

        /// <summary>
        /// Несение службы
        /// </summary>
        public ObservableCollection<MilitaryProcessModel> MilitaryProcesses { get; set; }

        /// <summary>
        /// Классности
        /// </summary>
        public ObservableCollection<ClasinessModel> Clasiness { get; set; }

        /// <summary>
        /// Контракты
        /// </summary>
        public ObservableCollection<ContractModel> Contracts { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        public ObservableCollection<ViolationModel> Violations { get; set; }

        /// <summary>
        /// Обучения
        /// </summary>
        public ObservableCollection<EducationTimeModel> EducationTimes { get; set; } 

        /// <summary>
        /// Отпуска
        /// </summary>
        public ObservableCollection<HolidayTimeModel> HolidayTimes { get; set; }

        /// <summary>
        /// Присвоения должностей
        /// </summary>
        public ObservableCollection<PostAssignmentModel> PostAssignments { get; set; }

        /// <summary>
        /// Присвоение званий
        /// </summary>
        public ObservableCollection<RankAssignmentModel> RankAssignments { get; set; }

        /// <summary>
        /// Родственники
        /// </summary>
        public ObservableCollection<RelativeModel> Relatives { get; set; }

        /// <summary>
        /// Активный справочник
        /// </summary>
        public object SelectedItem  //Очередной гребанный костыль...стоит запилить что-то получше.
        {
            get
            {
                if (SelectedIndex < 0) return null;
                switch (SelectedIndex)
                {
                    case 0:
                        using (var prvdr = new SertificationTableProvider())
                        {
                            return Sertifications ?? (Sertifications = new ObservableCollection<SertificationModel>(prvdr.Select()));
                        }
                    case 1:
                        using (var prvdr = new GratitudeTableProvider())
                        {
                            return Gratitudes ?? (Gratitudes = new ObservableCollection<GratitudeModel>(prvdr.Select()));
                        }
                    case 2:
                        using (var prvdr = new HospitalTimeTableProvider())
                        {
                            return HospitalTimes ?? (HospitalTimes = new ObservableCollection<HospitalTimeModel>(prvdr.Select()));
                        }
                    case 3:
                        using (var prvdr = new ReprimandTableProvider())
                        {
                            return Reprimands ?? (Reprimands = new ObservableCollection<ReprimandModel>(prvdr.Select()));
                        }
                    case 4:
                        using (var prvdr = new MilitaryProcessTableProvider())
                        {
                            return MilitaryProcesses ?? (MilitaryProcesses = new ObservableCollection<MilitaryProcessModel>(prvdr.Select()));
                        }
                    case 5:
                        using (var prvdr = new ClasinessTableProvider())
                        {
                            return Clasiness ?? (Clasiness = new ObservableCollection<ClasinessModel>(prvdr.Select()));
                        }
                    case 6:
                        using (var prvdr = new ContractTableProvider())
                        {
                            return Contracts ?? (Contracts = new ObservableCollection<ContractModel>(prvdr.Select()));
                        }
                    case 7:
                        using (var prvdr = new ViolationTableProvider())
                        {
                            return Violations ?? (Violations = new ObservableCollection<ViolationModel>(prvdr.Select()));
                        }
                    case 8:
                        using (var prvdr = new EducationTimeTableProvider())
                        {
                            return EducationTimes ?? (EducationTimes = new ObservableCollection<EducationTimeModel>(prvdr.Select()));
                        }
                    case 9:
                        using (var prvdr = new HolidayTimeTableProvider())
                        {
                            return HolidayTimes ?? (HolidayTimes = new ObservableCollection<HolidayTimeModel>(prvdr.Select()));
                        }
                    case 10:
                        using (var prvdr = new PostAssignmentTableProvider())
                        {
                            return PostAssignments ?? (PostAssignments = new ObservableCollection<PostAssignmentModel>(prvdr.Select()));
                        }
                    case 11:
                        using (var prvdr = new RankAssignmentTableProvider())
                        {
                            return RankAssignments ?? (RankAssignments = new ObservableCollection<RankAssignmentModel>(prvdr.Select()));
                        }
                    case 12:
                        using (var prvdr = new RelativeTableProvider())
                        {
                            return Relatives ?? (Relatives = new ObservableCollection<RelativeModel>(prvdr.Select()));
                        }
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Режимы
        /// </summary>
        public List<string> InformationModeList => _informationModeList ?? (_informationModeList = new List<string> //...рука лицо: костыль на костыле...
        {
            "Аттестация",
            "Благодарности",
            "Больничные",
            "Взыскания",
            "Воинская служба",
            "Классность",
            "Контракты",
            "Нарушения",
            "Образование",
            "Отпуска",
            "Присвоение должностей",
            "Присвоение званий",
            "Родственники"
        });

        #endregion

        #region Commands
        
        /// <summary>
        /// Принять изменения
        /// </summary>
        private RelayCommand _acceptChanges;
        public RelayCommand AcceptChanges => _acceptChanges ?? (_acceptChanges = new RelayCommand(AcceptChangesExecute));   //TODO: добавить флаг изменений

        private void AcceptChangesExecute()
        {
            var accept = MessageBox.Show("Применить изменения?", "Принять изменения", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);

            if (accept == MessageBoxResult.No) return;

            EmployeeViewModel.Rank = _rankList.SelectedItem;
            EmployeeViewModel.Post = SelectedPost;

            EmployeeViewModel.BornDate = BornDate;
            EmployeeViewModel.JobStartDate = JobStartDate;
            EmployeeViewModel.FirstName = FirstName;
            EmployeeViewModel.LastName = LastName;
            EmployeeViewModel.MiddleName = MiddleName;
            EmployeeViewModel.City = City;
            EmployeeViewModel.Street = Street;
            EmployeeViewModel.House = House;
            EmployeeViewModel.Flat = Flat;
            EmployeeViewModel.HomePhoneNumber = HomePhoneNumber;
            EmployeeViewModel.MobilePhoneNumber = MobilePhoneNumber;
            EmployeeViewModel.PersonalNumber = PersonalNumber;
            EmployeeViewModel.Photo = Photo;

            using (var prvdr = new PasportTableProvider())
            {
                if (!prvdr.Update(Pasport))
                    MessageBox.Show("Не удалось применить изменения! Ошибка: " + prvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

            }

            using (var prvdr = new EmployeeTableProvider())
            {
                if (!prvdr.Update(EmployeeViewModel._empModel))
                    MessageBox.Show("Не удалось применить изменения! Ошибка: " + prvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

            }
        }

        /// <summary>
        /// Отменить изменения
        /// </summary>
        private RelayCommand _cleanOut;
        public RelayCommand CleanOut => _cleanOut ?? (_cleanOut = new RelayCommand(CleanOutExecute));

        private void CleanOutExecute()
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
            //PasportNumber = EmployeeViewModel.PasportNumber;
            //PasportOrganizationUnit = EmployeeViewModel.PasportOrganizationUnit;
            //PasportSeries = EmployeeViewModel.PasportSeries;
            PersonalNumber = EmployeeViewModel.PersonalNumber;
            Photo = EmployeeViewModel.Photo;

            EmployeeViewModel = EmployeeViewModel;
        }

        /// <summary>
        /// Открывает OpenDialog для выбора новой фотографии
        /// </summary>
        private RelayCommand _changePhoto;
        public RelayCommand ChangePhoto => _changePhoto ?? (_changePhoto = new RelayCommand(ChangePhotoExecute));

        private void ChangePhotoExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JPEG Photo (*.jpg)|*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);

                if (fileInfo.Extension.ToLower() == ".jpg")
                {
                    Photo = BitmapImageHelper.SetSize(new BitmapImage(new Uri(fileInfo.FullName)), 600, 600); //стоит использовать менеезатратный вариант
                 }
            }
        }

        /// <summary>
        /// Удаляет фото служащего (устанавливает аватар по умолчанию)
        /// </summary>
        private RelayCommand _dropPhoto;
        public RelayCommand DropPhoto => _dropPhoto ?? (_dropPhoto = new RelayCommand(DropPhotoExecute));

        private void DropPhotoExecute()
        {
            Photo = null;
        }

        /// <summary>
        /// Переходим на edit tabs и обратно
        /// </summary>
        private RelayCommand _tabsToggle;
        
        public RelayCommand TabsToggle => _tabsToggle ?? (_tabsToggle = new RelayCommand(TabsToggleExecute));

        private void TabsToggleExecute()
        {
            SelectedTabIndex = SelectedTabIndex == 0 ? 1 : 0;   
        }
        
        #endregion

        #region Methods
        

        /// <summary>
        /// Подбирает паспорт из БД по id
        /// </summary>
        /// <param name="id">id паспорта</param>
        /// <returns></returns>
        private PasportModel GetPasport(long? id)
        {
            PasportModel pasport = null;
            try
            {
                using (var pasportTblPrvdr = new PasportTableProvider())
                {
                    pasport = pasportTblPrvdr.Select(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить паспортные данные", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            return pasport;
        }
        
        #endregion
    }
}