using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
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

            ////список паспортных столов
            //_pasportOrganizationUnitList = new ListViewModel<PasportOrganizationUnitModel>(DataSingleton.Instance.PasportOrganizationUnitList);

        }

        public EmployeeEditViewModel(EmployeeViewModel employeeViewModel) : this()
        {
            EmployeeViewModel = employeeViewModel;

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
            Pasport = GetPasport(employeeViewModel.PasportId);
            PersonalNumber = employeeViewModel.PersonalNumber;
            Photo = employeeViewModel.Photo;

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

        ///// <summary>
        ///// Список паспортных столов
        ///// </summary>
        //private ListViewModel<PasportOrganizationUnitModel> _pasportOrganizationUnitList;

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


        #endregion

        #region Properties

        /// <summary>
        /// Флаг изменений
        /// </summary>
        public bool WasChanged
        {
            get { return _wasChanged; }
            set
            {
                _wasChanged = value;
                RaisePropertyChanged("WasChanged");
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

        ///// <summary>
        ///// Паспортные столы
        ///// </summary>
        //public ListViewModel<PasportOrganizationUnitModel> PasportOrganizationList => _pasportOrganizationUnitList;

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

                WasChanged = !ImageCompare(_photo, EmployeeViewModel.Photo);
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
        public RelayCommand AcceptChanges => _acceptChanges ?? (_acceptChanges = new RelayCommand(AcceptChangesExecute));   //TODO: добавить флаг изменений

        private void AcceptChangesExecute()
        {
            var accept = MessageBox.Show("Применить изменения?", "Принять изменения", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);

            if (accept == MessageBoxResult.No) return;

            EmployeeViewModel.Rank = _rankList.SelectedItem;
            EmployeeViewModel.Post = _postList.SelectedItem;

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
                    Photo = new BitmapImage(new Uri(fileInfo.FullName));
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

        #endregion

        #region Methods

        /// <summary>
        /// Конвертирует картинку в массив байт
        /// </summary>
        /// <param name="image">исходное изображение</param>
        /// <returns></returns>
        private byte[] ImageToByte(BitmapImage image)
        {
            if (image == null) return null;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Сравнивает 2 изображения
        /// </summary>
        /// <param name="btm1">первое изображение</param>
        /// <param name="btm2">второе изображение</param>
        /// <returns></returns>
        private bool ImageCompare(BitmapImage btm1, BitmapImage btm2)
        {
            if (btm2 == null || btm1 == null) return false;

            return Convert.ToBase64String(ImageToByte(btm1))
                   == Convert.ToBase64String(ImageToByte(btm2));
        }

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

        //Законсервировал до лучших времен...
        ///// <summary>
        ///// Подбираем паспортный стол по id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private PasportOrganizationUnitModel GetPasportOrganizationUnit(long? id)
        //{
        //    PasportOrganizationUnitModel pasportOrganizationUnit = null;
        //    try
        //    {
        //        using (var prvdr = new PasportOrganizationUnitTableProvider())
        //        {
        //            pasportOrganizationUnit = prvdr.Select(id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Не удалось загрузить паспортные данные", "Ошибка", MessageBoxButton.OK,
        //            MessageBoxImage.Error);
        //    }
        //    return pasportOrganizationUnit;
        //}
        
        #endregion
    }
}