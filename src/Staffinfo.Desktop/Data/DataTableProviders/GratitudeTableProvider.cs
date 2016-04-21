using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице GRATITUDE
    /// </summary>
    public class GratitudeTableProvider: IWritableDirectoryTableContract<GratitudeModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить запись о вынесении благодарности в БД
        /// </summary>
        /// <param name="gratitude">Вынесение благодарности</param>
        /// <returns></returns>
        public GratitudeModel Save(GratitudeModel gratitude)
        {
            if (gratitude == null) throw new ArgumentNullException(nameof(gratitude), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"ADD_GRATITUDE {gratitude.EmployeeId}, '{gratitude.Description}', '{gratitude.GratitudeDate}';");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                gratitude.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return gratitude;
        }

        /// <summary>
        /// Возвращает благодарность по id
        /// </summary>
        /// <param name="id">id контракта</param>
        /// <returns></returns>
        public GratitudeModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM GRATITUDE WHERE ID={id};");

            GratitudeModel gratitudeModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                gratitudeModel = new GratitudeModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    Description = sqlDataReader[2].ToString(),
                    GratitudeDate = DateTime.Parse(sqlDataReader[3].ToString())
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return gratitudeModel;
        }

        /// <summary>
        /// Возвращает список благодарностей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<GratitudeModel> Select()
        {
            var gratitudeList = new ObservableCollection<GratitudeModel>();

            var cmd = new SqlCommand("GET_GRATITUDE");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var gratitudeModel = new GratitudeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        GratitudeDate = DateTime.Parse(sqlDataReader[3].ToString())
                    };

                    gratitudeList.Add(gratitudeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return gratitudeList;
        }

        /// <summary>
        /// Обновить запись о благодарности
        /// </summary>
        /// <param name="gratitude">Благодарность</param>
        /// <returns></returns>
        public bool Update(GratitudeModel gratitude)
        {
            if (gratitude == null) throw new ArgumentNullException(nameof(gratitude), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE GRATITUDE SET EMPLOYEE_ID={gratitude.EmployeeId}, GRATITUDE_DATE='{gratitude.GratitudeDate}', DESCRIPTION='{gratitude.Description}' WHERE ID={gratitude.Id};");

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
        /// Удалить благодарность по id
        /// </summary>
        /// <param name="id">id контракта</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM GRATITUDE WHERE ID = '{id}'");
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
        /// Возвращает список благодарностей по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<GratitudeModel> SelectByEmployeeId(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var gratitudeList = new ObservableCollection<GratitudeModel>();

            var cmd = new SqlCommand($"GET_GRATITUDE {id}");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var gratitudeModel = new GratitudeModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        Description = sqlDataReader[2].ToString(),
                        GratitudeDate = DateTime.Parse(sqlDataReader[3].ToString())
                    };

                    gratitudeList.Add(gratitudeModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return gratitudeList;
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