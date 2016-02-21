using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице USERS
    /// </summary>
    public class UserTableProvider: IDisposable
    {
        public string ErrorInfo { get; set; }

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

        public UserModel Check(string login, string password )
        {
            if (login == null || login.Length < 5) throw new ArgumentException(Resources.UserTableProvider_Check_Некорректный_логин,login);
            if (password == null || password.Length < 5) throw new ArgumentException(Resources.UserTableProvider_Check_Некорректный_пароль, password);

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

        public UserModel Save(UserModel obj)
        {
            throw new NotImplementedException();
        }

        public UserModel Select(long? id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<UserModel> Select()
        {
            throw new NotImplementedException();
        }

        public bool Update(UserModel obj)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(long? id)
        {
            throw new NotImplementedException();
        }

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