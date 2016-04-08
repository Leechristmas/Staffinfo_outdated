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
        /// Типы родства
        /// </summary>
        public ObservableCollection<RelativeTypeModel> RelativeTypes => DataSingleton.Instance.RelativeTypeList;

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

        #region Catalogs

        #region Common fields

        /// <summary>
        /// Текст ошибки при работе со справочниками
        /// </summary>
        private string _catalogErrorText = String.Empty;

        /// <summary>
        /// Индекс выделенной записи справочника
        /// </summary>
        private int _selectedCatalogRecordIndex = -1;

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

        #endregion

        #region Common commands
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
            
            try
            {
                switch (SelectedCatalogIndex)
                {
                    case 0:     //аттестация
                        using (SertificationTableProvider sPrvdr = new SertificationTableProvider())
                        {
                            var sertificationId = Sertifications.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!sPrvdr.DeleteById(sertificationId)) throw new Exception(sPrvdr.ErrorInfo); //если удалить не удалось, бросаем exception
                        }
                        Sertifications.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 1:     //благодарности
                        using (GratitudeTableProvider gPrvdr = new GratitudeTableProvider())
                        {
                            var gratitudeId = Gratitudes.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!gPrvdr.DeleteById(gratitudeId)) throw new Exception(gPrvdr.ErrorInfo);
                        }
                        Gratitudes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 2:     //больничные
                        using (HospitalTimeTableProvider hTPrvdr = new HospitalTimeTableProvider())
                        {
                            var hospitalTimeId = HospitalTimes.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!hTPrvdr.DeleteById(hospitalTimeId)) throw new Exception(hTPrvdr.ErrorInfo);
                        }
                        HospitalTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 3:     //взыскания
                        using (ReprimandTableProvider rPrvdr = new ReprimandTableProvider())
                        {
                            var reprimandId = Reprimands.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!rPrvdr.DeleteById(reprimandId)) throw new Exception(rPrvdr.ErrorInfo);
                        }
                        Reprimands.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 4:     //воинская служба
                        using (MilitaryProcessTableProvider mPrvdr = new MilitaryProcessTableProvider())
                        {
                            var militaryId = MilitaryProcesses.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!mPrvdr.DeleteById(militaryId)) throw new Exception(mPrvdr.ErrorInfo);
                        }
                        MilitaryProcesses.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 5:     //классность
                        using (ClasinessTableProvider cPrvdr = new ClasinessTableProvider())
                        {
                            var clasinessId = Clasiness.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!cPrvdr.DeleteById(clasinessId)) throw new Exception(cPrvdr.ErrorInfo);
                        }
                        Clasiness.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 6:     //классность
                        using (ContractTableProvider cPrvdr = new ContractTableProvider())
                        {
                            var contractId = Contracts.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!cPrvdr.DeleteById(contractId)) throw new Exception(cPrvdr.ErrorInfo);
                        }
                        Contracts.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 7:     //нарушения
                        using (ViolationTableProvider vPrvdr = new ViolationTableProvider())
                        {
                            var violationId = Violations.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!vPrvdr.DeleteById(violationId)) throw new Exception(vPrvdr.ErrorInfo);
                        }
                        Violations.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 8:     //образование
                        using (EducationTimeTableProvider ePrvdr = new EducationTimeTableProvider())
                        {
                            var educationId = EducationTimes.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!ePrvdr.DeleteById(educationId)) throw new Exception(ePrvdr.ErrorInfo);
                        }
                        EducationTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 9:     //отпуска
                        using (HolidayTimeTableProvider hPrvdr = new HolidayTimeTableProvider())
                        {
                            var holidayId = HolidayTimes.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!hPrvdr.DeleteById(holidayId)) throw new Exception(hPrvdr.ErrorInfo);
                        }
                        HolidayTimes.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 10:    //присвоение должностей
                        using (PostAssignmentTableProvider pPrvdr = new PostAssignmentTableProvider())
                        {
                            var assignmentId = PostAssignments.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!pPrvdr.DeleteById(assignmentId)) throw new Exception(pPrvdr.ErrorInfo);
                        }
                        PostAssignments.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 11:    //присвоение званий
                        using (RankAssignmentTableProvider rPrvdr = new RankAssignmentTableProvider())
                        {
                            var assignmentId = RankAssignments.ElementAt(SelectedCatalogRecordIndex).Id;
                            if (!rPrvdr.DeleteById(assignmentId)) throw new Exception(rPrvdr.ErrorInfo);
                        }
                        RankAssignments.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 12:    //родственники
                        using (RelativeTableProvider rPrvdr = new RelativeTableProvider())
                        {
                            var relativeId = Relatives.ElementAt(SelectedCatalogRecordIndex).Id;
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
                    return;
                }
                Sertifications.Add(sertification);
            }

            SertificationDate = null;
            SertificationDescription = String.Empty;

            TabsToggleExecute();
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
                    return;
                }
                Gratitudes.Add(gratitude);
            }

            GratitudeDate = null;
            GratitudeDescription = String.Empty;

            TabsToggleExecute();
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
        public string  HospitalTimeDescription
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
                var hospitalTime = hTPrvdr.Save(new HospitalTimeModel()
                {
                    EmployeeId = EmployeeViewModel.Id.Value,
                    StartDate = StartHospitalDate.Value,
                    FinishDate = FinishHospitalDate.Value,
                    Description = GratitudeDescription
                });

                if (hospitalTime == null)
                {
                    MessageBox.Show("Не удалось сохранить вынесение благодарности: " + hTPrvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                HospitalTimes.Add(hospitalTime);
            }

            StartHospitalDate = null;
            FinishHospitalDate = null;
            HospitalTimeDescription = String.Empty;

            TabsToggleExecute();
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
                    return;
                }
                Reprimands.Add(reprimand);
            }

            ReprimandDate = null;
            ReprimandDescription = String.Empty;
            ReprimandSum = String.Empty;

            TabsToggleExecute();
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
                    return;
                }
                MilitaryProcesses.Add(militaryProcess);

                MilitaryStartDate = null;
                MilitaryFinishDate = null;
                MilitaryUnit = null;
                MilitaryDescription = String.Empty;

                TabsToggleExecute();
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
                    return;
                }
                Clasiness.Add(clasiness);

                ClasinessDate = null;
                ClasinessOrderNumber = null;
                ClasinessDegree = null;
                ClasinessDescription = String.Empty;

                TabsToggleExecute();
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
                    return;
                }
                Contracts.Add(contract);
            }

            StartContractDate = null;
            FinishContractDate = null;
            ContractDescription = String.Empty;

            TabsToggleExecute();
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
                    return;
                }
                Violations.Add(violation);
            }

            ViolationDate = null;
            ViolationDescription = String.Empty;

            TabsToggleExecute();
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
                    return;
                }
                EducationTimes.Add(education);

                EducationStartDate = null;
                EducationFinishDate = null;
                EducationalInstitution = null;
                Speciality = null;
                EducationDescription = String.Empty;

                TabsToggleExecute();
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



        #endregion

    }
}