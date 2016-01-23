using GalaSoft.MvvmLight;
using Staffinfo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;

namespace Staffinfo.Desktop.ViewModel
{
    public class EmployeeViewModel: ViewModelBase
    {
        public EmployeeViewModel(EmployeeModel employeeModel)
        {
            empModel = employeeModel;
        }

        [PreferredConstructor]
        public EmployeeViewModel()
        {
                empModel = new EmployeeModel();
        }

        EmployeeModel empModel;
        
        public BitmapImage Photo
        {
            get
            {
                return empModel.Photo ??
                    (empModel.Photo = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Images/empty_avatar_150x100.png"))));
            }
            set { empModel.Photo = value; }
        }

        public string LastName
        {
            get { return empModel.LastName; }
            set { empModel.LastName = value; }
        }

        public string MiddleName
        {
            get { return empModel.MiddleName; }
            set { empModel.MiddleName = value; }
        }
        
        public string FirstName
        {
            get { return empModel.FirstName; }
            set { empModel.FirstName = value; }
        }
        
        public long Id
        {
            get { return empModel.Id; }
            set { empModel.Id = value; }
        }
        
        public string PersonalNumber
        {
            get { return empModel.PersonalNumber; }
            set { empModel.PersonalNumber = value; }
        }
        
        public long? Post
        {
            get { return empModel.Post; }
            set { empModel.Post = value; }
        }
        
        public long? Rank
        {
            get { return empModel.Rank; }
            set { empModel.Rank = value; }
        }
        
        public DateTime? BornDate
        {
            get { return empModel.BornDate; }
            set { empModel.BornDate = value; }
        }
        
        public DateTime? JobStartDate
        {
            get { return empModel.JobStartDate; }
            set { empModel.JobStartDate = value; }
        }
        
        public string Address
        {
            get { return empModel.Address; }
            set { empModel.Address = value; }
        }
        
        public string Pasport
        {
            get { return empModel.Pasport; }
            set { empModel.Pasport = value; }
        }
        
        public string MobilePhoneNumber
        {
            get { return empModel.MobilePhoneNumber; }
            set { empModel.MobilePhoneNumber = value; }
        }

        /// <summary>
        /// Тип даныых
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
