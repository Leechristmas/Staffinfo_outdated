﻿using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Staffinfo.Desktop.Data.DataTableContracts;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Data.DataTableProviders
{
    /// <summary>
    /// Компонент доступа к таблице PASPORT_ORANIZATION_UNIT (паспортные столы) 
    /// </summary>
    public class PasportOrganizationUnitTableProvider: IWritableTableContract<PasportOrganizationUnitModel>, IDisposable
    {
        public string ErrorInfo { get; set; }

        #region IWritableDirectoryTableContract

        /// <summary>
        /// Сохранить паспортный стол
        /// </summary>
        /// <param name="pasportOrganizationUnit">Паспортный стол</param>
        /// <returns></returns>
        public PasportOrganizationUnitModel Save(PasportOrganizationUnitModel pasportOrganizationUnit)
        {
            if (pasportOrganizationUnit == null) throw new ArgumentNullException(nameof(pasportOrganizationUnit), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd =
                new SqlCommand($@"INSERT INTO PASPORT_ORGANIZATION_UNIT VALUES('{pasportOrganizationUnit.OrganizationUnitName}', '{pasportOrganizationUnit.Address}'); SELECT MAX(ID) FROM PASPORT_ORGANIZATION_UNIT;");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                sqlDataReader.Read();
                pasportOrganizationUnit.Id = Int64.Parse(sqlDataReader[0].ToString());
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return pasportOrganizationUnit;
        }

        /// <summary>
        /// Возвращает паспортный стол по id
        /// </summary>
        /// <param name="id">id паспортного стола</param>
        /// <returns></returns>
        public PasportOrganizationUnitModel Select(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"SELECT * FROM PASPORT_ORGANIZATION_UNIT WHERE ID={id};");

            PasportOrganizationUnitModel organizationUnit = null;

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);
                sqlDataReader.Read();

                organizationUnit = new PasportOrganizationUnitModel
                {
                    Id = Int64.Parse(sqlDataReader[0].ToString()),
                    OrganizationUnitName = sqlDataReader[1].ToString(),
                    Address = sqlDataReader[2].ToString()
                };
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }

            return organizationUnit;
        }

        /// <summary>
        /// Возвращает список паспортных столов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PasportOrganizationUnitModel> Select()
        {
            var organizationUnitList = new ObservableCollection<PasportOrganizationUnitModel>();

            var cmd = new SqlCommand("SELECT * FROM PASPORT_ORGANIZATION_UNIT");

            try
            {
                var sqlDataReader = DataSingleton.Instance.DatabaseConnector.ExecuteReader(cmd);

                while (sqlDataReader.Read())
                {
                    var organizationUnit = new PasportOrganizationUnitModel
                    {
                        Id = Int64.Parse(sqlDataReader[0].ToString()),
                        OrganizationUnitName = sqlDataReader[1].ToString(),
                        Address = sqlDataReader[2].ToString()
                    };

                    organizationUnitList.Add(organizationUnit);
                }
                sqlDataReader.Close();

                ErrorInfo = null;
            }
            catch (Exception ex)
            {
                ErrorInfo = Resources.DatabaseConnector_operation_error + ex.Message;
                return null;
            }
            return organizationUnitList;
        }

        /// <summary>
        /// Обновить запись о паспортном столе
        /// </summary>
        /// <param name="organizationUnit">паспортный стол</param>
        /// <returns></returns>
        public bool Update(PasportOrganizationUnitModel organizationUnit)
        {
            if (organizationUnit == null) throw new ArgumentNullException(nameof(organizationUnit), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"UPDATE PASPORT_ORGANIZATION_UNIT SET ORGANIZATION_NAME='{organizationUnit.OrganizationUnitName}', ADDRESS='{organizationUnit.Address}' WHERE ID={organizationUnit.Id};");

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
        /// Удалить паспортный стол по id
        /// </summary>
        /// <param name="id">id паспортного стола</param>
        /// <returns></returns>
        public bool DeleteById(long? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id), Resources.DatabaseConnector_parameter_cannot_be_null);

            var cmd = new SqlCommand($@"DELETE FROM PASPORT_ORGANIZATION_UNIT WHERE ID = '{id}'");
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