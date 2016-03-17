using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Desktop.Shared
{
    /// <summary>
    /// Исключение: ошибка авторизации
    /// </summary>
    public class AuthorizationException: Exception
    {
        public AuthorizationException()
        { }

        public AuthorizationException(string message): base(message)
        { }

        public AuthorizationException(string message, Exception ex): base(message, ex)
        { }
    }
}
