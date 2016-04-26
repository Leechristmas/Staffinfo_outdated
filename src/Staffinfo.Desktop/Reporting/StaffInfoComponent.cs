using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Reporting
{
    /// <summary>
    /// Компонент формирования данных по штату для отчета
    /// </summary>
    public class StaffInfoComponent
    {
        /// <summary>
        /// Формирует пустую таблицу для отчета штатной расстановки
        /// </summary>
        /// <returns></returns>
        private DataTable CreateStaffPlacementTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("Post") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("Rank") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("Name") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("PersonalNumber") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("BirthDate") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("Clasiness") {DataType = typeof (string)});
            dataTable.Columns.Add(new DataColumn("Phone") {DataType = typeof (string)});

            return dataTable;
        }

        public DataTable GetStaffPlacementTable()
        {
            List<EmployeeModel> employees = null;
            List<ServiceModel> services = null;
            DataTable dataTable = CreateStaffPlacementTable();

            int soldierCount = 0, officerCount = 0;

            //получаем служащих
            using (var prvdr = new EmployeeTableProvider())
            {
                employees = prvdr.Select().ToList();
            }

            //получаем службы
            using (var prvdr = new ServiceTableProvider())
            {
                services = prvdr.Select().ToList();
            }
            
            //сортируем службы
            services.Sort((a, b) =>
            {
                if (a.GroupId < b.GroupId) return -1;
                if (a.GroupId > b.GroupId) return 1;
                return 0;
            });

            //идем по службам
            foreach (var service in services)
            {
                //заголовок - название службы
                dataTable.Rows.Add(service.ServiceTitle, "", "", "", "", "", "");

                //вытягиваем сотрудников текущей службы и сортируем их по должности
                var selectedEmployees =
                    employees.Where(e => GetPost(e).ServiceId == service.Id).OrderBy(e => GetPost(e).PostWeight);

                foreach (var e in selectedEmployees)
                {
                    var dRow = dataTable.NewRow();
                    
                    dRow["Post"] = GetPost(e).PostTitle;
                    dRow["Rank"] = GetRank(e).RankTitle;
                    dRow["Name"] = e.LastName + " " + e.FirstName + " " + e.MiddleName;
                    dRow["PersonalNumber"] = e.PersonalNumber;
                    dRow["BirthDate"] = e.BornDate?.ToString("d");
                    dRow["Clasiness"] = GetClasiness(e)?.ClasinessLevel;
                    dRow["Phone"] = e.MobilePhoneNumber ?? e.HomePhoneNumber;

                    dataTable.Rows.Add(dRow);

                    if (GetRank(e).RankWeight >= 6) officerCount++;
                    else soldierCount++;
                }
            }

            dataTable.Rows.Add("", $"{officerCount}", $"{soldierCount}");
            return dataTable;
        }

        /// <summary>
        /// Возвращает должность служащего
        /// </summary>
        /// <param name="employee">Служащий</param>
        /// <returns></returns>
        private PostModel GetPost(EmployeeModel employee)
        {
            return DataSingleton.Instance.PostList.FirstOrDefault(p => p.Id == employee.PostId);
        }

        /// <summary>
        /// Возвращает звание служащего
        /// </summary>
        /// <param name="employee">служащий</param>
        /// <returns></returns>
        private RankModel GetRank(EmployeeModel employee)
        {
            return DataSingleton.Instance.RankList.FirstOrDefault(r => r.Id == employee.RankId);
        }

        /// <summary>
        /// Возвращает актуальную классность для служащего
        /// </summary>
        /// <param name="employee">служащий</param>
        /// <returns></returns>
        private ClasinessModel GetClasiness(EmployeeModel employee)
        {
            using (var prvdr = new ClasinessTableProvider())
            {
                return prvdr.SelectActualClasiness(employee.Id);
            }
        }

    }
}