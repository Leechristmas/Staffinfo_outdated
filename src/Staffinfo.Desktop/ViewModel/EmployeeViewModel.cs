using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// ViewModel для окна отображения служащего
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

        /// <summary>
        /// Выбранный служащий
        /// </summary>
        readonly EmployeeModel _empModel;

        #region Properties

        /// <summary>
        /// Фото служащего
        /// </summary>
        public BitmapImage Photo
        {
            get
            {
                return _empModel.Photo ??
                       (_empModel.Photo =
                           new BitmapImage(
                               new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                   "Resources/Images/empty_avatar_150x100.png"))));
            }
            set { _empModel.Photo = value; }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _empModel.LastName; }
            set { _empModel.LastName = value; }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _empModel.MiddleName; }
            set { _empModel.MiddleName = value; }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _empModel.FirstName; }
            set { _empModel.FirstName = value; }
        }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id
        {
            get { return _empModel.Id; }
            set { _empModel.Id = value; }
        }

        /// <summary>
        /// Личный номер
        /// </summary>
        public string PersonalNumber
        {
            get { return _empModel.PersonalNumber; }
            set { _empModel.PersonalNumber = value; }
        }

        /// <summary>
        /// Должность
        /// </summary>
        public long? Post
        {
            get { return _empModel.PostId; }
            set { _empModel.PostId = value; }
        }

        /// <summary>
        /// Звание
        /// </summary>
        public long? Rank
        {
            get { return _empModel.RankId; }
            set { _empModel.RankId = value; }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BornDate
        {
            get { return _empModel.BornDate; }
            set { _empModel.BornDate = value; }
        }

        /// <summary>
        /// Дата начала работы в МЧС
        /// </summary>
        public DateTime? JobStartDate
        {
            get { return _empModel.JobStartDate; }
            set { _empModel.JobStartDate = value; }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address
        {
            get { return _empModel.Address; }
            set { _empModel.Address = value; }
        }

        /// <summary>
        /// Паспорт
        /// </summary>
        public string Pasport
        {
            get { return _empModel.Pasport; }
            set { _empModel.Pasport = value; }
        }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string MobilePhoneNumber
        {
            get { return _empModel.MobilePhoneNumber; }
            set { _empModel.MobilePhoneNumber = value; }
        }

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        public string HomePhoneNumber
        {
            get { return _empModel.HomePhoneNumber; }
            set { _empModel.HomePhoneNumber = value; }
        }


        /// <summary>
        /// Тип даныых, которые будут отображаться в grid'e
        /// </summary>
        private List<string> _informationModeList;

        public List<string> InformationModeList
        {
            get
            {
                return _informationModeList ?? (_informationModeList = new List<string>
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
            }
            set { _informationModeList = value; }
        }

        #endregion
    }
}
