using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице PASPORT (паспорт)
    /// </summary>
    public class PasportTableProvider : IWritableTableContract<PasportModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить паспорт в БД
        /// </summary>
        /// <param name="pasport">паспорт</param>
        /// <returns></returns>
        public PasportModel Save(PasportModel pasport)
        {
            if (pasport == null) throw new ArgumentNullException(nameof(pasport), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO PASPORT VALUES('{pasport.OrganizationUnit}', '{pasport.Number}', '{pasport.Series}'); SELECT MAX(ID) FROM PASPORT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                pasport.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return pasport;
        }

        /// <summary>
        /// Возвращает паспорт по id
        /// </summary>
        /// <param name="id">id паспорта</param>
        /// <returns></returns>
        public PasportModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM PASPORT WHERE ID={id};");

            PasportModel pasport = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                pasport = new PasportModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    OrganizationUnit = sqlDataReader[1].ToString(),
                    Number = sqlDataReader[2].ToString(),
                    Series = sqlDataReader[3].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return pasport;
        }

        /// <summary>
        /// Возвращает все паспорта
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PasportModel> Select()
        {
            var pasportList = new ObservableCollection<PasportModel>();

            var cmd = new SqlCommand("SELECT * FROM PASPORT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var pasport = new PasportModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        OrganizationUnit = sqlDataReader[1].ToString(),
                        Number = sqlDataReader[2].ToString(),
                        Series = sqlDataReader[3].ToString()
                    };

                    pasportList.Add(pasport);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return pasportList;
        }

        /// <summary>
        /// Обновляет процесс обучения
        /// </summary>
        /// <param name="pasport">паспорт</param>
        /// <returns></returns>
        public bool Update(PasportModel pasport)
        {
            if (pasport == null) throw new ArgumentNullException(nameof(pasport), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE PASPORT SET ORGANIZATION_UNIT='{pasport.OrganizationUnit}', NUMBER='{pasport.Number}', SERIES='{pasport.Series}' WHERE ID={pasport.Id};");

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
        /// Удалить паспорт по id
        /// </summary>
        /// <param name="id">id паспорта</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM PASPORT WHERE ID = '{id}'");
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