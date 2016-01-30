using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class ServiceTableProviderTests
    {
        [TestMethod]
        public void AddNewServiceIntoDatabase()
        {
            using (var prvdr = new ServiceTableProvider())
            {
                var service = new ServiceModel
                {
                    ServiceTitle = "тестовая служба"
                };

                Assert.IsNotNull(service = prvdr.AddNewElement(service) as ServiceModel);
                Assert.IsNotNull(service.Id);
                Assert.IsTrue(prvdr.DeleteById(service.Id));
            }
        }

        [TestMethod]
        public void UpdateService()
        {
            using (var prvdr = new ServiceTableProvider())
            {
                var service = new ServiceModel
                {
                    ServiceTitle = "тестовая служба 2"
                };

                Assert.IsTrue(prvdr.Update(service));
            }
        }

        [TestMethod]
        public void GetAllServices_HasElements()
        {
            using (var prvdr = new ServiceTableProvider())
            {
                var serviceList = prvdr.GetAllElements();

                Assert.IsNotNull(serviceList);
                Assert.IsTrue(serviceList.Count > 0);
            }
        }

        [TestMethod]
        public void GetElementById()
        {
            using (var prvdr = new RankTableProvider())
            {
                Assert.IsNotNull(prvdr.GetElementById(2));
            }
        }
    }
}