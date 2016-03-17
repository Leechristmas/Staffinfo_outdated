using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для работы с таблицей должностей
    /// </summary>
    public class RankTableProvider : IReadOnlyTableContract<RankModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IReadOnlyTableContract implementation
        
        /// <summary>
        /// Возвращает звание по id
        /// </summary>
        /// <param name="id">id звания</param>
        /// <returns></returns>
        public RankModel Select(long? id)
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

        /// <summary>
        /// Возвращает все звания
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RankModel> Select()
        {
            var rankList = new ObservableCollection<RankModel>();

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
