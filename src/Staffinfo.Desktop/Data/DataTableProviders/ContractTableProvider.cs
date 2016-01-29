using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для таблицы Contract
    /// </summary>
    public class ContractTableProvider: ITableProvider, IDisposable
    {
        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract), Resources.DatabaseConnector_parameter_cannot_be_null);

            var contractModel = contract as ContractModel;

            var cmd =
                new SqlCommand($@"INSERT INTO CONTRACT VALUES({contractModel.EmployeeId}, '{contractModel.StartDate}', '{contractModel.FinishDate}', '{contractModel.Description}'); SELECT MAX(ID) FROM CONTRACT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                contractModel.Id = Int64.Parse(sqlDataReader[0].ToString());
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

        public string ErrorInfo { get; set; }

        public BaseModel GetElementById(long? id)
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

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var contractList = new ObservableCollection<BaseModel>();

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

        public bool Update(BaseModel contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract), Resources.DatabaseConnector_parameter_cannot_be_null);

            var contractModel = contract as ContractModel;

            var cmd = new SqlCommand($@"UPDATE CONTRACT SET EMPLOYEE_ID={contractModel.EmployeeId}, START_DATE='{contractModel.StartDate}', FINISH_DATE='{contractModel.FinishDate}', DESCRIPTION='{contractModel.Description}' WHERE ID={contractModel.Id};");

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
