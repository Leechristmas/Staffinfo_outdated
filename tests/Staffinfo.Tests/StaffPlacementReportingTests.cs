using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Reporting;

namespace Staffinfo.Tests
{
    [TestClass]
    public class StaffPlacementReportingTests
    {
        /// <summary>
        /// ПОстроение отчета штатной расстановки
        /// </summary>
        [TestMethod]
        public void GetStaffPlacementTableTest()
        {
            DataSingleton.Instance.DatabaseConnector = new DatabaseConnector("Data Source=DESKTOP-2B54QFI\\SQLEXPRESS;Initial Catalog=STAFFINFO_TESTS; Integrated Security=SSPI;");
            DataSingleton.Instance.DataInitialize();
            var reporting = new StaffInfoReports();
            reporting.Make();
        }
    }
}