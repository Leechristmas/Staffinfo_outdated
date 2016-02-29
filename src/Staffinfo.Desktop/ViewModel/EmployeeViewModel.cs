using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel служащего
    /// </summary>
    public class EmployeeViewModel: WindowViewModelBase
    {
        #region Constructors

        public EmployeeViewModel(EmployeeModel employeeModel)
        {
            _empModel = employeeModel;
        }

        [PreferredConstructor]
        public EmployeeViewModel()
        {
            _empModel = new EmployeeModel();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Паспорт
        /// </summary>
        private PasportModel _pasport;

        /// <summary>
        /// Тип даныых, которые будут отображаться в grid'e
        /// </summary>
        private List<string> _informationModeList;

        #endregion

        #region Properties

        /// <summary>
        /// Выбранный служащий
        /// </summary>
        public readonly EmployeeModel _empModel;

        /// <summary>
        /// Фото служащего
        /// </summary>
        public BitmapImage Photo
        {
            get
            {
                return _empModel.Photo ??
                       (new BitmapImage(
                               new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                   "Resources/Images/empty_avatar_100x100.jpg"))));
            }
            set
            {
                _empModel.Photo = value; 
                RaisePropertyChanged("Photo");
            }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _empModel.LastName; }
            set
            {
                _empModel.LastName = value; 
                RaisePropertyChanged("LastName");
            }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _empModel.MiddleName; }
            set
            {
                _empModel.MiddleName = value; 
                RaisePropertyChanged("MiddleName");
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _empModel.FirstName; }
            set
            {
                _empModel.FirstName = value; 
                RaisePropertyChanged("FirstName");
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id
        {
            get { return _empModel.Id; }
            set
            {
                _empModel.Id = value; 
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// Личный номер
        /// </summary>
        public string PersonalNumber
        {
            get { return _empModel.PersonalNumber; }
            set
            {
                _empModel.PersonalNumber = value; 
                RaisePropertyChanged("PersonalNumber");
            }
        }

        /// <summary>
        /// Должность
        /// </summary>
        public PostModel Post
        {
            get { return DataSingleton.Instance.PostList.Find(p => p.Id == _empModel.PostId); }
            set
            {
                _empModel.PostId = value.Id; 
                RaisePropertyChanged("Post");
            }
        }

        /// <summary>
        /// Звание
        /// </summary>
        public RankModel Rank
        {
            get { return DataSingleton.Instance.RankList.Find(p => p.Id == _empModel.RankId); }
            set
            {
                _empModel.RankId = value.Id;
                RaisePropertyChanged("Rank");
            }
        }

        /// <summary>
        /// Служба
        /// </summary>
        public ServiceModel Service
        {
            get { return DataSingleton.Instance.ServiceList.Find(p => p.Id == Post.ServiceId); }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BornDate
        {
            get { return _empModel.BornDate; }
            set
            {
                _empModel.BornDate = value; 
                RaisePropertyChanged("BornDate");
            }
        }

        /// <summary>
        /// Дата начала работы в МЧС
        /// </summary>
        public DateTime? JobStartDate
        {
            get { return _empModel.JobStartDate; }
            set
            {
                _empModel.JobStartDate = value; 
                RaisePropertyChanged("JobStartDate");
            }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        //public string Address
        //{
        //    get { return _empModel.Address; }
        //    set
        //    {
        //        _empModel.Address = value; 
        //        RaisePropertyChanged("Address");
        //    }
        //}

        /// <summary>
        /// Город
        /// </summary>
        public string City
        {
            get { return _empModel.City; }
            set
            {
                _empModel.City = value;
                RaisePropertyChanged("City");
            }
        }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street
        {
            get { return _empModel.Street; }
            set
            {
                _empModel.Street = value;
                RaisePropertyChanged("Street");
            }
        }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string House
        {
            get { return _empModel.House; }
            set
            {
                _empModel.House = value;
                RaisePropertyChanged("House");
            }
        }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public string Flat
        {
            get { return _empModel.Flat; }
            set
            {
                _empModel.Flat = value;
                RaisePropertyChanged("Flat");
            }
        }

        /// <summary>
        /// Id паспорта
        /// </summary>
        public long? PasportId
        {
            get { return _empModel.PasportId; }
            set
            {
                _empModel.PasportId = value;
                RaisePropertyChanged("PasportId");
            }
        }
        
        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string MobilePhoneNumber
        {
            get { return _empModel.MobilePhoneNumber; }
            set
            {
                _empModel.MobilePhoneNumber = value; 
                RaisePropertyChanged("MobilePhoneNumber");
            }
        }

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        public string HomePhoneNumber
        {
            get { return _empModel.HomePhoneNumber; }
            set
            {
                _empModel.HomePhoneNumber = value; 
                RaisePropertyChanged("HomePhoneNumber");
            }
        }
        
        /// <summary>
        /// Режимы
        /// </summary>
        public List<string> InformationModeList => _informationModeList ?? (_informationModeList = new List<string>
        {
            "Аттестация",
            "Благодарности",
            "Больничные",
            "Взыскания",
            "Вониская служба",
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
    }
}
