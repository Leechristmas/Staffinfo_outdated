using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class PasportTableProviderTests
    {
        [TestMethod]
        [Description("Добавление и удаление паспорта")]
        public void AddPasportTest()
        {
            using (var pasportTp = new PasportTableProvider())
            {
                var pasport = new PasportModel
                {
                    Number = "228329",
                    OrganizationUnit = "паспортный стол 1",
                    Series = "HB"
                };

                pasport = pasportTp.Save(pasport);

                Assert.IsNotNull(pasport);
                Assert.IsNotNull(pasport.Id);

                Assert.IsTrue(pasportTp.DeleteById(pasport.Id));
            }
        }

        [TestMethod]
        [Description("Получение и обновление паспортов из БД")]
        public void GetPasportTest()
        {
            using (var pasportTp = new PasportTableProvider())
            {
                var pasportList = pasportTp.Select();
                Assert.IsNotNull(pasportList);

                var pasport = pasportTp.Select(pasportList[0].Id);
                Assert.IsNotNull(pasport);

                pasport.Number = "000000";
                Assert.IsTrue(pasportTp.Update(pasport));
            }
        }

        //[TestMethod]
        //[Description("Добавление и удаление паспортного стола")]
        //public void AddPasportOrganizationUnit()
        //{
        //    using (var pasportOrgUnitTp = new PasportOrganizationUnitTableProvider())
        //    {
        //        var orgUnit = new PasportOrganizationUnitModel()
        //        {
        //            OrganizationUnitName = "Паспортный стол 1",
        //            Address = "Где-то"
        //        };

        //        orgUnit = pasportOrgUnitTp.Save(orgUnit);

        //        Assert.IsNotNull(orgUnit);
        //        Assert.IsNotNull(orgUnit.Id);

        //        Assert.IsTrue(pasportOrgUnitTp.DeleteById(orgUnit.Id));
        //    }
        //}

        //[TestMethod]
        //[Description("Получение и обновление паспортных столов из БД")]
        //public void GetPasportOrganizationUnitTest()
        //{
        //    using (var pasportOrgUnitTp = new PasportOrganizationUnitTableProvider())
        //    {
        //        var pasportList = pasportOrgUnitTp.Select();
        //        Assert.IsNotNull(pasportList);

        //        var pasport = pasportOrgUnitTp.Select(pasportList[0].Id);
        //        Assert.IsNotNull(pasport);

        //        pasport.Address = "на севере за стеной";
        //        Assert.IsTrue(pasportOrgUnitTp.Update(pasport));
        //    }
        //}
    }
}