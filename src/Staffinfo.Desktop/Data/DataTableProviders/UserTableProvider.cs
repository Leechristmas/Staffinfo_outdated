using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;
using Staffinfo.Desktop.Shared;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице USERS
    /// </summary>
    public class UserTableProvider: IDisposable
    {
        public string ErrorInfo { get; set; }

        /// <summary>
        /// Возвращает пользователя если авторизация удалась
        /// </summary>
        /// <param name="user">авторизуемый пользователь</param>
        /// <returns></returns>
        public UserModel Check(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM USERS WHERE USER_LOGIN = @USER_LOGIN AND PASSWORD = @USER_PASSWORD;");

            UserModel userModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                userModel = new UserModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    Login = sqlDataReader[1].ToString(),
                    Password = sqlDataReader[2].ToString(),
                    AccessLevel = int.Parse(sqlDataReader[3].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return userModel;
        }

        /// <summary>
        /// Возвращает пользователя если авторизация удалась
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public UserModel Check(string login, string password )
        {
            if (login == null || login.Length < 5) throw new AuthorizationException(Resources.UserTableProvider_Check_Некорректный_логин);
            if (password == null || password.Length < 5) throw new AuthorizationException(Resources.UserTableProvider_Check_Некорректный_пароль);

            var cmd = new SqlCommand($@"CHECK_USER '{login.ToUpper()}', '{password}';");

            //SqlParameter loginP = cmd.Parameters.Add("@USER_LOGIN", SqlDbType.VarChar);
            //SqlParameter passwordP = cmd.Parameters.Add("@USER_PASSWORD", SqlDbType.VarChar);
            //loginP.Value = login.ToUpper();
            //passwordP.Value = password.ToUpper();

            UserModel userModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                userModel = new UserModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    Login = sqlDataReader[1].ToString(),
                    Password = sqlDataReader[2].ToString(),
                    AccessLevel = int.Parse(sqlDataReader[3].ToString()),
                    LastName = sqlDataReader[4].ToString(),
                    FirstName = sqlDataReader[5].ToString(),
                    MiddleName = sqlDataReader[6].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return userModel;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public UserModel Save(UserModel obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel Select(long? id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает всех пользователей    
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<UserModel> Select()
        {
            var userList = new ObservableCollection<UserModel>();

            var cmd = new SqlCommand("GET_ALL_USERS");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var user = new UserModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        Login = sqlDataReader["USER_LOGIN"].ToString(),
                        Password = sqlDataReader["USER_PASSWORD"].ToString(),
                        AccessLevel = int.Parse(sqlDataReader["ACCESS_LEVEL"].ToString()),
                        LastName = sqlDataReader["LAST_NAME"].ToString(),
                        FirstName = sqlDataReader["FIRST_NAME"].ToString(),
                        MiddleName = sqlDataReader["MIDDLE_NAME"].ToString()
                    };

                    userList.Add(user);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return userList;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(UserModel obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ObservableCollection<UserModel> SelectByEmployeeId(long? id)
        {
            throw new NotImplementedException();
        }

        #region IDisposable implementation

        private bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || disposing)
                return;

            _disposed = true;
        }

        #endregion 
    }
}