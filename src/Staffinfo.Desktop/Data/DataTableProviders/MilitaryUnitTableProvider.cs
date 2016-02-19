using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице MILITARY_UNIT
    /// </summary>
    public class MilitaryUnitTableProvider: IWritableTableContract<MilitaryUnitModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить военную часть
        /// </summary>
        /// <param name="militaryUnit">Военная часть</param>
        /// <returns></returns>
        public MilitaryUnitModel Save(MilitaryUnitModel militaryUnit)
        {
            if (militaryUnit == null) throw new ArgumentNullException(nameof(militaryUnit), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO MILITARY_UNIT VALUES('{militaryUnit.MilitaryName}', '{militaryUnit.Description}'); SELECT MAX(ID) FROM MILITARY_UNIT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                militaryUnit.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return militaryUnit;
        }

        /// <summary>
        /// Возвращает военную часть по id
        /// </summary>
        /// <param name="id">id военной части</param>
        /// <returns></returns>
        public MilitaryUnitModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM MILITARY_UNIT WHERE ID={id};");

            MilitaryUnitModel militaryUnit = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                militaryUnit = new MilitaryUnitModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    MilitaryName = sqlDataReader[1].ToString(),
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

            return militaryUnit;
        }

        /// <summary>
        /// Возвращает список военных частей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MilitaryUnitModel> Select()
        {
            var militaryUnitList = new ObservableCollection<MilitaryUnitModel>();

            var cmd = new SqlCommand("SELECT * FROM MILITARY_UNIT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var militaryUnit = new MilitaryUnitModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        MilitaryName = sqlDataReader[1].ToString(),
                        Description = sqlDataReader[2].ToString()
                    };

                    militaryUnitList.Add(militaryUnit);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return militaryUnitList;
        }

        /// <summary>
        /// Обновить запись о военной части
        /// </summary>
        /// <param name="militaryUnit">военная часть</param>
        /// <returns></returns>
        public bool Update(MilitaryUnitModel militaryUnit)
        {
            if (militaryUnit == null) throw new ArgumentNullException(nameof(militaryUnit), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE MILITARY_UNIT SET ORGANIZATION_NAME='{militaryUnit.MilitaryName}', ADDRESS='{militaryUnit.Description}' WHERE ID={militaryUnit.Id};");

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
        /// Удалить военную часть по id
        /// </summary>
        /// <param name="id">id военной части</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM MILITARY_UNIT WHERE ID = '{id}'");
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