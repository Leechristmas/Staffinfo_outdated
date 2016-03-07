using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Desktop.Shared
{
    /// <summary>
    /// Исключение: ошибка создания базы данных
    /// </summary>
    public class DatabaseWasNotCreatedException: Exception
    {
        public DatabaseWasNotCreatedException()
        { }

        public DatabaseWasNotCreatedException(string message): base(message)
        { }

        public DatabaseWasNotCreatedException(string message, Exception ex): base(message, ex)
        { }
    }
}
