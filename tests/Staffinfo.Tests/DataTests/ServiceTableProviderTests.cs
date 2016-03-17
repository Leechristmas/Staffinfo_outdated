using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class ServiceTableProviderTests
    {
        [TestMethod]
        public void GetAllServices_HasElements()
        {
            using (var prvdr = new ServiceTableProvider())
            {
                var serviceList = prvdr.Select();

                Assert.IsNotNull(serviceList);
                Assert.IsTrue(serviceList.Count > 0);
            }
        }

        [TestMethod]
        public void GetElementById()
        {
            using (var prvdr = new RankTableProvider())
            {
                Assert.IsNotNull(prvdr.Select(2));
            }
        }
    }
}