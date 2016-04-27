using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Shared;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model пользователя для отображения в списке
    /// </summary>
    public class PartialUserViewModel: WindowViewModelBase
    {
        public PartialUserViewModel()
        {}

        public PartialUserViewModel(UserModel user)
        {
            _id = user.Id;
            _login = user.Login;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _accessLevel = user.AccessLevel;
        }

        #region Fields

        private readonly long? _id;

        /// <summary>
        /// Имя
        /// </summary>
        private readonly string _firstName;

        /// <summary>
        /// Фамилия
        /// </summary>
        private readonly string _lastName;

        /// <summary>
        /// Уровень доступа
        /// </summary>
        private readonly int _accessLevel;

        /// <summary>
        /// Логин
        /// </summary>
        private readonly string _login;

        

        #endregion


        #region Properties

        public long? Id => _id;

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName => _firstName;

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName => _lastName;

        /// <summary>
        /// Логин
        /// </summary>
        public string Login => _login;

        #endregion

    }
}