using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class EmployeeViewModel: ViewModelBase
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

        readonly EmployeeModel _empModel;

        #region Properties

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

        public string LastName
        {
            get { return _empModel.LastName; }
            set { _empModel.LastName = value; }
        }

        public string MiddleName
        {
            get { return _empModel.MiddleName; }
            set { _empModel.MiddleName = value; }
        }

        public string FirstName
        {
            get { return _empModel.FirstName; }
            set { _empModel.FirstName = value; }
        }

        public long? Id
        {
            get { return _empModel.Id; }
            set { _empModel.Id = value; }
        }

        public string PersonalNumber
        {
            get { return _empModel.PersonalNumber; }
            set { _empModel.PersonalNumber = value; }
        }

        public long? Post
        {
            get { return _empModel.PostId; }
            set { _empModel.PostId = value; }
        }

        public long? Rank
        {
            get { return _empModel.RankId; }
            set { _empModel.RankId = value; }
        }

        public DateTime? BornDate
        {
            get { return _empModel.BornDate; }
            set { _empModel.BornDate = value; }
        }

        public DateTime? JobStartDate
        {
            get { return _empModel.JobStartDate; }
            set { _empModel.JobStartDate = value; }
        }

        public string Address
        {
            get { return _empModel.Address; }
            set { _empModel.Address = value; }
        }

        public string Pasport
        {
            get { return _empModel.Pasport; }
            set { _empModel.Pasport = value; }
        }

        public string MobilePhoneNumber
        {
            get { return _empModel.MobilePhoneNumber; }
            set { _empModel.MobilePhoneNumber = value; }
        }
        #endregion

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
    }
}
