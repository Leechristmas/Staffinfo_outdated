using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для таблицы EDUCATIONAL_INSTITUTION
    /// </summary>
    public class EducationalInstitutonTableProvider: ITableProvider, IDisposable
    {
        public string ErrorInfo { get; private set; }

        #region ITableProvider implementation

        public BaseModel AddNewElement(BaseModel educationalInstitutionModel)
        {
            if (educationalInstitutionModel == null) throw new ArgumentNullException(nameof(educationalInstitutionModel), Resources.DatabaseConnector_parameter_cannot_be_null);

            var educationalInstitution = educationalInstitutionModel as EducationalInstitutionModel;

            var cmd =
                new SqlCommand($@"INSERT INTO EDUCATIONAL_INSTITUTION VALUES('{educationalInstitution.InstituitionTitle}','{educationalInstitution.Description}', '{educationalInstitution.InstituitionType}'); SELECT MAX(ID) FROM EDUCATIONAL_INSTITUTION;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                educationalInstitutionModel.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return educationalInstitutionModel;
        }

        public BaseModel GetElementById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM EDUCATIONAL_INSTITUTION WHERE ID={id};");

            EducationalInstitutionModel educationalInstitutionModel = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                educationalInstitutionModel = new EducationalInstitutionModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    InstituitionTitle = sqlDataReader[1].ToString(),
                    InstituitionType = sqlDataReader[2].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return educationalInstitutionModel;
        }

        public ObservableCollection<BaseModel> GetAllElements()
        {
            var educationalInstitutionList = new ObservableCollection<BaseModel>();

            var cmd = new SqlCommand("SELECT * FROM EDUCATIONAL_INSTITUTION");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var educationalInstitutionModel = new EducationalInstitutionModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        InstituitionTitle = sqlDataReader[1].ToString(),
                        InstituitionType = sqlDataReader[2].ToString()
                    };

                    educationalInstitutionList.Add(educationalInstitutionModel);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return educationalInstitutionList;
        }

        public bool Update(BaseModel educationalInstitution)
        {
            if (educationalInstitution == null) throw new ArgumentNullException(nameof(educationalInstitution), Resources.DatabaseConnector_parameter_cannot_be_null);

            var educationalInstitutionModel = educationalInstitution as EducationalInstitutionModel;

            var cmd = new SqlCommand($@"UPDATE EDUCATIONAL_INSTITUTION SET INST_TITLE='{educationalInstitutionModel.InstituitionTitle}', INST_TYPE='{educationalInstitutionModel.InstituitionType}' WHERE ID={educationalInstitutionModel.Id};");

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

            var cmd = new SqlCommand($@"DELETE FROM EDUCATIONAL_INSTITUTION WHERE ID = '{id}'");
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
