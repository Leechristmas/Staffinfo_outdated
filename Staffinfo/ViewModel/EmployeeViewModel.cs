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

        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

    }
}
