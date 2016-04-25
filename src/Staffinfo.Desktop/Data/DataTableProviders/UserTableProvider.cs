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

            var cmd = new SqlCommand($@"CHECK_USER @USER_LOGIN, @USER_PASSWORD;");

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
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
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

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            finally
            {
                sqlDataReader?.Close();
            }

            return userModel;
        }

        /// <summary>
        /// Сохраняет учетную запись
        /// </summary>
        /// <param name="user">учетная запись</param>
        /// <returns></returns>
        public UserModel Save(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"ADD_USERS '{user.Login}', '{user.Password}', '{user.AccessLevel}', '{user.LastName}', '{user.FirstName}', '{user.MiddleName}';");
            
            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                user.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return user;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"GET_USER {id};");

            UserModel user = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                user = new UserModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    Login = sqlDataReader["USER_LOGIN"].ToString(),
                    Password = sqlDataReader["USER_PASSWORD"].ToString(),
                    AccessLevel = int.Parse(sqlDataReader["ACCESS_LEVEL"].ToString()),
                    LastName = sqlDataReader["LAST_NAME"].ToString(),
                    FirstName = sqlDataReader["FIRST_NAME"].ToString(),
                    MiddleName = sqlDataReader["MIDDLE_NAME"].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return user;
        }

        /// <summary>
        /// Возвращает всех пользователей    
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<UserModel> Select()
        {
            var userList = new ObservableCollection<UserModel>();

            var cmd = new SqlCommand("GET_USER");

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
        /// Обновляет пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Update(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE USERS SET USER_LOGIN = '{user.Login}', USER_PASSWORD = '{user.Password}', ACCESS_LEVEL = {user.AccessLevel}, LAST_NAME = '{user.LastName}', FIRST_NAME = '{user.FirstName}', MIDDLE_NAME = '{user.MiddleName}' WHERE ID={user.Id};");

            try
            {
                DataSingleton.Instance.DatabaseConnector.Execute(cmd);
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Обновляет пароль пользователя
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <param name="password">новый пароль</param>
        /// <returns></returns>
        public bool UpdatePassword(long? userId, string password)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE USERS SET USER_PASSWORD = '{password}' WHERE ID = {userId}");

            try
            {
                DataSingleton.Instance.DatabaseConnector.Execute(cmd);
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удаляет пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM USERS WHERE ID = '{id}'");
            try
            {
                DataSingleton.Instance.DatabaseConnector.Execute(cmd);
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return false;
            }
            return true;
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