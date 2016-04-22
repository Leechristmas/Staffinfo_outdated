using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Helpers;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.View;


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
        private int _selectedCatalogIndex = -1;

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
        /// Индекс выбранного "таба" (datagrid или добавление/редактирование pfgbcb)
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TabsToogleTitle");

                //обнуляем текст ошибки при переходе между табами
                CatalogTextError = String.Empty;
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
                RaisePropertyChanged();

                WasChanged = (_personalNumber != EmployeeViewModel.PersonalNumber);
            }
        }

        /// <summary>
        /// Полное имя служащего
        /// </summary>
        public string FullName => LastName + ' ' + FirstName + ' ' + MiddleName;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();
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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
                RaisePropertyChanged();

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
        public int SelectedCatalogIndex
        {
            get { return _selectedCatalogIndex; }
            set
            {
                _selectedCatalogIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedItem");

                //обнуляем текст ошибки при переходе между справочниками
                CatalogTextError = String.Empty;
            }
        }

        /// <summary>
        /// Аттестация
        /// </summary>
        public ObservableCollection<SertificationViewModel> Sertifications { get; set; }

        /// <summary>
        /// Больничные
        /// </summary>
        public ObservableCollection<HospitalTimeViewModel> HospitalTimes { get; set; }

        /// <summary>
        /// Благодарности
        /// </summary>
        public ObservableCollection<GratitudeViewModel> Gratitudes { get; set; }

        /// <summary>
        /// Выговоры
        /// </summary>
        public ObservableCollection<ReprimandViewModel> Reprimands { get; set; }

        /// <summary>
        /// Несение службы
        /// </summary>
        public ObservableCollection<MilitaryProcessViewModel> MilitaryProcesses { get; set; }

        /// <summary>
        /// Классности
        /// </summary>
        public ObservableCollection<ClasinessViewModel> Clasiness { get; set; }

        /// <summary>
        /// Контракты
        /// </summary>
        public ObservableCollection<ContractViewModel> Contracts { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        public ObservableCollection<ViolationViewModel> Violations { get; set; }

        /// <summary>
        /// Обучения
        /// </summary>
        public ObservableCollection<EducationalTimeViewModel> EducationTimes { get; set; }

        /// <summary>
        /// Отпуска
        /// </summary>
        public ObservableCollection<HolidayTimeViewModel> HolidayTimes { get; set; }

        /// <summary>
        /// Присвоения должностей
        /// </summary>
        public ObservableCollection<PostAssignmentViewModel> PostAssignments { get; set; }

        /// <summary>
        /// Присвоение званий
        /// </summary>
        public ObservableCollection<RankAssignmentViewModel> RankAssignments { get; set; }

        /// <summary>
        /// Родственники
        /// </summary>
        public ObservableCollection<RelativeViewModel> Relatives { get; set; }

        /// <summary>
        /// Воинские части
        /// </summary>
        public ObservableCollection<MilitaryUnitModel> MilitaryUnits => DataSingleton.Instance.MilitaryUnitList;

        /// <summary>
        /// Специальности
        /// </summary>
        public ObservableCollection<SpecialityModel> Specialities => DataSingleton.Instance.SpecialityList.GetSorted();

        /// <summary>
        /// Учебные заведения
        /// </summary>
        public ObservableCollection<EducationalInstitutionModel> EducationalInstitutions => DataSingleton.Instance.EducationalInstitutionList;

        /// <summary>
        /// Активный справочник
        /// </summary>
        public object SelectedItem  //Очередной гребанный костыль...стоит запилить что-то получше.
        {
            get
            {
                if (SelectedCatalogIndex < 0) return null;
                switch (SelectedCatalogIndex)
                {
                    case 0:
                        using (var prvdr = new SertificationTableProvider())
                        {
                            return Sertifications ??
                                   (Sertifications =
                                       new ObservableCollection<SertificationViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new SertificationViewModel(p))));
                        }
                    case 1:
                        using (var prvdr = new GratitudeTableProvider())
                        {
                            return Gratitudes ??
                                   (Gratitudes =
                                       new ObservableCollection<GratitudeViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new GratitudeViewModel(p))));
                        }
                    case 2:
                        using (var prvdr = new HospitalTimeTableProvider())
                        {
                            return HospitalTimes ??
                                   (HospitalTimes =
                                       new ObservableCollection<HospitalTimeViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new HospitalTimeViewModel(p))));
                        }
                    case 3:
                        using (var prvdr = new ReprimandTableProvider())
                        {
                            return Reprimands ??
                                   (Reprimands =
                                       new ObservableCollection<ReprimandViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new ReprimandViewModel(p))));
                        }
                    case 4:
                        using (var prvdr = new MilitaryProcessTableProvider())
                        {
                            return MilitaryProcesses ??
                                   (MilitaryProcesses =
                                       new ObservableCollection<MilitaryProcessViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new MilitaryProcessViewModel(p))));
                        }
                    case 5:
                        using (var prvdr = new ClasinessTableProvider())
                        {
                            return Clasiness ??
                                   (Clasiness =
                                       new ObservableCollection<ClasinessViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new ClasinessViewModel(p))));
                        }
                    case 6:
                        using (var prvdr = new ContractTableProvider())
                        {
                            return Contracts ??
                                   (Contracts =
                                       new ObservableCollection<ContractViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new ContractViewModel(p))));
                        }
                    case 7:
                        using (var prvdr = new ViolationTableProvider())
                        {
                            return Violations ??
                                   (Violations =
                                       new ObservableCollection<ViolationViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new ViolationViewModel(p))));
                        }
                    case 8:
                        using (var prvdr = new EducationTimeTableProvider())
                        {
                            return EducationTimes ??
                                   (EducationTimes =
                                       new ObservableCollection<EducationalTimeViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new EducationalTimeViewModel(p))));
                        }
                    case 9:
                        using (var prvdr = new HolidayTimeTableProvider())
                        {
                            return HolidayTimes ??
                                   (HolidayTimes =
                                       new ObservableCollection<HolidayTimeViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new HolidayTimeViewModel(p))));
                        }
                    case 10:
                        using (var prvdr = new PostAssignmentTableProvider())
                        {
                            return PostAssignments ??
                                   (PostAssignments =
                                       new ObservableCollection<PostAssignmentViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new PostAssignmentViewModel(p))));
                        }
                    case 11:
                        using (var prvdr = new RankAssignmentTableProvider())
                        {
                            return RankAssignments ??
                                   (RankAssignments =
                                       new ObservableCollection<RankAssignmentViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new RankAssignmentViewModel(p))));
                        }
                    case 12:
                        using (var prvdr = new RelativeTableProvider())
                        {
                            return Relatives ??
                                   (Relatives =
                                       new ObservableCollection<RelativeViewModel>(
                                           prvdr.Select().Where(s => s.EmployeeId == EmployeeViewModel.Id).Select(p => new RelativeViewModel(p))));
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

            CloseWindow();
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
        private RelayCommand _toAddView;

        public RelayCommand ToAddView => _toAddView ?? (_toAddView = new RelayCommand(ToAddViewExecute));

        private void ToAddViewExecute()
        {
            //указываем, что будет происходить добавление
            isChanging = false;
            TabsToggle();
        }

        private void TabsToggle()
        {
            SelectedTabIndex = SelectedTabIndex == 0 ? 1 : 0;
            switch (SelectedCatalogIndex)
            {
                case 0:
                    SertificationSetDefault();
                    break;
                case 1:
                    GratitudeSetDefault();
                    break;
                case 2:
                    HospitalTimeSetDefault();
                    break;
                case 3:
                    ReprimandSetDefault();
                    break;
                case 4:
                    MilitarySetDefault();
                    break;
                case 5:
                    ClasinessSetDefault();
                    break;
                case 6:
                    ContractSetDefault();
                    break;
                case 7:
                    ViolationSetDefault();
                    break;
                case 8:
                    EducationSetDefault();
                    break;
                case 9:
                    HolidayTimeSetDefault();
                    break;
                case 10:
                    PostAssignmentSetDefault();
                    break;
                case 11:
                    RankAssignmentSetDefault();
                    break;
                case 12:
                    RelativeSetDefault();
                    break;
            }
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

        /// <summary>
        /// Установить значения полей аттестации по умолчанию
        /// </summary>
        private void SertificationSetDefault()
        {
            SertificationDate = null;
            SertificationDescription = String.Empty;
        }

        /// <summary>
        /// Установить значения полей благодарности по умолчанию
        /// </summary>
        private void GratitudeSetDefault()
        {
            GratitudeDate = null;
            GratitudeDescription = String.Empty;
        }

        /// <summary>
        /// Установить значения полей больничного по умолчанию
        /// </summary>
        private void HospitalTimeSetDefault()
        {
            StartHospitalDate = null;
            FinishHospitalDate = null;
            HospitalTimeDescription = String.Empty;
        }

        /// <summary>
        /// Установить значения полей взыскания по умолчанию
        /// </summary>
        private void ReprimandSetDefault()
        {
            ReprimandDate = null;
            ReprimandDescription = String.Empty;
            ReprimandSum = String.Empty;
        }

        /// <summary>
        /// Установить значения полей прохождения службы по умолчанию
        /// </summary>
        private void MilitarySetDefault()
        {
            MilitaryStartDate = null;
            MilitaryFinishDate = null;
            MilitaryUnit = null;
            MilitaryDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей классности по умолчанию
        /// </summary>
        private void ClasinessSetDefault()
        {
            ClasinessDate = null;
            ClasinessOrderNumber = null;
            ClasinessDegree = null;
            ClasinessDescription = String.Empty;
        }

        /// <summary>
        /// Установить значения полей контракта по умолчанию
        /// </summary>
        private void ContractSetDefault()
        {
            StartContractDate = null;
            FinishContractDate = null;
            ContractDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей нарушения по умолчанию
        /// </summary>
        private void ViolationSetDefault()
        {
            ViolationDate = null;
            ViolationDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей обучения по умолчанию
        /// </summary>
        private void EducationSetDefault()
        {
            EducationStartDate = null;
            EducationFinishDate = null;
            EducationalInstitution = null;
            Speciality = null;
            EducationDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей отпуска по умолчанию
        /// </summary>
        private void HolidayTimeSetDefault()
        {
            StartHolidayDate = null;
            FinishHolidayDate = null;
            HolidayTimeDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей присвоения должности по умолчанию
        /// </summary>
        private void PostAssignmentSetDefault()
        {
            PostAssignmentDate = null;
            PostAssignmentOrderNumber = null;
            PostAssignmentOldPost = null;
            PostAssignmentNewPost = null;
            PostAssignmentDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей присвоения звания по умолчанию
        /// </summary>
        private void RankAssignmentSetDefault()
        {
            RankAssignmentDate = null;
            RankAssignmentOrderNumber = null;
            RankAssignmentOldRank = null;
            RankAssignmentNewRank = null;
            RankAssignmentDescription = String.Empty;
        }

        /// <summary>
        /// Установить значение полей родственника по умолчанию
        /// </summary>
        private void RelativeSetDefault()
        {
            RelativeFirstName = String.Empty;
            RelativeLastName = String.Empty;
            RelativeMiddleName = String.Empty;
            RelativeType = null;
            RelativeBornDate = null;

            RelativeDescription = String.Empty;
        }

        #endregion

        #region Catalogs

        #region Common fields

        /// <summary>
        /// Редактирование или добавление записи? (используется при добавлении записи)
        /// </summary>
        private bool isChanging = false;

        /// <summary>
        /// Текст ошибки при работе со справочниками
        /// </summary>
        private string _catalogErrorText = String.Empty;

        /// <summary>
        /// Индекс выделенной записи справочника
        /// </summary>
        private int _selectedCatalogRecordIndex = -1;

        /// <summary>
        /// Выбранный элемент из списков "catalog"
        /// </summary>
        private object _selectedCatalogRecord;


        #endregion

        #region Common properties

        /// <summary>
        /// Индекс выделенной записи справочника
        /// </summary>
        public int SelectedCatalogRecordIndex
        {
            get { return _selectedCatalogRecordIndex; }
            set
            {
                _selectedCatalogRecordIndex = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текст ошибки при работе со справочниками
        /// </summary>
        public string CatalogTextError
        {
            get { return _catalogErrorText; }
            set
            {
                _catalogErrorText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный элемент из списков "catalog"
        /// </summary>
        public object SelectedCatalogRecord
        {
            get { return _selectedCatalogRecord; }
            set
            {
                _selectedCatalogRecord = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Common commands

        /// <summary>
        /// Изменение записи
        /// </summary>
        private RelayCommand _updateItem;
        public RelayCommand UpdateItem => _updateItem ?? (_updateItem = new RelayCommand(UpdateItemExecute));

        private void UpdateItemExecute()
        {
            CatalogTextError = String.Empty;
            if (SelectedCatalogRecordIndex < 0)
            {
                CatalogTextError = "Запись не выбрана";
                return;
            }

            //переходим в редактирование
            TabsToggle();

            //указываем, что будет происходить изменение записи
            isChanging = true;

            switch (SelectedCatalogIndex)
            {
                case 0:     //аттестация
                    var sertification = SelectedCatalogRecord as SertificationViewModel;
                    if (sertification == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    SertificationDate = sertification.GetModel().SertificationDate;
                    SertificationDescription = sertification.GetModel().Description;
                    break;
                case 1:     //благодарности
                    var gratitude = SelectedCatalogRecord as GratitudeViewModel;
                    if (gratitude == null)
                    {
                        CatalogTextError = "ошибка. Редактирование невозможно";
                        return;
                    }
                    GratitudeDate = gratitude.GetModel().GratitudeDate;
                    GratitudeDescription = gratitude.GetModel().Description;
                    break;
                case 2:     //больничные
                    var hospitalTime = SelectedCatalogRecord as HospitalTimeViewModel;
                    if (hospitalTime == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    StartHospitalDate = hospitalTime.GetModel().StartDate;
                    FinishHospitalDate = hospitalTime.GetModel().FinishDate;
                    HospitalTimeDescription = hospitalTime.GetModel().Description;
                    break;
                case 3:     //взыскания
                    var reprimand = SelectedCatalogRecord as ReprimandViewModel;
                    if (reprimand == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    ReprimandDate = reprimand.GetModel().ReprimandDate;
                    ReprimandSum = reprimand.GetModel().ReprimandSum.ToString(CultureInfo.CurrentCulture);
                    ReprimandDescription = reprimand.GetModel().Description;
                    break;
                case 4:     //воинская служба
                    var military = SelectedCatalogRecord as MilitaryProcessViewModel;
                    if (military == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    MilitaryStartDate = military.GetModel().StartDate;
                    MilitaryFinishDate = military.GetModel().FinishDate;
                    MilitaryDescription = military.GetModel().Description;
                    var t = military.GetModel().MilitaryUnitId;
                    MilitaryUnit =
                        DataSingleton.Instance.MilitaryUnitList.FirstOrDefault(
                            m => m.Id == military.GetModel().MilitaryUnitId);
                    break;
                case 5:     //классность
                    var clasiness = SelectedCatalogRecord as ClasinessViewModel;
                    if (clasiness == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    ClasinessDate = clasiness.GetModel().ClasinessDate;
                    ClasinessDegree = clasiness.ClasinessLevel;
                    ClasinessOrderNumber = clasiness.OrderNumber;
                    ClasinessDescription = clasiness.Description;
                    break;
                case 6:     //контракты
                    var contract = SelectedCatalogRecord as ContractViewModel;
                    if (contract == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    ContractDescription = contract.Description;
                    StartContractDate = contract.GetModel().StartDate;
                    FinishContractDate = contract.GetModel().FinishDate;
                    break;
                case 7:     //нарушения
                    var violation = SelectedCatalogRecord as ViolationViewModel;
                    if (violation == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    ViolationDate = violation.GetModel().ViolationDate;
                    ViolationDescription = violation.Description;
                    break;
                case 8:     //образование
                    var education = SelectedCatalogRecord as EducationalTimeViewModel;
                    if (education == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    EducationStartDate = education.GetModel().StartDate;
                    EducationFinishDate = education.GetModel().FinishDate;
                    EducationDescription = education.Description;
                    EducationalInstitution =
                        DataSingleton.Instance.EducationalInstitutionList.FirstOrDefault(
                            i => i.Id == education.GetModel().InstitutionId);
                    Speciality =
                        DataSingleton.Instance.SpecialityList.FirstOrDefault(
                            s => s.Id == education.GetModel().SpecialityId);

                    break;
                case 9:     //отпуска
                    var holidayTime = SelectedCatalogRecord as HolidayTimeViewModel;
                    if (holidayTime == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно".ToString();
                        return;
                    }
                    StartHolidayDate = holidayTime.GetModel().StartDate;
                    FinishHolidayDate = holidayTime.GetModel().FinishDate;
                    HolidayTimeDescription = holidayTime.Description;
                    break;
                case 10:    //присвоение должностей
                    var postAssignment = SelectedCatalogRecord as PostAssignmentViewModel;
                    if (postAssignment == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    PostAssignmentDate = postAssignment.GetModel().AssignmentDate;
                    PostAssignmentDescription = postAssignment.GetModel().Description;
                    PostAssignmentOrderNumber = postAssignment.GetModel().OrderNumber;
                    PostAssignmentOldPost =
                        DataSingleton.Instance.PostList.FirstOrDefault(
                            p => p.Id == postAssignment.GetModel().PreviousPostId);
                    PostAssignmentNewPost =
                        DataSingleton.Instance.PostList.FirstOrDefault(p => p.Id == postAssignment.GetModel().NewPostId);
                    PostAssignmentService =
                        DataSingleton.Instance.ServiceList.FirstOrDefault(s => s.Id == PostAssignmentNewPost.Id);
                    break;
                case 11:    //присвоение званий
                    var rankAssignment = SelectedCatalogRecord as RankAssignmentViewModel;
                    if (rankAssignment == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    RankAssignmentDate = rankAssignment.GetModel().AssignmentDate;
                    RankAssignmentDescription = rankAssignment.GetModel().Description;
                    RankAssignmentOrderNumber = rankAssignment.GetModel().OrderNumber;
                    RankAssignmentOldRank =
                        DataSingleton.Instance.RankList.FirstOrDefault(
                            r => r.Id == rankAssignment.GetModel().PreviousRankId);
                    RankAssignmentNewRank =
                        DataSingleton.Instance.RankList.FirstOrDefault(r => r.Id == rankAssignment.GetModel().NewRankId);
                    break;
                case 12:    //родственники
                    var relative = SelectedCatalogRecord as RelativeViewModel;
                    if (relative == null)
                    {
                        CatalogTextError = "Ошибка. Редактирование невозможно";
                        return;
                    }
                    RelativeFirstName = relative.Firtname;
                    RelativeLastName = relative.Lastname;
                    RelativeMiddleName = relative.Middlename;
                    RelativeBornDate = relative.GetModel().BornDate;
                    RelativeDescription = relative.GetModel().Description;
                    RelativeType = relative.GetModel().RelationType;
                    break;
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        private RelayCommand _removeItem;
        public RelayCommand RemoveItem => _removeItem ?? (_removeItem = new RelayCommand(RemoveItemExecute));

        private void RemoveItemExecute()
        {
            CatalogTextError = String.Empty;
            if (SelectedCatalogRecordIndex < 0)
            {
                CatalogTextError = "Запись не выбрана";
                return;
            }
            var answer = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No);

            if (answer == MessageBoxResult.No) return;

            try
            {
                switch (SelectedCatalogIndex)
                {
                    case 0:     //аттестация
                        using (SertificationTableProvider sPrvdr = new SertificationTableProvider())
                        {
                            var sertificationId = Sertifications.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!sPrvdr.DeleteById(sertificationId)) throw new Exception(sPrvdr.ErrorInfo); //если удалить не удалось, бросаем exception
                        }
                        Sertifications.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 1:     //благодарности
                        using (GratitudeTableProvider gPrvdr = new GratitudeTableProvider())
                        {
                            var gratitudeId = Gratitudes.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!gPrvdr.DeleteById(gratitudeId)) throw new Exception(gPrvdr.ErrorInfo);
                        }
                        Gratitudes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 2:     //больничные
                        using (HospitalTimeTableProvider hTPrvdr = new HospitalTimeTableProvider())
                        {
                            var hospitalTimeId = HospitalTimes.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!hTPrvdr.DeleteById(hospitalTimeId)) throw new Exception(hTPrvdr.ErrorInfo);
                        }
                        HospitalTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 3:     //взыскания
                        using (ReprimandTableProvider rPrvdr = new ReprimandTableProvider())
                        {
                            var reprimandId = Reprimands.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!rPrvdr.DeleteById(reprimandId)) throw new Exception(rPrvdr.ErrorInfo);
                        }
                        Reprimands.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 4:     //воинская служба
                        using (MilitaryProcessTableProvider mPrvdr = new MilitaryProcessTableProvider())
                        {
                            var militaryId = MilitaryProcesses.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!mPrvdr.DeleteById(militaryId)) throw new Exception(mPrvdr.ErrorInfo);
                        }
                        MilitaryProcesses.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 5:     //классность
                        using (ClasinessTableProvider cPrvdr = new ClasinessTableProvider())
                        {
                            var clasinessId = Clasiness.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!cPrvdr.DeleteById(clasinessId)) throw new Exception(cPrvdr.ErrorInfo);
                        }
                        Clasiness.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 6:     //классность
                        using (ContractTableProvider cPrvdr = new ContractTableProvider())
                        {
                            var contractId = Contracts.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!cPrvdr.DeleteById(contractId)) throw new Exception(cPrvdr.ErrorInfo);
                        }
                        Contracts.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 7:     //нарушения
                        using (ViolationTableProvider vPrvdr = new ViolationTableProvider())
                        {
                            var violationId = Violations.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!vPrvdr.DeleteById(violationId)) throw new Exception(vPrvdr.ErrorInfo);
                        }
                        Violations.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 8:     //образование
                        using (EducationTimeTableProvider ePrvdr = new EducationTimeTableProvider())
                        {
                            var educationId = EducationTimes.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!ePrvdr.DeleteById(educationId)) throw new Exception(ePrvdr.ErrorInfo);
                        }
                        EducationTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 9:     //отпуска
                        using (HolidayTimeTableProvider hPrvdr = new HolidayTimeTableProvider())
                        {
                            var holidayId = HolidayTimes.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!hPrvdr.DeleteById(holidayId)) throw new Exception(hPrvdr.ErrorInfo);
                        }
                        HolidayTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 10:    //присвоение должностей
                        using (PostAssignmentTableProvider pPrvdr = new PostAssignmentTableProvider())
                        {
                            var assignmentId = PostAssignments.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!pPrvdr.DeleteById(assignmentId)) throw new Exception(pPrvdr.ErrorInfo);
                        }
                        PostAssignments.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 11:    //присвоение званий
                        using (RankAssignmentTableProvider rPrvdr = new RankAssignmentTableProvider())
                        {
                            var assignmentId = RankAssignments.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!rPrvdr.DeleteById(assignmentId)) throw new Exception(rPrvdr.ErrorInfo);
                        }
                        RankAssignments.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 12:    //родственники
                        using (RelativeTableProvider rPrvdr = new RelativeTableProvider())
                        {
                            var relativeId = Relatives.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!rPrvdr.DeleteById(relativeId)) throw new Exception(rPrvdr.ErrorInfo);
                        }
                        Relatives.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить запись: " + e.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.OK);
            }

        }

        #endregion

        #region Sertifications

        /// <summary>
        /// Дата аттестации
        /// </summary>
        private DateTime? _sertificationDate = null;

        /// <summary>
        /// Заметка к 
        /// </summary>
        private string _sertificationDescription = String.Empty;

        /// <summary>
        /// Дата аттестации
        /// </summary>
        public DateTime? SertificationDate
        {
            get { return _sertificationDate; }
            set
            {
                _sertificationDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к аттестации
        /// </summary>
        public string SertificationDescription
        {
            get { return _sertificationDescription; }
            set
            {
                _sertificationDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить аттестацию
        /// </summary>
        private RelayCommand _addSertification;
        public RelayCommand AddSertification
            => _addSertification ?? (_addSertification = new RelayCommand(AddSertificationExecute));

        private void AddSertificationExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (SertificationDate == null)  //SertificationDate > DateTime.Now.Date - позволяем вводить аттестации с датой > Now, чтобы отслеживать следующее прохождение аттестации
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата не указана или указана неверно";
                return;
            }

            //заносим аттестацию в бд и список
            using (SertificationTableProvider sPrvdr = new SertificationTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as SertificationViewModel;
                    var model = viewModel?.GetModel();

                    if (model.SertificationDate == SertificationDate.Value &&
                        viewModel.Description == SertificationDescription)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.SertificationDate = SertificationDate.Value;
                    model.Description = SertificationDescription;

                    //обновление в бд
                    if (!sPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + sPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Sertifications[SelectedCatalogRecordIndex] = new SertificationViewModel(model);
                    }
                }
                else
                {
                    var sertification = sPrvdr.Save(new SertificationModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        SertificationDate = SertificationDate.Value,
                        Description = SertificationDescription
                    });

                    if (sertification == null)
                    {
                        MessageBox.Show("Не удалось сохранить аттестацию: " + sPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Sertifications.Add(new SertificationViewModel(sertification));
                }
            }

            SertificationSetDefault();

            TabsToggle();
        }

        #endregion

        #region Gratitudes

        /// <summary>
        /// Дата вынесения благодарности
        /// </summary>
        private DateTime? _gratitudeionDate = null;

        /// <summary>
        /// Заметка к благодарности
        /// </summary>
        private string _gratitudeDescription = String.Empty;

        /// <summary>
        /// Дата вынесения благодарности
        /// </summary>
        public DateTime? GratitudeDate
        {
            get { return _gratitudeionDate; }
            set
            {
                _gratitudeionDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к вынесению благодарности
        /// </summary>
        public string GratitudeDescription
        {
            get { return _gratitudeDescription; }
            set
            {
                _gratitudeDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить вынесение благодарности
        /// </summary>
        private RelayCommand _addGratitude;
        public RelayCommand AddGratitude
            => _addGratitude ?? (_addGratitude = new RelayCommand(AddGratitudeExecute));

        private void AddGratitudeExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (GratitudeDate == null || GratitudeDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата не указана или указана неверно";
                return;
            }

            //заносим благодарность в бд и список
            using (GratitudeTableProvider sPrvdr = new GratitudeTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as GratitudeViewModel;
                    var model = viewModel?.GetModel();

                    if (model.GratitudeDate == GratitudeDate.Value &&
                        viewModel.Description == GratitudeDescription)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.GratitudeDate = GratitudeDate.Value;
                    model.Description = GratitudeDescription;

                    //обновление в бд
                    if (!sPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + sPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Gratitudes[SelectedCatalogRecordIndex] = new GratitudeViewModel(model);
                    }
                }
                else
                {
                    var gratitude = sPrvdr.Save(new GratitudeModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        GratitudeDate = GratitudeDate.Value,
                        Description = GratitudeDescription
                    });

                    if (gratitude == null)
                    {
                        MessageBox.Show("Не удалось сохранить вынесение благодарности: " + sPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Gratitudes.Add(new GratitudeViewModel(gratitude));
                }
            }

            GratitudeSetDefault();

            TabsToggle();
        }

        #endregion

        #region Hospital times

        /// <summary>
        /// Дата открытия больничного
        /// </summary>
        private DateTime? _startHospitalDate = null;

        /// <summary>
        /// Дата закрытия больничного
        /// </summary>
        private DateTime? _finishHospitalDate = null;

        /// <summary>
        /// Заметка к больничному
        /// </summary>
        private string _hospitalTimeDescription = String.Empty;

        /// <summary>
        /// Дата открытия больничного
        /// </summary>
        public DateTime? StartHospitalDate
        {
            get { return _startHospitalDate; }
            set
            {
                _startHospitalDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата закрытия больничного
        /// </summary>
        public DateTime? FinishHospitalDate
        {
            get { return _finishHospitalDate; }
            set
            {
                _finishHospitalDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к больничному
        /// </summary>
        public string HospitalTimeDescription
        {
            get { return _hospitalTimeDescription; }
            set
            {
                _hospitalTimeDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить больничный
        /// </summary>
        private RelayCommand _addHospitalTime;
        public RelayCommand AddHospitalTime
            => _addHospitalTime ?? (_addHospitalTime = new RelayCommand(AddHospitalTimeExecute));

        private void AddHospitalTimeExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (StartHospitalDate == null || StartHospitalDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата открытия больничного не указана или указана неверно";
                return;
            }
            if (FinishHospitalDate > DateTime.Now.Date || FinishHospitalDate < StartHospitalDate || FinishHospitalDate == null)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата закрытия больничного не указана или указана неверно";
                return;
            }

            //заносим больничный в бд и список
            using (HospitalTimeTableProvider hTPrvdr = new HospitalTimeTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as HospitalTimeViewModel;
                    var model = viewModel?.GetModel();

                    if (model.StartDate == StartHospitalDate.Value &&
                        model.Description == HospitalTimeDescription &&
                        model.FinishDate == FinishHospitalDate)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.StartDate = StartHospitalDate.Value;
                    model.Description = HospitalTimeDescription;
                    model.FinishDate = FinishHospitalDate.Value;

                    //обновление в бд
                    if (!hTPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + hTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        HospitalTimes[SelectedCatalogRecordIndex] = new HospitalTimeViewModel(model);
                    }
                }
                else
                {
                    var hospitalTime = hTPrvdr.Save(new HospitalTimeModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        StartDate = StartHospitalDate.Value,
                        FinishDate = FinishHospitalDate.Value,
                        Description = HospitalTimeDescription
                    });

                    if (hospitalTime == null)
                    {
                        MessageBox.Show("Не удалось сохранить больничный: " + hTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        HospitalTimes.Add(new HospitalTimeViewModel(hospitalTime));
                }
            }

            HospitalTimeSetDefault();

            TabsToggle();
        }

        #endregion

        #region Reprimands

        /// <summary>
        /// Дата вынесения выговора
        /// </summary>
        private DateTime? _reprimandDate = null;

        /// <summary>
        /// Сумма взыскания
        /// </summary>
        private string _reprimanSum;

        /// <summary>
        /// Заметка к выговору
        /// </summary>
        private string _reprimandDescription;

        /// <summary>
        /// Дата вынесения выговора
        /// </summary>
        public DateTime? ReprimandDate
        {
            get { return _reprimandDate; }
            set
            {
                _reprimandDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Сумма взыскания
        /// </summary>
        public string ReprimandSum
        {
            get { return _reprimanSum; }
            set
            {
                _reprimanSum = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к выговору
        /// </summary>
        public string ReprimandDescription
        {
            get { return _reprimandDescription; }
            set
            {
                _reprimandDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить выговор
        /// </summary>
        private RelayCommand _addReprimand;

        public RelayCommand AddReprimand
            => _addReprimand ?? (_addReprimand = new RelayCommand(AddReprimandExecute));

        private void AddReprimandExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (ReprimandDate == null || ReprimandDate.Value.Date > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата вынесения выговора не указана или указана неверно";
                return;
            }
            //введенная сумма выговора
            decimal reprimandSum;
            if (!Decimal.TryParse(ReprimandSum, out reprimandSum))
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Сумма взыскания не указана или указана неверно";
                return;
            }

            using (ReprimandTableProvider rPrvdr = new ReprimandTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as ReprimandViewModel;
                    var model = viewModel?.GetModel();

                    if (model.ReprimandDate == ReprimandDate.Value &&
                        model.Description == ReprimandDescription &&
                        model.ReprimandSum == reprimandSum)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.ReprimandDate = ReprimandDate.Value;
                    model.Description = ReprimandDescription;
                    model.ReprimandSum = reprimandSum;

                    //обновление в бд
                    if (!rPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + rPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Reprimands[SelectedCatalogRecordIndex] = new ReprimandViewModel(model);
                    }
                }
                else
                {
                    var reprimand = rPrvdr.Save(new ReprimandModel
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        ReprimandDate = ReprimandDate.Value,
                        ReprimandSum = reprimandSum,
                        Description = ReprimandDescription
                    });

                    if (reprimand == null)
                    {
                        MessageBox.Show("Не удалось сохранить вынесение выговора: " + rPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Reprimands.Add(new ReprimandViewModel(reprimand));
                }
            }

            ReprimandSetDefault();

            TabsToggle();
        }

        #endregion

        #region Military

        /// <summary>
        /// Дата начала службы
        /// </summary>
        private DateTime? _militaryStartDate = null;

        /// <summary>
        /// Дата окончания службы
        /// </summary>
        private DateTime? _militaryFinishDate = null;

        /// <summary>
        /// Выделенная воинская часть
        /// </summary>
        private MilitaryUnitModel _militaryUnit;

        /// <summary>
        /// Описание
        /// </summary>
        private string _militaryDescription = String.Empty;

        /// <summary>
        /// Дата начала службы
        /// </summary>
        public DateTime? MilitaryStartDate
        {
            get { return _militaryStartDate; }
            set
            {
                _militaryStartDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата окончания службы
        /// </summary>
        public DateTime? MilitaryFinishDate
        {
            get { return _militaryFinishDate; }
            set
            {
                _militaryFinishDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выделенная воинская часть
        /// </summary>
        public MilitaryUnitModel MilitaryUnit
        {
            get { return _militaryUnit; }
            set
            {
                _militaryUnit = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string MilitaryDescription
        {
            get { return _militaryDescription; }
            set
            {
                _militaryDescription = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Добавить прохождение службы
        /// </summary>
        private RelayCommand _addMilitaryProcess;
        public RelayCommand AddMilitaryProcess
            => _addMilitaryProcess ?? (_addMilitaryProcess = new RelayCommand(AddMilitaryProcessExecute));

        private void AddMilitaryProcessExecute()
        {
            CatalogTextError = String.Empty;
            if (MilitaryStartDate == null || MilitaryStartDate.Value.Date > DateTime.Now)
            {
                CatalogTextError = "Дата начала службы не указана или указана неверно";
                return;
            }
            if (MilitaryFinishDate == null ||
                MilitaryFinishDate.Value.Date < MilitaryStartDate.Value.Date ||
                MilitaryFinishDate.Value.Date > DateTime.Now.Date)
            {
                CatalogTextError = "Дата окончания службы не указана или указана неверно";
                return;
            }
            if (MilitaryUnit == null)
            {
                CatalogTextError = "Воинская часть не указана";
                return;
            }
            if (MilitaryDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
            }
            using (MilitaryProcessTableProvider mPrvdr = new MilitaryProcessTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as MilitaryProcessViewModel;
                    var model = viewModel?.GetModel();

                    if (model.StartDate == MilitaryStartDate.Value &&
                        model.Description == MilitaryDescription &&
                        model.FinishDate == MilitaryFinishDate.Value &&
                        model.MilitaryUnitId == MilitaryUnit.Id)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.StartDate = MilitaryStartDate.Value;
                    model.Description = MilitaryDescription;
                    model.FinishDate = MilitaryFinishDate.Value;
                    model.MilitaryUnitId = MilitaryUnit.Id.Value;

                    //обновление в бд
                    if (!mPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + mPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        MilitaryProcesses[SelectedCatalogRecordIndex] = new MilitaryProcessViewModel(model);
                    }
                }
                else
                {
                    var militaryProcess = mPrvdr.Save(new MilitaryProcessModel
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        StartDate = MilitaryStartDate.Value,
                        FinishDate = MilitaryFinishDate.Value,
                        MilitaryUnitId = MilitaryUnit.Id.Value,
                        Description = MilitaryDescription
                    });
                    if (militaryProcess == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись о несении воинской службы: " + mPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MilitaryProcesses.Add(new MilitaryProcessViewModel(militaryProcess));
                }
                
                MilitarySetDefault();

                TabsToggle();
            }
        }

        /// <summary>
        /// Перейти к добавлению воинской части
        /// </summary>
        private RelayCommand _toMilitaryUnitAddOn;
        public RelayCommand ToMilitaryUnitAddOn
            => _toMilitaryUnitAddOn ?? (_toMilitaryUnitAddOn = new RelayCommand(ToMilitaryUnitAddOnExecute));

        private void ToMilitaryUnitAddOnExecute()
        {
            AddMilitaryUnitView addWindow = new AddMilitaryUnitView();
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();
        }

        #endregion

        #region Clasiness

        /// <summary>
        /// Дата получения/подтверждения классности
        /// </summary>
        private DateTime? _clasinessDate = null;

        /// <summary>
        /// Номер приказа
        /// </summary>
        private int? _clasinessOrderNumber = null;

        /// <summary>
        /// Описание
        /// </summary>
        private string _clasinessDescription = String.Empty;

        /// <summary>
        /// Степень классности
        /// </summary>
        private int? _clasinessDegree = null;

        /// <summary>
        /// Дата получения/подтверждения классности
        /// </summary>
        public DateTime? ClasinessDate
        {
            get { return _clasinessDate; }
            set
            {
                _clasinessDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public int? ClasinessOrderNumber
        {
            get { return _clasinessOrderNumber; }
            set
            {
                _clasinessOrderNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание к классности
        /// </summary>
        public string ClasinessDescription
        {
            get { return _clasinessDescription; }
            set
            {
                _clasinessDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Уровень классности
        /// </summary>
        public int? ClasinessDegree
        {
            get { return _clasinessDegree; }
            set
            {
                _clasinessDegree = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить классность
        /// </summary>
        private RelayCommand _addClasiness;
        public RelayCommand AddClasiness => _addClasiness ?? (_addClasiness = new RelayCommand(AddClasinessExecute));

        private void AddClasinessExecute()
        {
            var date = ClasinessDate;
            var desc = ClasinessDescription;
            var order = ClasinessOrderNumber;
            var level = ClasinessDegree;

            CatalogTextError = String.Empty;

            if (ClasinessDate == null || ClasinessDate.Value.Date > DateTime.Now.Date)
            {
                CatalogTextError = "Дата не указана или указана неверно";
                return;
            }
            if (ClasinessDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
                return;
            }
            if (ClasinessOrderNumber == null)
            {
                CatalogTextError = "Не указан номер приказа";
                return;
            }
            if (ClasinessDegree == null)
            {
                CatalogTextError = "Не указана классность";
                return;
            }
            using (ClasinessTableProvider cPrvdr = new ClasinessTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as ClasinessViewModel;
                    var model = viewModel?.GetModel();

                    if (model.ClasinessDate == ClasinessDate.Value &&
                        model.Description == ClasinessDescription &&
                        model.ClasinessLevel == ClasinessDegree &&
                        model.OrderNumber == ClasinessOrderNumber)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.ClasinessDate = ClasinessDate.Value;
                    model.Description = ClasinessDescription;
                    model.ClasinessLevel = ClasinessDegree.Value;
                    model.OrderNumber = ClasinessOrderNumber.Value;

                    //обновление в бд
                    if (!cPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + cPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Clasiness[SelectedCatalogRecordIndex] = new ClasinessViewModel(model);
                    }
                }
                else
                {
                    var clasiness = cPrvdr.Save(new ClasinessModel
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        ClasinessDate = ClasinessDate.Value,
                        ClasinessLevel = ClasinessDegree.Value,
                        OrderNumber = ClasinessOrderNumber.Value,
                        Description = ClasinessDescription
                    });
                    if (clasiness == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись о классности: " + cPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Clasiness.Add(new ClasinessViewModel(clasiness));
                }
                
                ClasinessSetDefault();

                TabsToggle();
            }
        }
        #endregion

        #region Contracts

        /// <summary>
        /// Дата подписания контракта
        /// </summary>
        private DateTime? _startContractDate = null;

        /// <summary>
        /// Дата окончания контракта
        /// </summary>
        private DateTime? _finishContractDate = null;

        /// <summary>
        /// Заметка к контракту
        /// </summary>
        private string _contractDescription = String.Empty;

        /// <summary>
        /// Дата подписания контракта
        /// </summary>
        public DateTime? StartContractDate
        {
            get { return _startContractDate; }
            set
            {
                _startContractDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата окончания контракта
        /// </summary>
        public DateTime? FinishContractDate
        {
            get { return _finishContractDate; }
            set
            {
                _finishContractDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к контракту
        /// </summary>
        public string ContractDescription
        {
            get { return _contractDescription; }
            set
            {
                _contractDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить контракт
        /// </summary>
        private RelayCommand _addContract;

        public RelayCommand AddContract
            => _addContract ?? (_addContract = new RelayCommand(AddContractExecute));

        private void AddContractExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (StartContractDate == null || StartContractDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата открытия контракта не указана или указана неверно";
                return;
            }
            if (FinishContractDate < StartContractDate ||
                FinishContractDate == null)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата окончания контракта не указана или указана неверно";
                return;
            }
            if (ContractDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
                return;
            }

            //заносим контракт в бд и список
            using (ContractTableProvider cTPrvdr = new ContractTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as ContractViewModel;
                    var model = viewModel?.GetModel();

                    if (model.StartDate == StartContractDate.Value &&
                        model.Description == ContractDescription &&
                        model.FinishDate == FinishContractDate.Value)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.StartDate = StartContractDate.Value;
                    model.Description = ContractDescription;
                    model.FinishDate = FinishContractDate.Value;

                    //обновление в бд
                    if (!cTPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + cTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Contracts[SelectedCatalogRecordIndex] = new ContractViewModel(model);
                    }
                }
                else
                {
                    var contract = cTPrvdr.Save(new ContractModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        StartDate = StartContractDate.Value,
                        FinishDate = FinishContractDate.Value,
                        Description = ContractDescription
                    });

                    if (contract == null)
                    {
                        MessageBox.Show("Не удалось сохранить контракт: " + cTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Contracts.Add(new ContractViewModel(contract));
                }
            }

            ContractSetDefault();

            TabsToggle();
        }

        #endregion

        #region Violations

        /// <summary>
        /// Дата нарушения
        /// </summary>
        private DateTime? _violationDate = null;

        /// <summary>
        /// Заметка к нарушению
        /// </summary>
        private string _violationDescription = String.Empty;

        /// <summary>
        /// Дата нарушения
        /// </summary>
        public DateTime? ViolationDate
        {
            get { return _violationDate; }
            set
            {
                _violationDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к нарушению
        /// </summary>
        public string ViolationDescription
        {
            get { return _violationDescription; }
            set
            {
                _violationDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить нарушение
        /// </summary>
        private RelayCommand _addViolation;

        public RelayCommand AddViolation
            => _addViolation ?? (_addViolation = new RelayCommand(AddViolationExecute));

        private void AddViolationExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (ViolationDate == null || ViolationDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата не указана или указана неверно";
                return;
            }
            if (ViolationDescription.Length == 0)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Нарушение не указано";
                return;
            }
            if (ViolationDescription.Length > 200)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Слишком длинное описание нарушения";
                return;
            }

            //заносим нарушение в бд и список
            using (ViolationTableProvider vPrvdr = new ViolationTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as ViolationViewModel;
                    var model = viewModel?.GetModel();

                    if (model.ViolationDate == ViolationDate.Value &&
                        model.Description == ViolationDescription)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.ViolationDate = ViolationDate.Value;
                    model.Description = ViolationDescription;

                    //обновление в бд
                    if (!vPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + vPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Violations[SelectedCatalogRecordIndex] = new ViolationViewModel(model);
                    }
                }
                else
                {
                    var violation = vPrvdr.Save(new ViolationModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        ViolationDate = ViolationDate.Value,
                        Description = ViolationDescription
                    });

                    if (violation == null)
                    {
                        MessageBox.Show("Не удалось сохранить нарушение: " + vPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Violations.Add(new ViolationViewModel(violation));
                }
            }

            ViolationSetDefault();

            TabsToggle();
        }

        #endregion

        #region Studying

        /// <summary>
        /// Дата начала обучения
        /// </summary>
        private DateTime? _educationStartDate = null;

        /// <summary>
        /// Дата окончания обучения
        /// </summary>
        private DateTime? _educationFinishDate = null;

        /// <summary>
        /// уч. заведение
        /// </summary>
        private EducationalInstitutionModel _educationalInstitution;

        /// <summary>
        /// специальность
        /// </summary>
        private SpecialityModel _speciality;

        /// <summary>
        /// Описание
        /// </summary>
        private string _educationDescription = String.Empty;

        /// <summary>
        /// Дата начала обучения
        /// </summary>
        public DateTime? EducationStartDate
        {
            get { return _educationStartDate; }
            set
            {
                _educationStartDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата окончания обучения
        /// </summary>
        public DateTime? EducationFinishDate
        {
            get { return _educationFinishDate; }
            set
            {
                _educationFinishDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранное уч. заведение
        /// </summary>
        public EducationalInstitutionModel EducationalInstitution
        {
            get { return _educationalInstitution; }
            set
            {
                _educationalInstitution = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Специальность
        /// </summary>
        public SpecialityModel Speciality
        {
            get { return _speciality; }
            set
            {
                _speciality = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string EducationDescription
        {
            get { return _educationDescription; }
            set
            {
                _educationDescription = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Добавить обучение
        /// </summary>
        private RelayCommand _addEducation;
        public RelayCommand AddEducation
            => _addEducation ?? (_addEducation = new RelayCommand(AddEducationExecute));

        private void AddEducationExecute()
        {
            CatalogTextError = String.Empty;
            if (EducationStartDate == null || EducationStartDate.Value.Date > DateTime.Now)
            {
                CatalogTextError = "Дата начала обучения не указана или указана неверно";
                return;
            }
            if (EducationFinishDate == null ||
                EducationFinishDate.Value.Date < EducationStartDate.Value.Date)
            {
                CatalogTextError = "Дата окончания обучения не указана или указана неверно";
                return;
            }
            if (EducationalInstitution == null)
            {
                CatalogTextError = "Учебное заведение не указано";
                return;
            }
            if (Speciality == null)
            {
                CatalogTextError = "Специальность не указана";
                return;
            }
            if (EducationDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
            }
            using (EducationTimeTableProvider ePrvdr = new EducationTimeTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as EducationalTimeViewModel;
                    var model = viewModel?.GetModel();

                    if (model.StartDate == EducationStartDate.Value &&
                        model.FinishDate == EducationFinishDate.Value &&
                        model.Description == EducationDescription &&
                        model.InstitutionId == EducationalInstitution.Id &&
                        model.SpecialityId == Speciality.Id)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.StartDate = EducationStartDate.Value;
                    model.FinishDate = EducationFinishDate.Value;
                    model.Description = EducationDescription;
                    model.InstitutionId = EducationalInstitution.Id.Value;
                    model.SpecialityId = Speciality.Id.Value;

                    //обновление в бд
                    if (!ePrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + ePrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        EducationTimes[SelectedCatalogRecordIndex] = new EducationalTimeViewModel(model);
                    }
                }
                else
                {
                    var education = ePrvdr.Save(new EducationTimeModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        StartDate = EducationStartDate.Value,
                        FinishDate = EducationFinishDate.Value,
                        InstitutionId = EducationalInstitution.Id.Value,
                        SpecialityId = Speciality.Id.Value,
                        Description = EducationDescription
                    });
                    if (education == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись об обучении: " + ePrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        EducationTimes.Add(new EducationalTimeViewModel(education));
                }
                    


                TabsToggle();
            }
        }

        /// <summary>
        /// Открыть окно добавления учебного заведения
        /// </summary>
        private RelayCommand _addEducationUnit;

        public RelayCommand AddEducationUnit
            => _addEducationUnit ?? (_addEducationUnit = new RelayCommand(AddEducationUnitExecute));

        private void AddEducationUnitExecute()
        {
            AddEducationalInstitutionView addWindow = new AddEducationalInstitutionView();
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();
        }

        /// <summary>
        /// Открыть окно добавления специальности
        /// </summary>
        private RelayCommand _addSpeciality;

        public RelayCommand AddSpeciality
            => _addSpeciality ?? (_addSpeciality = new RelayCommand(AddSpecialityExecute));

        private void AddSpecialityExecute()
        {
            AddSpecialityView addWindow = new AddSpecialityView();
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();
        }

        #endregion

        #region Holiday times

        /// <summary>
        /// Дата открытия отпуска
        /// </summary>
        private DateTime? _startHolidayDate = null;

        /// <summary>
        /// Дата закрытия отпуска
        /// </summary>
        private DateTime? _finishHolidayDate = null;

        /// <summary>
        /// Заметка к отпуску
        /// </summary>
        private string _holidayTimeDescription = String.Empty;

        /// <summary>
        /// Дата открытия отпуска
        /// </summary>
        public DateTime? StartHolidayDate
        {
            get { return _startHolidayDate; }
            set
            {
                _startHolidayDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата закрытия отпуска
        /// </summary>
        public DateTime? FinishHolidayDate
        {
            get { return _finishHolidayDate; }
            set
            {
                _finishHolidayDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Заметка к отпуску
        /// </summary>
        public string HolidayTimeDescription
        {
            get { return _holidayTimeDescription; }
            set
            {
                _holidayTimeDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить отпуск
        /// </summary>
        private RelayCommand _addHolidayTime;

        public RelayCommand AddHolidayTime
            => _addHolidayTime ?? (_addHolidayTime = new RelayCommand(AddHolidayTimeExecute));

        private void AddHolidayTimeExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (StartHolidayDate == null || StartHolidayDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата открытия отпуска не указана или указана неверно";
                return;
            }
            if (FinishHolidayDate > DateTime.Now.Date || FinishHolidayDate < StartHolidayDate ||
                FinishHolidayDate == null)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата закрытия отпуска не указана или указана неверно";
                return;
            }

            //заносим отпуск в бд и список
            using (HolidayTimeTableProvider hTPrvdr = new HolidayTimeTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as HolidayTimeViewModel;
                    var model = viewModel?.GetModel();

                    if (model.StartDate == StartHolidayDate.Value &&
                        model.FinishDate == FinishHolidayDate.Value &&
                        model.Description == HolidayTimeDescription)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.StartDate = StartHolidayDate.Value;
                    model.FinishDate = FinishHolidayDate.Value;
                    model.Description = HolidayTimeDescription;

                    //обновление в бд
                    if (!hTPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + hTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        HolidayTimes[SelectedCatalogRecordIndex] = new HolidayTimeViewModel(model);
                    }
                }
                else
                {
                    var holidayTime = hTPrvdr.Save(new HolidayTimeModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        StartDate = StartHolidayDate.Value,
                        FinishDate = FinishHolidayDate.Value,
                        Description = HolidayTimeDescription
                    });

                    if (holidayTime == null)
                    {
                        MessageBox.Show("Не удалось сохранить отпуск: " + hTPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        HolidayTimes.Add(new HolidayTimeViewModel(holidayTime));
                }
                    
            }

            HolidayTimeSetDefault();

            TabsToggle();
        }

        #endregion

        #region PostAssignment

        #region Fields

        /// <summary>
        /// Дата присвоения должности
        /// </summary>
        private DateTime? _postAssignmentDate = null;

        /// <summary>
        /// Номер приказа
        /// </summary>
        private int? _postAssignmentOrderNumber = null;

        /// <summary>
        /// Описание
        /// </summary>
        private string _postAssignmentDescription = String.Empty;

        /// <summary>
        /// Выбранная служба
        /// </summary>
        private ServiceModel _postAssignmentService;

        /// <summary>
        /// Старая должность
        /// </summary>
        private PostModel _postAssignmentOldPost;

        /// <summary>
        /// Новая должность
        /// </summary>
        private PostModel _postAssignmentNewPost;

        /// <summary>
        /// "Активная служба"
        /// </summary>
        private readonly ServiceModel _postAssignmenService;

        #endregion

        #region Properties

        /// <summary>
        /// Дата присвоения должности
        /// </summary>
        public DateTime? PostAssignmentDate
        {
            get { return _postAssignmentDate; }
            set
            {
                _postAssignmentDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public int? PostAssignmentOrderNumber
        {
            get { return _postAssignmentOrderNumber; }
            set
            {
                _postAssignmentOrderNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string PostAssignmentDescription
        {
            get { return _postAssignmentDescription; }
            set
            {
                _postAssignmentDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранная служба
        /// </summary>
        public ServiceModel PostAssignmentService
        {
            get { return _postAssignmentService; }
            set
            {
                _postAssignmentService = value;
                RaisePropertyChanged();
                RaisePropertyChanged("PostAssignmentPostList");
            }
        }

        /// <summary>
        /// Старая должность
        /// </summary>
        public PostModel PostAssignmentOldPost
        {
            get { return _postAssignmentOldPost; }
            set
            {
                _postAssignmentOldPost = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Новая должность
        /// </summary>
        public PostModel PostAssignmentNewPost
        {
            get { return _postAssignmentNewPost; }
            set
            {
                _postAssignmentNewPost = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Должности (для учета всех присвоений)
        /// </summary>
        public ListViewModel<PostModel> PostAssignmentPostList => new ListViewModel<PostModel>(_postList.ModelList.Where(post => post.ServiceId == PostAssignmentService?.Id).ToList());

        #endregion

        /// <summary>
        /// Добавить присвоение должности
        /// </summary>
        private RelayCommand _addPostAssignment;

        public RelayCommand AddPostAssignment
            => _addPostAssignment ?? (_addPostAssignment = new RelayCommand(AddPostAssignmentExecute));

        private void AddPostAssignmentExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (PostAssignmentDate == null || PostAssignmentDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата присвоения должности не указана или указана неверно";
                return;
            }
            if (PostAssignmentOrderNumber == null)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Не указан номер приказа";
                return;
            }
            if (PostAssignmentDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
            }
            if (PostAssignmentOldPost == null)
            {
                CatalogTextError = "Не указана текущая должность";
                return;
            }
            if (PostAssignmentNewPost == null)
            {
                CatalogTextError = "Не указана новая должность";
                return;
            }
            if (PostAssignmentNewPost.Id == PostAssignmentOldPost.Id)
            {
                CatalogTextError = "Не может быть выбрана та же должность";
                return;
            }

            //заносим присвоение должности в бд и список
            using (PostAssignmentTableProvider pAPrvdr = new PostAssignmentTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as PostAssignmentViewModel;
                    var model = viewModel?.GetModel();

                    if (model.AssignmentDate == PostAssignmentDate.Value &&
                        model.Description == PostAssignmentDescription &&
                        model.NewPostId == PostAssignmentNewPost.Id &&
                        model.PreviousPostId == PostAssignmentOldPost.Id &&
                        model.OrderNumber == PostAssignmentOrderNumber)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.AssignmentDate = PostAssignmentDate.Value;
                    model.Description = PostAssignmentDescription;
                    model.NewPostId = PostAssignmentNewPost.Id.Value;
                    model.PreviousPostId = PostAssignmentOldPost.Id.Value;
                    model.OrderNumber = PostAssignmentOrderNumber.Value;

                    //обновление в бд
                    if (!pAPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + pAPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        PostAssignments[SelectedCatalogRecordIndex] = new PostAssignmentViewModel(model);
                    }
                }
                else
                {
                    var postAssignment = pAPrvdr.Save(new PostAssignmentModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        AssignmentDate = PostAssignmentDate.Value,
                        OrderNumber = PostAssignmentOrderNumber.Value,
                        PreviousPostId = PostAssignmentOldPost.Id.Value,
                        NewPostId = PostAssignmentNewPost.Id.Value,
                        Description = PostAssignmentDescription
                    });

                    if (postAssignment == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись: " + pAPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        PostAssignments.Add(new PostAssignmentViewModel(postAssignment));
                }
                    
            }

            PostAssignmentSetDefault();

            TabsToggle();
        }

        #endregion

        #region RankAssignment

        #region Fields

        /// <summary>
        /// Дата присвоения звания
        /// </summary>
        private DateTime? _rankAssignmentDate = null;

        /// <summary>
        /// Номер приказа
        /// </summary>
        private int? _rankAssignmentOrderNumber = null;

        /// <summary>
        /// Описание
        /// </summary>
        private string _rankAssignmentDescription = String.Empty;

        /// <summary>
        /// Старое звание
        /// </summary>
        private RankModel _rankAssignmentOldRank;

        /// <summary>
        /// Новое звание
        /// </summary>
        private RankModel _rankAssignmentNewRank;

        #endregion

        #region Properties

        /// <summary>
        /// Дата присвоения 
        /// </summary>
        public DateTime? RankAssignmentDate
        {
            get { return _rankAssignmentDate; }
            set
            {
                _rankAssignmentDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public int? RankAssignmentOrderNumber
        {
            get { return _rankAssignmentOrderNumber; }
            set
            {
                _rankAssignmentOrderNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string RankAssignmentDescription
        {
            get { return _rankAssignmentDescription; }
            set
            {
                _rankAssignmentDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Старое звание
        /// </summary>
        public RankModel RankAssignmentOldRank
        {
            get { return _rankAssignmentOldRank; }
            set
            {
                _rankAssignmentOldRank = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Новое звание
        /// </summary>
        public RankModel RankAssignmentNewRank
        {
            get { return _rankAssignmentNewRank; }
            set
            {
                _rankAssignmentNewRank = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Добавить присвоение звания
        /// </summary>
        private RelayCommand _addRankAssignment;
        public RelayCommand AddRankAssignment
            => _addRankAssignment ?? (_addRankAssignment = new RelayCommand(AddRankAssignmentExecute));

        private void AddRankAssignmentExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;
            if (RankAssignmentDate == null || PostAssignmentDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата присвоения звания не указана или указана неверно";
                return;
            }
            if (RankAssignmentOrderNumber == null)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Не указан номер приказа";
                return;
            }
            if (RankAssignmentDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
            }
            if (RankAssignmentOldRank == null)
            {
                CatalogTextError = "Не указано текущее звание";
                return;
            }
            if (RankAssignmentNewRank == null)
            {
                CatalogTextError = "Не указано новое звание";
                return;
            }
            if (RankAssignmentOldRank.Id == RankAssignmentNewRank.Id)
            {
                CatalogTextError = "Не может быть выбрано то же звание";
                return;
            }

            //заносим присвоение должности в бд и список
            using (RankAssignmentTableProvider rAPrvdr = new RankAssignmentTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as RankAssignmentViewModel;
                    var model = viewModel?.GetModel();

                    if (model.AssignmentDate == RankAssignmentDate.Value &&
                        model.Description == RankAssignmentDescription &&
                        model.NewRankId == RankAssignmentNewRank.Id &&
                        model.PreviousRankId == RankAssignmentOldRank.Id &&
                        model.OrderNumber == RankAssignmentOrderNumber)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.AssignmentDate = RankAssignmentDate.Value;
                    model.Description = RankAssignmentDescription;
                    model.NewRankId = RankAssignmentNewRank.Id.Value;
                    model.PreviousRankId = RankAssignmentOldRank.Id.Value;
                    model.OrderNumber = RankAssignmentOrderNumber.Value;

                    //обновление в бд
                    if (!rAPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + rAPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        RankAssignments[SelectedCatalogRecordIndex] = new RankAssignmentViewModel(model);
                    }
                }
                else
                {
                    var rankAssignment = rAPrvdr.Save(new RankAssignmentModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        AssignmentDate = RankAssignmentDate.Value,
                        OrderNumber = RankAssignmentOrderNumber.Value,
                        PreviousRankId = RankAssignmentOldRank.Id.Value,
                        NewRankId = RankAssignmentNewRank.Id.Value,
                        Description = RankAssignmentDescription
                    });

                    if (rankAssignment == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись: " + rAPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        RankAssignments.Add(new RankAssignmentViewModel(rankAssignment));
                }
                    
            }

            RankAssignmentSetDefault();

            TabsToggle();
        }

        #endregion

        #region Relatives

        /// <summary>
        /// Имя родственника
        /// </summary>
        private string _relativeFirstName = String.Empty;

        /// <summary>
        /// Фамилия родственника
        /// </summary>
        private string _relativeLastName = String.Empty;

        /// <summary>
        /// Отчество родственника
        /// </summary>
        private string _relativeMiddleName = String.Empty;

        /// <summary>
        /// Тип родства
        /// </summary>
        private string _relativeType;

        /// <summary>
        /// Дата рождения родственника
        /// </summary>
        private DateTime? _relativeBornDate;

        /// <summary>
        /// Описание
        /// </summary>
        private string _relativeDescription = String.Empty;

        /// <summary>
        /// Имя родственника
        /// </summary>
        public string RelativeFirstName
        {
            get { return _relativeFirstName; }
            set
            {
                _relativeFirstName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Фамилия родственника
        /// </summary>
        public string RelativeLastName
        {
            get { return _relativeLastName; }
            set
            {
                _relativeLastName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Отчество родственника
        /// </summary>
        public string RelativeMiddleName
        {
            get { return _relativeMiddleName; }
            set
            {
                _relativeMiddleName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Тип родства
        /// </summary>
        public string RelativeType
        {
            get { return _relativeType; }
            set
            {
                _relativeType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Дата рождения родственника
        /// </summary>
        public DateTime? RelativeBornDate
        {
            get { return _relativeBornDate; }
            set
            {
                _relativeBornDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string RelativeDescription
        {
            get { return _relativeDescription; }
            set
            {
                _relativeDescription = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить родственника
        /// </summary>
        private RelayCommand _addRelative;
        public RelayCommand AddRelative
            => _addRelative ?? (_addRelative = new RelayCommand(AddRelativeExecute));

        private void AddRelativeExecute()
        {
            //зануляем текст ошибки
            CatalogTextError = String.Empty;

            if (RelativeFirstName == String.Empty || RelativeFirstName.Length > 60)
            {
                CatalogTextError = "Имя не указано или имеет слишком большую длину";
                return;
            }
            if (RelativeLastName == String.Empty || RelativeLastName.Length > 60)
            {
                CatalogTextError = "Фамилия не указана или имеет слишком большую длину";
                return;
            }
            if (RelativeMiddleName == String.Empty || RelativeMiddleName.Length > 60)
            {
                CatalogTextError = "Отчество не указано или имеет слишком большую длину";
                return;
            }
            if (RelativeType == null)
            {
                CatalogTextError = "Не указано родство";
                return;
            }
            if (RelativeBornDate == null || PostAssignmentDate > DateTime.Now.Date)
            {
                //если ошибка валидации - указываем текст ошибки
                CatalogTextError = "Дата рождения не указана или указана неверно";
                return;
            }
            if (RelativeDescription.Length > 200)
            {
                CatalogTextError = "Слишком длинное описание";
                return;
            }

            //заносим присвоение должности в бд и список
            using (RelativeTableProvider rPrvdr = new RelativeTableProvider())
            {
                if (isChanging)
                {
                    //поднимаем модель из viewmodel
                    var viewModel = SelectedCatalogRecord as RelativeViewModel;
                    var model = viewModel?.GetModel();

                    if (model.BornDate == RelativeBornDate.Value &&
                        model.Description == RelativeDescription &&
                        model.RelationType == RelativeType &&
                        model.LastName == RelativeLastName &&
                        model.FirstName == RelativeFirstName &&
                        model.MiddleName == RelativeMiddleName)
                    {
                        TabsToggle();
                        return;
                    }

                    //обновляем модель
                    model.BornDate = RelativeBornDate.Value;
                    model.Description = RelativeDescription;
                    model.RelationType = RelativeType;
                    model.LastName = RelativeLastName;
                    model.FirstName = RelativeFirstName;
                    model.MiddleName = RelativeMiddleName;

                    //обновление в бд
                    if (!rPrvdr.Update(model))
                        MessageBox.Show("Не удалось применить изменения: " + rPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Relatives[SelectedCatalogRecordIndex] = new RelativeViewModel(model);
                    }
                }
                else
                {
                    var relative = rPrvdr.Save(new RelativeModel()
                    {
                        EmployeeId = EmployeeViewModel.Id.Value,
                        FirstName = RelativeFirstName,
                        LastName = RelativeLastName,
                        MiddleName = RelativeMiddleName,
                        RelationType = RelativeType,
                        BornDate = RelativeBornDate.Value,
                        Description = RelativeDescription
                    });

                    if (relative == null)
                    {
                        MessageBox.Show("Не удалось сохранить запись: " + rPrvdr.ErrorInfo, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Relatives.Add(new RelativeViewModel(relative));
                }
                    
            }

            RelativeSetDefault();

            TabsToggle();
        }

        #endregion

        #endregion

        //private RelayCommand _autoGenerationColumn;
        //public RelayCommand AutoGenerationColumn => _autoGenerationColumn ?? (AutoGenerationColumnExecute);

        //перенести обработку из code-behind сюда!!!
    }
}