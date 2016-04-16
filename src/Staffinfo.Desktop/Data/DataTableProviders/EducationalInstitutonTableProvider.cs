using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Класс для таблицы EDUCATIONAL_INSTITUTION
    /// </summary>
    public class EducationalInstitutonTableProvider: IWritableDirectoryTableContract<EducationalInstitutionModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract implementation

        /// <summary>
        /// Сохранить запись учреждение образования в БД
        /// </summary>
        /// <param name="educationalInstitution"></param>
        /// <returns></returns>
        public EducationalInstitutionModel Save(EducationalInstitutionModel educationalInstitution)
        {
            if (educationalInstitution == null) throw new ArgumentNullException(nameof(educationalInstitution), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd =
                new SqlCommand($@"INSERT INTO EDUCATIONAL_INSTITUTION VALUES('{educationalInstitution.InstituitionTitle}','{educationalInstitution.Description}', '{educationalInstitution.InstituitionType}'); SELECT MAX(ID) FROM EDUCATIONAL_INSTITUTION;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                educationalInstitution.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return educationalInstitution;
        }

        /// <summary>
        /// Возвращает учреждение образования по id
        /// </summary>
        /// <param name="id">id учреждения образования</param>
        /// <returns></returns>
        public EducationalInstitutionModel Select(long? id)
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
                    Description = sqlDataReader[2].ToString(),
                    InstituitionType = sqlDataReader[3].ToString()
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

        /// <summary>
        /// Возвращает список учреждений образования
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EducationalInstitutionModel> Select()
        {
            var educationalInstitutionList = new ObservableCollection<EducationalInstitutionModel>();

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
                        Description = sqlDataReader[2].ToString(),
                        InstituitionType = sqlDataReader[3].ToString()
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

        /// <summary>
        /// Обновить запись об учебном заведении
        /// </summary>
        /// <param name="educationalInstitution">учебное заведение</param>
        /// <returns></returns>
        public bool Update(EducationalInstitutionModel educationalInstitution)
        {
            if (educationalInstitution == null) throw new ArgumentNullException(nameof(educationalInstitution), Resources.DatabaseConnector_parameter_cannot_be_null);
            
            var cmd = new SqlCommand($@"UPDATE EDUCATIONAL_INSTITUTION SET INST_TITLE='{educationalInstitution.InstituitionTitle}', INST_TYPE='{educationalInstitution.InstituitionType}' WHERE ID={educationalInstitution.Id};");

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
        /// Удалить учебное заведение по id
        /// </summary>
        /// <param name="id">id учебного заведения</param>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает список учебных заведений по id служащего
        /// </summary>
        /// <param name="id">id служащего</param>
        /// <returns></returns>
        public ObservableCollection<EducationalInstitutionModel> SelectByEmployeeId(long? id)
        {
            var educationalInstitutionList = new ObservableCollection<EducationalInstitutionModel>();

            var cmd = new SqlCommand($"SELECT * FROM EDUCATIONAL_INSTITUTION WHERE EMPLOYEE_ID={id}");

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
