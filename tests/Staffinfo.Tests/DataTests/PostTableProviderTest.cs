using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests
{
    [TestClass]
    public class PostTableProviderTest
    {
        [TestMethod]
        public void AddNewPostIntoDatabaseIsSucces()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var post = pPrvdr.AddNewElement(new PostModel() {Id = null, PostTitle = "Лейтенант", ServiceId = 1});

                Assert.IsNotNull(post.Id);
                Assert.IsTrue(pPrvdr.DeleteById(post.Id));
            }
        }

        [TestMethod]
        public void GetElementById()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postModel = pPrvdr.GetElementById(1) as PostModel;

                Assert.IsNotNull(postModel);
                Assert.IsTrue(postModel.GetType() == typeof(PostModel));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetElementById_ThrowsArgumentNullException()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postModel = pPrvdr.GetElementById(null) as PostModel;
            }
        }

        [TestMethod]
        public void GetAllElements_HasElements()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                var postList = pPrvdr.GetAllElements();

                Assert.IsNotNull(postList);
                Assert.IsTrue(postList.Count > 0);
            }
        }

        [TestMethod]
        public void UpdateElement()
        {
            using (var pPrvdr = new PostTableProvider())
            {
                Assert.IsTrue(pPrvdr.Update(new PostModel {Id = 1, PostTitle = "изменен", ServiceId = 1}));
            }
        }


    }
}
