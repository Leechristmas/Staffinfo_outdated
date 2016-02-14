using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Staffinfo.Desktop.Data.DataTableContracts;
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

        /// <summary>
        /// Конвертирует из массива байт в BitmapImage TODO!!!
        /// </summary>
        /// <param name="imageBytes">массив байт</param>
        /// <returns></returns>
        private BitmapImage ByteToImage(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.DecodePixelHeight = 100;
                image.DecodePixelWidth = 100;
                image.CacheOption = BitmapCacheOption.OnLoad;;
                image.EndInit();

                return image;
            }
        }

        /// <summary>
        /// Конвертирует картинку в массив байт
        /// </summary>
        /// <param name="image">исходное изображение</param>
        /// <returns></returns>
        private byte[] ImageToByte(BitmapImage image)
        {
            if (image == null) return null;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Сравнивает 2 изображения
        /// </summary>
        /// <param name="btm1">первое изображение</param>
        /// <param name="btm2">второе изображение</param>
        /// <returns></returns>
        private bool ImageCompare(BitmapImage btm1, BitmapImage btm2)
        {
            return Convert.ToBase64String(ImageToByte(btm1))
                == Convert.ToBase64String(ImageToByte(btm2));
        }

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
                $"'{employee.JobStartDate.Value}', '{employee.Address}', '{employee.Pasport}', '{employee.MobilePhoneNumber}', '{employee.HomePhoneNumber}', '{employee.IsPensioner}', {null}); "+
                "SELECT MAX(ID) FROM EMPLOYEE;");

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

                //Паспорт
                var pasport = sqlDataReader["PASPORT"].ToString();
                //Реквизитный состав паспорта
                var pasportProps = pasport.Split('#');

                //Фотография
                var ms = new MemoryStream();
                BitmapImage photo = null;
                if (sqlDataReader["PHOTO"].ToString() != "")
                {
                    ms.Write((byte[])sqlDataReader["PHOTO"], 0, ((byte[])sqlDataReader["PHOTO"]).Length);
                    photo = ByteToImage(ms.ToArray());
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
                    Address = address,
                    City = addressProps[0],
                    Street = addressProps[1],
                    House = addressProps[2],
                    Flat = addressProps[3],
                    Pasport = pasport,
                    PasportOrganizationUnit = pasportProps[0],
                    PasportSeries = pasportProps[1],
                    PasportNumber = pasportProps[2],
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

                    //Паспорт
                    var pasport = sqlDataReader["PASPORT"].ToString();
                    //Реквизитный состав паспорта
                    var pasportProps = pasport.Split('#');

                    //Фотография
                    var ms = new MemoryStream();
                    BitmapImage photo = null;
                    if (sqlDataReader["PHOTO"].ToString() != "")
                    {
                        ms.Write((byte[])sqlDataReader["PHOTO"], 0, ((byte[])sqlDataReader["PHOTO"]).Length);
                        photo = ByteToImage(ms.ToArray());
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
                        Address = address,
                        City = addressProps[0],
                        Street = addressProps[1],
                        House = addressProps[2],
                        Flat = addressProps[3],
                        Pasport = pasport,
                        PasportOrganizationUnit = pasportProps[0],
                        PasportSeries = pasportProps[1],
                        PasportNumber = pasportProps[2],
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
            
            var cmd = new SqlCommand($@"UPDATE EMPLOYEE SET EMPLOYEE_FIRSTNAME='{employee.FirstName}', EMPLOYEE_MIDDLENAME='{employee.MiddleName}'," + 
                $"EMPLOYEE_LASTNAME='{employee.LastName}', PERSONAL_KEY='{employee.PersonalNumber}', POST_ID={employee.PostId}," +
                $"RANK_ID={employee.RankId}, BORN_DATE='{employee.BornDate.Value}', JOB_START_DATE='{employee.JobStartDate.Value}'," +
                $"ADDRESS='{employee.Address}', PASPORT='{employee.Pasport}', MOBILE_PHONE_NUMBER='{employee.MobilePhoneNumber}'," +
                $"HOME_PHONE_NUMBER='{employee.HomePhoneNumber}', IS_PENSIONER='{employee.IsPensioner}', PHOTO=@Photo " +
                $"WHERE ID={employee.Id};");

            SqlParameter param = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary);
            param.Value = ImageToByte(employee.Photo);


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
