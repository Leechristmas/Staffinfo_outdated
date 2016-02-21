using System;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view-model для пользователя
    /// </summary>
    public class UserViewModel: WindowViewModelBase
    {
        #region Fields

        private string _middleName;
        private string _firstName;
        private string _lastName;
        private int _accessLevel;
        private string _login;
        private readonly string _password;

        #endregion

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value.Trim();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Уровень доступа к БД
        /// </summary>
        public int AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                if(value < 0 || value > 1) throw new Exception("Неверный уровень доступа");
                _accessLevel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                if(value.Length > 20) throw new Exception("Слишком длинный логин");
                _login = value;
            }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if(value.Length > 20) throw new Exception("Слишком длинный пароль");
            }
        }
    }
}