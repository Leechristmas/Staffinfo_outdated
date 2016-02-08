using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class RankTableProviderTests
    {
        [TestMethod]
        public void GetAllRanksFromDb()
        {
            using(var prvdr = new RankTableProvider())
            {
                var rankList = prvdr.Select();

                Assert.IsNotNull(rankList);
                Assert.IsTrue(rankList.Count > 0);
            }
        }

        [TestMethod]
        public void GetElementByIdFromDb()
        {
            using(var prvdr = new RankTableProvider())
            {
                Assert.IsNotNull(prvdr.Select(2));
            }
        }
    }
}
