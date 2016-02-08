using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class ClasinessTableProviderTests
    {
        [TestMethod]
        public void AddNewClasinessIntoDatabase()
        {
            using (var prvdr = new ClasinessTableProvider())
            {
                var clasiness = prvdr.Save(new ClasinessModel
                {
                    EmployeeId = 1,
                    OrderNumber = 1,
                    ClasinessDate = DateTime.Now,
                    ClasinessLevel = 1,
                    Description = "тестовая запись(добавление)"
                });

                Assert.IsNotNull(clasiness);
                Assert.IsNotNull(clasiness.Id);

                Assert.IsTrue(prvdr.DeleteById(clasiness.Id));
            }
        }

        [TestMethod]
        public void GetClasinessById()
        {
            using (var prvdr = new ClasinessTableProvider())
            {
                var clasiness = prvdr.Select(1);

                Assert.IsNotNull(clasiness);
            }
        }

        [TestMethod]
        public void GetAllClasiness_HasElements()
        {
            using (var prvdr = new ClasinessTableProvider())
            {
                var clasinessList = prvdr.Select();

                Assert.IsNotNull(clasinessList);
                Assert.IsTrue(clasinessList.Count > 0);
            }
        }

        [TestMethod]
        public void UpdateClasiness()
        {
            using (var prvdr = new ClasinessTableProvider())
            {
                Assert.IsTrue(prvdr.Update(new ClasinessModel
                {
                    Id = 1,
                    EmployeeId = 1,
                    OrderNumber = 1,
                    ClasinessDate = DateTime.Now,
                    ClasinessLevel = 1,
                    Description = "тестовая запись"
                }));
            }
        }
    }
}
