using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Helpers;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент для доступа к таблице Employee
    /// </summary>
    public class EmployeeTableProvider: IDisposable, IWritableTableContract<EmployeeModel>
    {
        public string ErrorInfo { get; set; }
        
        #region IWritableTableContract implementation

        /// <summary>
        /// Сохраняет служащего в БД
        /// </summary>
        /// <param name="employee">Служащий</param>
        /// <returns></returns>
        public EmployeeModel Save(EmployeeModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($"INSERT INTO EMPLOYEE VALUES('{employee.FirstName}', '{employee.MiddleName}', '{employee.LastName}'," + 
                $"'{employee.PersonalNumber}', {employee.PostId}, {employee.RankId}, '{employee.BornDate.Value}'," + 
                $"'{employee.JobStartDate.Value}', '{employee.Address}', {employee.PasportId}, '{employee.MobilePhoneNumber}', '{employee.HomePhoneNumber}', '{employee.IsPensioner}', @Photo); " +
                "SELECT MAX(ID) FROM EMPLOYEE;");

            var param = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary);
            param.Value = DBNull.Value;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                employee.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();
                
                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return employee;
        }

        /// <summary>
        /// Возвращает служащего по id
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public EmployeeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM EMPLOYEE WHERE ID={id};");

            EmployeeModel employeeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                //Адрес
                var address = sqlDataReader["ADDRESS"].ToString();
                //Реквизитный состав адреса
                var addressProps = address.Split('#');
                
                //Фотография
                var ms = new MemoryStream();
                BitmapImage photo = null;
                if (sqlDataReader["PHOTO"].ToString() != "")
                {
                    ms.Write((byte[])sqlDataReader["PHOTO"], 0, ((byte[])sqlDataReader["PHOTO"]).Length);
                    photo = BitmapImageHelper.ByteToImage(ms.ToArray());
                }

                employeeModel = new EmployeeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    FirstName = sqlDataReader["EMPLOYEE_FIRSTNAME"].ToString(),
                    LastName = sqlDataReader["EMPLOYEE_LASTNAME"].ToString(),
                    MiddleName = sqlDataReader["EMPLOYEE_MIDDLENAME"].ToString(),
                    PersonalNumber = sqlDataReader["PERSONAL_KEY"].ToString(),
                    PostId = Int64.Parse(sqlDataReader["POST_ID"].ToString()),
                    RankId = Int64.Parse(sqlDataReader["RANK_ID"].ToString()),
                    BornDate = DateTime.Parse(sqlDataReader["BORN_DATE"].ToString()),
                    JobStartDate = DateTime.Parse(sqlDataReader["JOB_START_DATE"].ToString()),
                    City = addressProps[0],
                    Street = addressProps[1],
                    House = addressProps[2],
                    Flat = addressProps[3],
                    PasportId = int.Parse(sqlDataReader["PASPORT_ID"].ToString()),
                    MobilePhoneNumber = sqlDataReader["MOBILE_PHONE_NUMBER"].ToString(),
                    HomePhoneNumber = sqlDataReader["HOME_PHONE_NUMBER"].ToString(),
                    IsPensioner = bool.Parse(sqlDataReader["IS_PENSIONER"].ToString()),
                    Photo = photo
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return employeeModel;
        }

        /// <summary>
        /// Возвращает всех служащих
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EmployeeModel> Select()
        {
            var employeeList = new ObservableCollection<EmployeeModel>();

            var cmd = new SqlCommand("SELECT * FROM EMPLOYEE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                
                while (sqlDataReader.Read())
                {
                    //Адрес
                    var address = sqlDataReader["ADDRESS"].ToString();
                    //Реквизитный состав адреса
                    var addressProps = address.Split('#');
                    
                    //Фотография
                    var ms = new MemoryStream();
                    BitmapImage photo = null;
                    if (sqlDataReader["PHOTO"].ToString() != "")
                    {
                        ms.Write((byte[])sqlDataReader["PHOTO"], 0, ((byte[])sqlDataReader["PHOTO"]).Length);
                        photo = BitmapImageHelper.ByteToImage(ms.ToArray());
                    }

                    var employeeModel = new EmployeeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        FirstName = sqlDataReader["EMPLOYEE_FIRSTNAME"].ToString(),
                        LastName = sqlDataReader["EMPLOYEE_LASTNAME"].ToString(),
                        MiddleName = sqlDataReader["EMPLOYEE_MIDDLENAME"].ToString(),
                        PersonalNumber = sqlDataReader["PERSONAL_KEY"].ToString(),
                        PostId = Int64.Parse(sqlDataReader["POST_ID"].ToString()),
                        RankId = Int64.Parse(sqlDataReader["RANK_ID"].ToString()),
                        BornDate = DateTime.Parse(sqlDataReader["BORN_DATE"].ToString()),
                        JobStartDate = DateTime.Parse(sqlDataReader["JOB_START_DATE"].ToString()),
                        City = addressProps[0],
                        Street = addressProps[1],
                        House = addressProps[2],
                        Flat = addressProps[3],
                        PasportId = int.Parse(sqlDataReader["PASPORT_ID"].ToString()),
                        MobilePhoneNumber = sqlDataReader["MOBILE_PHONE_NUMBER"].ToString(),
                        HomePhoneNumber = sqlDataReader["HOME_PHONE_NUMBER"].ToString(),
                        IsPensioner = bool.Parse(sqlDataReader["IS_PENSIONER"].ToString()),
                        Photo = photo
                    };

                    employeeList.Add(employeeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return employeeList;
        }

        /// <summary>
        /// Обновляет служащего
        /// </summary>
        /// <param name="employee">Служащий</param>
        /// <returns></returns>
        public bool Update(EmployeeModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE EMPLOYEE SET EMPLOYEE_FIRSTNAME=@EMPLOYEE_FIRSTNAME, EMPLOYEE_MIDDLENAME=@EMPLOYEE_MIDDLENAME," + 
                $"EMPLOYEE_LASTNAME=@EMPLOYEE_LASTNAME, PERSONAL_KEY=@PERSONAL_KEY, POST_ID=@POST_ID," +
                $"RANK_ID=@RANK_ID, BORN_DATE=@BORN_DATE, JOB_START_DATE=@JOB_START_DATE," +
                $"ADDRESS=@ADDRESS, PASPORT_ID=@PASPORT_ID, MOBILE_PHONE_NUMBER=@MOBILE_PHONE_NUMBER," +
                $"HOME_PHONE_NUMBER=@HOME_PHONE_NUMBER, IS_PENSIONER=@IS_PENSIONER, PHOTO=@Photo " +
                $"WHERE ID=@ID;");

            //Инициализация параметров
            var employeeFirstName = cmd.Parameters.Add("@EMPLOYEE_FIRSTNAME", SqlDbType.VarChar);
            employeeFirstName.Value = employee.FirstName;

            var employeeMiddleName = cmd.Parameters.Add("@EMPLOYEE_MIDDLENAME", SqlDbType.VarChar);
            employeeMiddleName.Value = employee.MiddleName;

            var employeeLastName = cmd.Parameters.Add("@EMPLOYEE_LASTNAME", SqlDbType.VarChar);
            employeeLastName.Value = employee.LastName;

            var personalKey = cmd.Parameters.Add("@PERSONAL_KEY", SqlDbType.VarChar);
            personalKey.Value = employee.PersonalNumber;

            var postId = cmd.Parameters.Add("@POST_ID", SqlDbType.Int);
            postId.Value = employee.PostId;

            var rankId = cmd.Parameters.Add("@RANK_ID", SqlDbType.Int);
            rankId.Value = employee.RankId;

            var bornDate = cmd.Parameters.Add("@BORN_DATE", SqlDbType.DateTime);
            bornDate.Value = employee.BornDate.Value;

            var jobStartDate = cmd.Parameters.Add("@JOB_START_DATE", SqlDbType.DateTime);
            jobStartDate.Value = employee.JobStartDate.Value;

            var address = cmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar);
            address.Value = employee.Address;

            var pasportId = cmd.Parameters.Add("@PASPORT_ID", SqlDbType.Int);
            pasportId.Value = employee.PasportId;

            var mobilePhoneNumber = cmd.Parameters.Add("@MOBILE_PHONE_NUMBER", SqlDbType.VarChar);
            mobilePhoneNumber.Value = employee.MobilePhoneNumber;

            var homePhoneNumber = cmd.Parameters.Add("@HOME_PHONE_NUMBER", SqlDbType.VarChar);
            homePhoneNumber.Value = employee.HomePhoneNumber;

            var isPensioner = cmd.Parameters.Add("@IS_PENSIONER", SqlDbType.Bit);
            isPensioner.Value = employee.IsPensioner;

            var id = cmd.Parameters.Add("@ID", SqlDbType.Int);
            id.Value = employee.Id;

            var param = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary);
            if (employee.Photo == null)
                param.Value = DBNull.Value;
            else param.Value = BitmapImageHelper.ImageToByte(employee.Photo);


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
        /// Удалить служащего по id
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM EMPLOYEE WHERE ID = '{id}'");
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

        #endregion

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
