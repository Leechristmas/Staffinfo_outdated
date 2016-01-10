using GalaSoft.MvvmLight;
using Staffinfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.ViewModel
{
    public class EmployeeViewModel: ViewModelBase
    {
        public EmployeeViewModel()
        {
            var data = Data.Data.Instance.DataBaseConnection;
            data.GetEmployee();
            empModel = Data.Data.Instance.EmployeeList.ToList()[0];
        }

        EmployeeModel empModel;
        
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

        //<!--string firstName,
        //   string middleName,
        //   string lastName,
        //   string personalNumber,
        //   long? post,
        //   long? rank,
        //   DateTime? bornDate,
        //   DateTime? jobStartDate,
        //   string address,
        //   string pasport,
        //   string mobilePhoneNumber-->
    }
}
