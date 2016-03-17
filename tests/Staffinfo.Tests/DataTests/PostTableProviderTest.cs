using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class PostTableProviderTest
    {
        [TestMethod]
        public void GetPostById()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postModel = pPrvdr.Select(1);

                Assert.IsNotNull(postModel);
                Assert.IsTrue(postModel.GetType() == typeof(PostModel));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPostById_ThrowsArgumentNullException()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postModel = pPrvdr.Select(null);
            }
        }

        [TestMethod]
        public void GetAllPosts_HasElements()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postList = pPrvdr.Select();

                Assert.IsNotNull(postList);
                Assert.IsTrue(postList.Count > 0);
            }
        }
    }
}
