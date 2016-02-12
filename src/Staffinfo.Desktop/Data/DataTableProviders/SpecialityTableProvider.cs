using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице SPECIALITY
    /// </summary>
    public class SpecialityTableProvider: IWritableDirectoryTableContract<SpecialityModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить специальность
        /// </summary>
        /// <param name="speciality">специальность</param>
        /// <returns></returns>
        public SpecialityModel Save(SpecialityModel speciality)
        {
            if (speciality == null) throw new ArgumentNullException(nameof(speciality), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO SPECIALITY VALUES({speciality.Speciality}, '{speciality.Description}'); SELECT MAX(ID) FROM SPECIALITY;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                speciality.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return speciality;
        }

        /// <summary>
        /// Возвращает специальность по id
        /// </summary>
        /// <param name="id">id специальности</param>
        /// <returns></returns>
        public SpecialityModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM SPECIALITY WHERE ID={id};");

            SpecialityModel specialityModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                specialityModel = new SpecialityModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    Speciality = sqlDataReader[1].ToString(),
                    Description = sqlDataReader[2].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return specialityModel;
        }

        /// <summary>
        /// Возвращает список специальностей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SpecialityModel> Select()
        {
            var specialityList = new ObservableCollection<SpecialityModel>();

            var cmd = new SqlCommand("SELECT * FROM SPECIALITY");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var speciality = new SpecialityModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        Speciality = sqlDataReader[1].ToString(),
                        Description = sqlDataReader[2].ToString()
                    };

                    specialityList.Add(speciality);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return specialityList;
        }

        /// <summary>
        /// Обновить запись о специальности
        /// </summary>
        /// <param name="speciality">специальность</param>
        /// <returns></returns>
        public bool Update(SpecialityModel speciality)
        {
            if (speciality == null) throw new ArgumentNullException(nameof(speciality), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE SPECIALITY SET SPECIALITY='{speciality.Speciality}', DESCRIPTION='{speciality.Description}' WHERE ID={speciality.Id};");

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
        /// Удалить специальность по id
        /// </summary>
        /// <param name="id">id специальности</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM SPECIALITY WHERE ID = '{id}'");
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
        /// Возвращает список специальностей по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<SpecialityModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var specialityList = new ObservableCollection<SpecialityModel>();

            var cmd = new SqlCommand($"SELECT * FROM SPECIALITY WHERE EMPLOYEE_ID = {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var specialityModel = new SpecialityModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        Speciality = sqlDataReader[1].ToString(),
                        Description = sqlDataReader[2].ToString()
                    };

                    specialityList.Add(specialityModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return specialityList;
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