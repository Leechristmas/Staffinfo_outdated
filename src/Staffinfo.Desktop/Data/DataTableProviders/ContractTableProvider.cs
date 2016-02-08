using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для таблицы Contract
    /// </summary>
    public class ContractTableProvider: IWritableTableContract<ContractModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableTableContract implementation

        /// <summary>
        /// Сохранить запись о контракте в БД
        /// </summary>
        /// <param name="contract">Контракт</param>
        /// <returns></returns>
        public ContractModel Save(ContractModel contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($@"INSERT INTO CONTRACT VALUES({contract.EmployeeId}, '{contract.StartDate}', '{contract.FinishDate}', '{contract.Description}'); SELECT MAX(ID) FROM CONTRACT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                contract.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return contract;
        }

        /// <summary>
        /// Возвращает контракт по id
        /// </summary>
        /// <param name="id">id контракта</param>
        /// <returns></returns>
        public ContractModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM CONTRACT WHERE ID={id};");

            ContractModel contractModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                contractModel = new ContractModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                    StartDate = DateTime.Parse(sqlDataReader[2].ToString()),
                    FinishDate = DateTime.Parse(sqlDataReader[3].ToString()),
                    Description = sqlDataReader[4].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return contractModel;
        }

        /// <summary>
        /// Возвращает список контрактов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ContractModel> Select()
        {
            var contractList = new ObservableCollection<ContractModel>();

            var cmd = new SqlCommand("SELECT * FROM CONTRACT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var contractModel = new ContractModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        EmployeeId = Int64.Parse(sqlDataReader[1].ToString()),
                        StartDate = DateTime.Parse(sqlDataReader[2].ToString()),
                        FinishDate = DateTime.Parse(sqlDataReader[3].ToString()),
                        Description = sqlDataReader[4].ToString()
                    };

                    contractList.Add(contractModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return contractList;
        }

        /// <summary>
        /// Обновить запись о контракте
        /// </summary>
        /// <param name="contract">Контракт</param>
        /// <returns></returns>
        public bool Update(ContractModel contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE CONTRACT SET EMPLOYEE_ID={contract.EmployeeId}, START_DATE='{contract.StartDate}', FINISH_DATE='{contract.FinishDate}', DESCRIPTION='{contract.Description}' WHERE ID={contract.Id};");

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
        /// Удалить контракт по id
        /// </summary>
        /// <param name="id">id контракта</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM CONTRACT WHERE ID = '{id}'");
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
