using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей должностей
    /// </summary>
    public class RankTableProvider : ITableProvider, IDisposable
    {
        public string ErrorInfo { get; private set; }

        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel rankModel)
        {
            if (rankModel == null) throw new ArgumentNullException(nameof(rankModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var rank = rankModel as RankModel;

            var cmd =
                new SqlCommand($@"INSERT INTO RANK VALUES('{rank.RankTitle}'); SELECT MAX(ID) FROM RANK;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                rank.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return rank;
        }

        public BaseModel GetElementById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM RANK WHERE ID={id};");

            RankModel rankModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                rankModel = new RankModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    RankTitle = sqlDataReader[1].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return rankModel;
        }

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var rankList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM RANK");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var rankModel = new RankModel()
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        RankTitle = sqlDataReader[1].ToString()
                    };

                    rankList.Add(rankModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return rankList;
        }

        public bool Update(BaseModel rank)
        {
            if (rank == null) throw new ArgumentNullException(nameof(rank), Resources.DatabaseConnector_parameter_cannot_be_null);

            var rankModel = rank as RankModel;

            var cmd = new SqlCommand($@"UPDATE RANK SET RANK_TITLE='{rankModel.RankTitle}' WHERE ID={rankModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM RANK WHERE ID = '{id}'");
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
