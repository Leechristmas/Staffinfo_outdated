using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Desktop.Shared
{
    /// <summary>
    /// Исключение, брошенное в случае, если указанная бд на сервере не найдена
    /// </summary>
    public class DatabaseNotFoundException: Exception
    {
        public DatabaseNotFoundException()
        { }

        public DatabaseNotFoundException(string message): base(message)
        { }

        public DatabaseNotFoundException(string message, Exception ex) : base(message, ex)
        { }
    }
}
