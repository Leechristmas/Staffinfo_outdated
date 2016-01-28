using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class RankTableProviderTests
    {
        [TestMethod]
        public void AddAndDropNewRankToDb()
        {
            using(var prvdr = new RankTableProvider())
            {
                var rank = new RankModel
                {
                    RankTitle = "тестовое звание"
                };

                Assert.IsNotNull(rank = prvdr.AddNewElement(rank) as RankModel);
                Assert.IsNotNull(rank.Id);
                Assert.IsTrue(prvdr.DeleteById(rank.Id));
            }
        }
        
        [TestMethod]
        public void UpdateRank()
        {
            using(var prvdr = new RankTableProvider())
            {
                var rank = new RankModel
                {
                    RankTitle = "тестовое звание 2"
                };

                Assert.IsTrue(prvdr.Update(rank));
            }
        }

        [TestMethod]
        public void GetAllRanksFromDb()
        {
            using(var prvdr = new RankTableProvider())
            {
                var rankList = prvdr.GetAllElements();

                Assert.IsNotNull(rankList);
                Assert.IsTrue(rankList.Count > 0);
            }
        }

        [TestMethod]
        public void GetElementByIdFromDb()
        {
            using(var prvdr = new RankTableProvider())
            {
                Assert.IsNotNull(prvdr.GetElementById(2));
            }
        }
    }
}
