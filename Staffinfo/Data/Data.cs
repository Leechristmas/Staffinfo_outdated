using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Data
{
    public class Data
    {
        private static Data _instance;

        private Data()
        {

        }

        public static Data Instance
        {
            get { return _instance ?? (_instance = new Data()); }
        }




    }
}
